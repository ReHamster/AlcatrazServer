#pragma once

#include "Objects/OrbitConfig.hpp"
#include "Helpers/Failure.hpp"
#include "Utils/Singleton.hpp"

#include "ProfileManager.hpp"

#include "HF/HackingFrameworkFWD.h"

#include "HF/HackingFramework.hpp"

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
	static constexpr uintptr_t kOnlineConfigServiceHostPRODAddress = 0x016DD358;
	static constexpr uintptr_t kSandboxSelectorConstructorAddr = 0x004CF530;
	static constexpr size_t kSandboxSelectorConstructorPatchSize = 10;


	struct HookProcess
	{
		HF::Win32::ProcessPtr MainProcess;
		HF::Win32::ModulePtr MainModule;
		HF::Hook::TrampolinePtr<kSandboxSelectorConstructorPatchSize> SandboxSelectorConstructorHook;
	};

	inline void __stdcall OnSandboxSelectorConstructor(SandboxSelector* self)
	{
		auto& profile = Singleton<ProfileData>::Instance().Get();

		char* str = *(char**)kOnlineConfigServiceHostPRODAddress;
		strcpy_s(str, 28, profile.ServiceUrl);

		strcpy_s(self->m_cOnlineConfigKey, 64, profile.ConfigKey);
		strcpy_s(self->m_cSandboxKey, 32, profile.SandboxAccessKey);
		strcpy_s(self->m_cSandboxName, 32, "DriverMadness Sandbox A");

		self->m_iSandboxTrackingID = 4;
	}

	inline void InitHooks()
	{
		auto& profile = Singleton<ProfileData>::Instance().Get();

		HookProcess proc;

		// do not initialize hook
		if (profile.ServiceUrl.length() == 0)
		{
			Singleton<HookProcess>::Instance().Set(proc);
			return;
		}

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

		//
		// hook to sandbox selector
		// 
		//-------------------------------------------
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

		if (!proc.SandboxSelectorConstructorHook->setup())
		{
			Fail("Failed to initialise hook for SandboxSelector constructor", true);
			return;
		}

		Singleton<HookProcess>::Instance().Set(proc);
	}

	inline void DestroyHooks()
	{
		auto& proc = Singleton<HookProcess>::Instance().Get();

		// don't restore since MainProcess is already gone
		//proc.SandboxSelectorConstructorHook = nullptr;
		proc.MainModule = nullptr;
		proc.MainProcess = nullptr;
	}
}