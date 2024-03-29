#include "pch.h"
#include "Loader.hpp"
#include "ProfileManager.hpp"

// ReSharper disable CppParameterNeverUsed
// ReSharper disable CppParameterMayBeConst
BOOL APIENTRY DllMain(HMODULE hModule,
					  DWORD ulReasonForCall,
					  LPVOID lpReserved)
{
	switch (ulReasonForCall)
	{
	case DLL_PROCESS_ATTACH:
	{
		AlcatrazUplayR2::InitConfig();
		AlcatrazUplayR2::InitProfile();
		AlcatrazUplayR2::InitHooks();

		break;
	}
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	case DLL_PROCESS_DETACH:
		AlcatrazUplayR2::DestroyHooks();
		break;
	default:;
	}
	return TRUE;
}

// ReSharper restore CppParameterNeverUsed
// ReSharper restore CppParameterMayBeConst
