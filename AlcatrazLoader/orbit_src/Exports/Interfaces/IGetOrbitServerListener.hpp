#pragma once

// ReSharper disable CppInconsistentNaming
namespace mg::orbitclient
// ReSharper restore CppInconsistentNaming
{
	class IGetOrbitServerListener
	{
	public:
		// ONLY GUESSED. Not used by game
		typedef void (__thiscall * CallBackPtrType)(void*, unsigned int requestId, unsigned int bytesWritten);
		void (**CallBackPtr)(unsigned int requestId, unsigned int bytesWritten);
	};
}
