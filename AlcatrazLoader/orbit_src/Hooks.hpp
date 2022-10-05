#pragma once

#include "Objects/OrbitConfig.hpp"
#include "Helpers/Failure.hpp"
#include "Utils/Singleton.hpp"

#include "ProfileManager.hpp"

#include "HF/HackingFrameworkFWD.h"

#include "HF/HackingFramework.hpp"
#include <minidumpapiset.h>

namespace Hermes
{
	enum LogChannel
	{
		LC_error = 0,
		LC_warning = 1,
		LC_info = 2,
		LC_sample = 3,
		LC_system_info = 4,
		LC_result = 5,
		LC_debug1 = 6,
		LC_debug2 = 7,
	};

	typedef void(__cdecl* LogCallback)(Hermes::LogChannel, const char*);
}

class SandboxSelector
{
public:
	// Default constructor in case if needed
	SandboxSelector()
	{
		strcpy_s(m_cOnlineConfigKey, 64, "885642bfde8842b79bbcf2c1f8102403");
		strcpy_s(m_cSandboxKey, 32, "w6kAtr3T");
		strcpy_s(m_cSandboxName, 32, "PC Sandbox PA");
		m_iSandboxTrackingID = 4;
	}

	char m_cOnlineConfigKey[64];
	char m_cSandboxKey[32];
	char m_cSandboxName[32];
	unsigned __int8 m_iSandboxTrackingID;
};

namespace AlcatrazUplayR2
{
	static constexpr uintptr_t kFriendsListPageLimitAddress = 0x009E24DE;
	static constexpr uintptr_t kOnlineConfigServiceHostPRODAddress = 0x016DD358;
	static constexpr uintptr_t kSandboxSelectorConstructorAddr = 0x004CF530;
	static constexpr size_t kSandboxSelectorConstructorPatchSize = 10;
	static constexpr size_t kPlatformListenerServiceRDVProcessSize = 10;
	static constexpr uintptr_t kHermesLogCallbackAddr = 0x016DB558;
	static Hermes::LogCallback& hermesLogCallbackVar = *reinterpret_cast<Hermes::LogCallback*>(kHermesLogCallbackAddr);
	static Hermes::LogCallback origOrDNGHookLogCallback = nullptr;

	struct HookProcess
	{
		HF::Win32::ProcessPtr MainProcess;
		HF::Win32::ModulePtr MainModule;
		HF::Hook::TrampolinePtr<kSandboxSelectorConstructorPatchSize> SandboxSelectorConstructorHook;
		HF::Hook::TrampolinePtr<kPlatformListenerServiceRDVProcessSize> PlatformListenerServiceRDVProcessHook;
	};

	void OnlineLogCallback(Hermes::LogChannel channel, const char* msg)
	{
		if (origOrDNGHookLogCallback) {
			origOrDNGHookLogCallback(channel, msg);
		}

		std::string message;

		// parse for presence
		if (strstr(msg, "Finished task")) {
			if (strstr(msg, "TaskRDV_Signin") || strstr(msg, "TaskRDV_LeaveParty")) {
				message = "In game";
			} else if (strstr(msg, "TaskRDV_CreateParty") || strstr(msg, "TaskRDV_JoinParty") || strstr(msg, "TaskRDV2_LeaveSession")) {
				message = "In online lobby";
			} else if (strstr(msg, "TaskRDV_JoinSessionStatus")) {
				message = "In online game session";
			} else if (strstr(msg, "TaskRDV2_ListSessions")) {
				message = "Searching for online matches";
			}
		}

		if (message.length()) {
			auto& profile = Singleton<ProfileData>::Instance().Get();

			DiscordRichPresence passMe = { 0 };
			GetDiscordManager()->SetStatus(passMe, (const char*)profile.AccountId, message);
		}
	}

	inline void WriteMiniDump(EXCEPTION_POINTERS* exception = nullptr)
	{
		//
		//	Credits https://stackoverflow.com/questions/5028781/how-to-write-a-sample-code-that-will-crash-and-produce-dump-file
		//
		auto hDbgHelp = LoadLibraryA("dbghelp");
		if (hDbgHelp == nullptr)
			return;
		auto pMiniDumpWriteDump = (decltype(&MiniDumpWriteDump))GetProcAddress(hDbgHelp, "MiniDumpWriteDump");
		if (pMiniDumpWriteDump == nullptr)
			return;

		char name[MAX_PATH];
		{
			auto nameEnd = name + GetModuleFileNameA(GetModuleHandleA(0), name, MAX_PATH);
			SYSTEMTIME t;
			GetSystemTime(&t);

			wsprintfA(nameEnd - strlen(".exe"),
				"_%4d%02d%02d_%02d%02d%02d.dmp",
				t.wYear, t.wMonth, t.wDay, t.wHour, t.wMinute, t.wSecond);
		}

		auto hFile = CreateFileA(name, GENERIC_WRITE, FILE_SHARE_READ, 0, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
		if (hFile == INVALID_HANDLE_VALUE)
			return;

		MINIDUMP_EXCEPTION_INFORMATION exceptionInfo;
		exceptionInfo.ThreadId = GetCurrentThreadId();
		exceptionInfo.ExceptionPointers = exception;
		exceptionInfo.ClientPointers = FALSE;

		auto dumped = pMiniDumpWriteDump(
			GetCurrentProcess(),
			GetCurrentProcessId(),
			hFile,
			MINIDUMP_TYPE(MiniDumpWithIndirectlyReferencedMemory | MiniDumpScanMemory),
			exception ? &exceptionInfo : nullptr,
			nullptr,
			nullptr);

		CloseHandle(hFile);
	}

	inline void NotifyAboutException(EXCEPTION_POINTERS* exceptionInfoFrame)
	{
		MessageBox(
			NULL,
			"We got an fatal error.\nMinidump will be saved near exe.",
			"Driver San Francisco",
			MB_ICONERROR | MB_OK
		);

		WriteMiniDump(exceptionInfoFrame);
		exit(0);
	}

	inline LONG __stdcall ExceptionFilterWin32(EXCEPTION_POINTERS* exceptionInfoFrame)
	{
		if (exceptionInfoFrame->ExceptionRecord->ExceptionCode < 0x80000000)
		{
			return EXCEPTION_CONTINUE_EXECUTION;
		}

		NotifyAboutException(exceptionInfoFrame);

		return EXCEPTION_EXECUTE_HANDLER;
	}

	inline void InitDiscordRichPresence()
	{
		auto& profile = Singleton<ProfileData>::Instance().Get();

		CDiscord::CreateInstance();

		//Enable it, this needs to be set via a config file of some sort. 
		GetDiscordManager()->SetUseDiscord(true);

		DiscordRichPresence passMe = { 0 };
		GetDiscordManager()->SetStatus(passMe, (const char*)profile.AccountId, "Started the game");

		// DNGHook-friendly callback
		Hermes::LogCallback oldCallback = hermesLogCallbackVar;
		hermesLogCallbackVar = OnlineLogCallback;
		origOrDNGHookLogCallback = oldCallback;
	}

	inline void __stdcall OnSandboxSelectorConstructor(SandboxSelector* self)
	{
		auto& profile = Singleton<ProfileData>::Instance().Get();

		char* str = *(char**)kOnlineConfigServiceHostPRODAddress;
		strcpy_s(str, 28, profile.ServiceUrl);

		strcpy_s(self->m_cOnlineConfigKey, 64, profile.ConfigKey);
		strcpy_s(self->m_cSandboxKey, 32, profile.SandboxAccessKey);
		strcpy_s(self->m_cSandboxName, 32, "DriverMadness Sandbox A");

		self->m_iSandboxTrackingID = 4;

		InitDiscordRichPresence();
	}

	inline void __stdcall OnSandboxSelectorConstructorDiscordOnly(SandboxSelector* self)
	{
		InitDiscordRichPresence();
	}

	inline bool IsSupportedGameVersion()
	{
		enum class GameVersion {
			DriverSanFrancisco_PC_1_0_4,
			// Not a version
			UnknownBuild = 0xDEAD
		};

		struct VersionDef
		{
			std::string_view Id;
			std::intptr_t StrAddr;
			bool Supported;
		};

		static constexpr size_t kMaxVersionLen = 32;
		static constexpr std::intptr_t kUnknownAddr = 0xDEADB33F;

		static std::pair<GameVersion, VersionDef> PossibleGameVersions[] = {
			{ GameVersion::DriverSanFrancisco_PC_1_0_4, { "1.04.1114", 0x00DD6480, true } },
		};

		for (const auto& [gameVersion, versionInfo] : PossibleGameVersions)
		{
			if (versionInfo.StrAddr == kUnknownAddr) continue; // SKip, unable to check it

			const auto valueInGame = std::string_view{ reinterpret_cast<const char*>(versionInfo.StrAddr) };
			if (valueInGame == versionInfo.Id)
				return true;
		}

		return false;
	}

	inline void InitHooks()
	{
		auto& profile = Singleton<ProfileData>::Instance().Get();

		HookProcess proc;

		proc.MainProcess = std::make_shared<HF::Win32::Process>(std::string_view(GAME_PROCESS_NAME));

		if (!proc.MainProcess->isValid())
		{
			Fail(String::fromPrintf("Failed to locate process '%s'!", GAME_PROCESS_NAME), false);
			return;
		}

		proc.MainModule = proc.MainProcess->getSelfModule();
		if (!proc.MainModule)
		{
			Fail(String::fromPrintf("Failed to locate module for process '%s'!", GAME_PROCESS_NAME), false);
			return;
		}

		if (!IsSupportedGameVersion())
		{
			Fail("Unsupported game version for Alcatraz. Your game must be 1.04.1114.", false);
			return;
		}

		AddVectoredExceptionHandler(0UL, ExceptionFilterWin32);

		//
		// hook to sandbox selector
		// 
		//-------------------------------------------
		if (profile.ServiceUrl.length() > 0)
		{
			// disable strcpy calls
			HF::Hook::FillMemoryByNOPs(proc.MainProcess, kSandboxSelectorConstructorAddr + 0xd, 5);
			HF::Hook::FillMemoryByNOPs(proc.MainProcess, kSandboxSelectorConstructorAddr + 0x1f, 5);
			HF::Hook::FillMemoryByNOPs(proc.MainProcess, kSandboxSelectorConstructorAddr + 0x31, 5);
			proc.SandboxSelectorConstructorHook = HF::Hook::HookFunction<void(__stdcall)(SandboxSelector*), kSandboxSelectorConstructorPatchSize>(
				proc.MainProcess,
				kSandboxSelectorConstructorAddr,
				&OnSandboxSelectorConstructor,
				{
					HF::X86::PUSH_AD,
					HF::X86::PUSH_FD,
					HF::X86::PUSH_EAX
				},
			{
				HF::X86::POP_FD,
				HF::X86::POP_AD
			});
		}
		else
		{
			proc.SandboxSelectorConstructorHook = HF::Hook::HookFunction<void(__stdcall)(SandboxSelector*), kSandboxSelectorConstructorPatchSize>(
				proc.MainProcess,
				kSandboxSelectorConstructorAddr,
				&OnSandboxSelectorConstructorDiscordOnly,
				{
					HF::X86::PUSH_AD,
					HF::X86::PUSH_FD,
					HF::X86::PUSH_EAX
				},
			{
				HF::X86::POP_FD,
				HF::X86::POP_AD
			});
		}

		if (!proc.SandboxSelectorConstructorHook->setup())
		{
			Fail("Failed to initialise hook for SandboxSelector constructor", true);
			return;
		}

		// Patch friends list, limit to 16 for og server
		// Alcatraz server itself will always send all friends
		uchar maxFriendsList = 16;
		proc.MainProcess->writeMemory(kFriendsListPageLimitAddress, sizeof(uchar), &maxFriendsList);

		Singleton<HookProcess>::Instance().Set(proc);
	}

	inline void DestroyHooks()
	{
		GetDiscordManager()->DestroyInstance();

		auto& proc = Singleton<HookProcess>::Instance().Get();

		// don't restore since MainProcess is already gone
		//proc.SandboxSelectorConstructorHook = nullptr;
		proc.MainModule = nullptr;
		proc.MainProcess = nullptr;
	}
}