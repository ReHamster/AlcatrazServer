#pragma once

// ReSharper disable CppInconsistentNaming
namespace mg::orbitclient
// ReSharper restore CppInconsistentNaming
{
	class IRemoveSavegameListener
	{
	public:
		typedef void (__thiscall *CallBackPtrType)(void*, unsigned int requestId, bool deleted);
		void (**CallBackPtr)(unsigned int requestId, bool deleted);
	};
}
