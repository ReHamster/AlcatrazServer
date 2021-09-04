#pragma once

// ReSharper disable CppInconsistentNaming
namespace mg::orbitclient
// ReSharper restore CppInconsistentNaming
{
	class IGetLoginDetailsListener
	{
	public:
		typedef int (__thiscall * CallBackPtrType)(void*, unsigned int requestId, const char* accountId,
		                                      const wchar_t* password, const char* gamekey);
		void (**CallBackPtr)(unsigned int requestId, const char* accountId, const wchar_t* userName,
		                     const char* password);
	};
}
