#pragma once

namespace mg {
namespace orbitclient {
class IGetOrbitServerListener
{
public:
	// ONLY GUESSED. Not used by game
	typedef void(__thiscall* CallBackPtrType)(void*, unsigned int requestId, unsigned int bytesWritten);
	void (**CallBackPtr)(unsigned int requestId, unsigned int bytesWritten);
};
}
}
