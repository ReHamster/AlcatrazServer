#pragma once

#include "Objects/AlcatrazConfig.hpp"
#include "Helpers/Failure.hpp"
#include "Utils/Singleton.hpp"

#include "ProfileManager.hpp"
#include "ExceptionHandler.hpp"

#include "HF/HackingFrameworkFWD.h"
#include "HF/HackingFramework.hpp"


namespace Hermes {
	enum LogChannel {
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

class SandboxSelector {
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

struct SDisplayMode
{
	uint width;
	uint height;
	uint frequency;
	uint bitsPerPixel;
};

namespace driveplatform {
	enum EFilteringMode : int
	{
		EFilteringMode_None = 0x0,
		EFilteringMode_BiLinear = 0x1,
		EFilteringMode_TriLinear = 0x2,
		EFilteringMode_Invalid = 0x3,
	};
}

struct SRendererSettings
{
	SDisplayMode displayMode;
	float gammaR;
	float gammaG;
	float gammaB;
	bool enableVSync;
	bool enablePreciseVSync;
	bool enableAntialiasing;
	bool enableRadialBlur;
	driveplatform::EFilteringMode filteringMode;
	uint vsyncCount;
	int _unknown;
	int pcRenderQuality;
};

struct SInitialisationParams
{
	SRendererSettings* settings;
	void* hWindow;
	bool bWindowed;
};

class IRenderer {
	void* vtbl;
public:
	SInitialisationParams m_InitialisationParams;
	// others aren't yet needed...
};

namespace AlcatrazUplayR2
{
	// QoL patches
	static constexpr uintptr_t kOriginalWndProcAddr = 0x008641C0;
	static constexpr uintptr_t kRegisterClassAAddr = 0x00864FBE;
	static constexpr uintptr_t kIRendererLoadGraphicsIniCallAddr = 0x00BAB4BE;
	static constexpr uintptr_t kIRendererLoadGraphicsIniAddr = 0x00BAA8F0;
	static constexpr size_t kRegisterClassAPatchSize = 6;
	static constexpr size_t kIRendererLoadGraphicsIniPatchSize = 5;

	// Online patches
	static constexpr uintptr_t kFriendsListPageLimitAddress = 0x009E24DE;
	static constexpr uintptr_t kOnlineConfigServiceHostPRODAddress = 0x016DD358;
	static constexpr uintptr_t kSandboxSelectorConstructorAddr = 0x004CF530;
	static constexpr uintptr_t kHermesLogCallbackAddr = 0x016DB558;
	static constexpr size_t kSandboxSelectorConstructorPatchSize = 10;

	static Hermes::LogCallback& hermesLogCallbackVar = *reinterpret_cast<Hermes::LogCallback*>(kHermesLogCallbackAddr);
	static Hermes::LogCallback origOrDNGHookLogCallback = nullptr;
	static char** s_OnlineConfigServiceHostPROD = reinterpret_cast<char**>(kOnlineConfigServiceHostPRODAddress);

	using WndProc_t = LRESULT(__stdcall* )(HWND, UINT, WPARAM, LPARAM);
	using RegisterClassA_t = ATOM(__stdcall*)(WNDCLASSA*);
	using SandboxSelectorConstructor_t = void(__stdcall*)(SandboxSelector*);
	using IRendererLoadGraphicsIni_t = bool(__fastcall*)(IRenderer*);

	struct HookProcess
	{
		HF::Win32::ProcessPtr MainProcess;
		HF::Win32::ModulePtr MainModule;

		HF::Hook::TrampolinePtr<kRegisterClassAPatchSize> RegisterClassAHook;
		HF::Hook::TrampolinePtr<kSandboxSelectorConstructorPatchSize> SandboxSelectorConstructorHook;
		HF::Hook::TrampolinePtr<kIRendererLoadGraphicsIniPatchSize> IRendererLoadGraphicsIniHook;
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

		if (message.length())
		{
			auto& profile = Singleton<ProfileData>::Instance().Get();
			GetDiscordManager()->SetStatus({ 0 }, (const char*)profile.AccountId, message);
		}
	}

	inline void InitDiscordRichPresence()
	{
		auto& config = Singleton<AlcatrazConfig>::Instance().Get();
		auto& profile = Singleton<ProfileData>::Instance().Get();

		CDiscord::CreateInstance();
		CDiscord* mgr = GetDiscordManager();

		// Enable it, this needs to be set via a config file of some sort. 
		mgr->SetUseDiscord(config.discordRichPresence);
		mgr->SetStatus({0}, (const char*)profile.AccountId, "Started the game");

		// DNGHook-friendly callback
		Hermes::LogCallback oldCallback = hermesLogCallbackVar;
		hermesLogCallbackVar = OnlineLogCallback;
		origOrDNGHookLogCallback = oldCallback;
	}

	inline void __stdcall OnSandboxSelectorConstructor(SandboxSelector* self)
	{
		auto& profile = Singleton<ProfileData>::Instance().Get();

		if (profile.ServiceUrl.length() > 0)
		{
			// since it's pointer to string we can safely
			// replace it without copying into 28 char buffer
			*s_OnlineConfigServiceHostPROD = (char*)profile.ServiceUrl;

			// not that necessary but we can play with that one too
			// (maybe we have another Alcatraz server but on different port)
			strcpy_s(self->m_cOnlineConfigKey, 64, profile.ConfigKey);
			strcpy_s(self->m_cSandboxKey, 32, profile.SandboxAccessKey);
			strcpy_s(self->m_cSandboxName, 32, "DriverMadness Sandbox A");
		}

		self->m_iSandboxTrackingID = 4;

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
			intptr_t StrAddr;
			bool Supported;
		};

		static constexpr size_t kMaxVersionLen = 32;
		static constexpr intptr_t kUnknownAddr = 0xDEADB33F;

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

	inline void GetDesktopRes(int& width, int& height)
	{
		HMONITOR monitor = MonitorFromWindow(GetDesktopWindow(), MONITOR_DEFAULTTONEAREST);
		MONITORINFO info = {};
		info.cbSize = sizeof(MONITORINFO);
		GetMonitorInfo(monitor, &info);

		width = info.rcMonitor.right - info.rcMonitor.left;
		height = info.rcMonitor.bottom - info.rcMonitor.top;
	}

	inline LRESULT WINAPI Hamster_WndProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
	{
		auto& config = Singleton<AlcatrazConfig>::Instance().Get();

		if (config.borderlessWindow && (msg == WM_SIZE || msg == WM_MOVE))
		{
			int desktopResW, desktopResH;
			GetDesktopRes(desktopResW, desktopResH);

			LONG lStyle = GetWindowLong(hWnd, GWL_STYLE);
			lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU);
			SetWindowLong(hWnd, GWL_STYLE, lStyle);

			static tagRECT REKT;
			REKT.left = 0;
			REKT.top = 0;
			REKT.right = desktopResW;
			REKT.bottom = desktopResH;
			SetWindowPos(hWnd, NULL, REKT.left, REKT.top, REKT.right, REKT.bottom, SWP_NOACTIVATE | SWP_NOZORDER);

			return 1;
		}

		auto hamsterWndProc = (WndProc_t)kOriginalWndProcAddr;
		return hamsterWndProc(hWnd, msg, wParam, lParam);
	}

	inline ATOM __stdcall RegisterClassA_Hooked(WNDCLASSA* wndClass)
	{
		// only replace if DriverNGHook was not installed
		if(wndClass->lpfnWndProc == (WndProc_t)kOriginalWndProcAddr)
			wndClass->lpfnWndProc = Hamster_WndProc;

		return RegisterClassA(wndClass);
	}

	inline bool __fastcall IRendererLoadGraphicsIni_Hooked(IRenderer* _this)
	{
		auto origLoadGraphicsIniFunc = (IRendererLoadGraphicsIni_t)kIRendererLoadGraphicsIniAddr;

		const bool result = origLoadGraphicsIniFunc(_this);

		auto& config = Singleton<AlcatrazConfig>::Instance().Get();

		if (config.borderlessWindow)
		{
			int desktopResW, desktopResH;
			GetDesktopRes(desktopResW, desktopResH);
			_this->m_InitialisationParams.bWindowed = true;
			_this->m_InitialisationParams.settings->displayMode.width = desktopResW;
			_this->m_InitialisationParams.settings->displayMode.height = desktopResH;
		}

		return result;
	}

	inline void InitHooks()
	{
		auto& profile = Singleton<ProfileData>::Instance().Get();

		const bool isDriverNGHookConnected = GetModuleHandleA("DriverNGHook.asi") != NULL;

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

		InitExceptionHandler();

		if (!isDriverNGHookConnected)
		{
			HF::Hook::FillMemoryByNOPs(proc.MainProcess, kRegisterClassAAddr, kRegisterClassAPatchSize);
			proc.RegisterClassAHook = HF::Hook::HookFunction<RegisterClassA_t, kRegisterClassAPatchSize>(
				proc.MainProcess,
				kRegisterClassAAddr,
				&RegisterClassA_Hooked,
				{},
				{});

			if (!proc.RegisterClassAHook->setup())
			{
				Fail("Failed to setup patch to RegisterClassA!\n", false);
				return;
			}
		}

		HF::Hook::FillMemoryByNOPs(proc.MainProcess, kIRendererLoadGraphicsIniCallAddr, kIRendererLoadGraphicsIniPatchSize);
		proc.IRendererLoadGraphicsIniHook = HF::Hook::HookFunction<IRendererLoadGraphicsIni_t, kIRendererLoadGraphicsIniPatchSize>(
			proc.MainProcess,
			kIRendererLoadGraphicsIniCallAddr,
			&IRendererLoadGraphicsIni_Hooked,
			{},
			{});
		
		if (!proc.IRendererLoadGraphicsIniHook->setup())
		{
			Fail("Failed to setup patch to IRenderer::LoadGraphicsIni!\n", false);
			return;
		}

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
		}

		proc.SandboxSelectorConstructorHook = HF::Hook::HookFunction<SandboxSelectorConstructor_t, kSandboxSelectorConstructorPatchSize>(
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