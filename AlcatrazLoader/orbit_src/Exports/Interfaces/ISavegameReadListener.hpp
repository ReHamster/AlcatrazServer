#pragma once

// ReSharper disable CppInconsistentNaming
namespace mg::orbitclient
// ReSharper restore CppInconsistentNaming
{
	class ISavegameReadListener
	{
	public:
		typedef void(__thiscall *CallBackPtrType)(void *, unsigned int requestId, unsigned int bytesRead);
		void (**CallBackPtr)(unsigned int requestId, unsigned int bytesRead);
	};
}
