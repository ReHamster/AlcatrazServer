#pragma once

namespace mg {
namespace orbitclient {
class ISavegameReadListener
{
public:
	typedef void(__thiscall* CallBackPtrType)(void*, unsigned int requestId, unsigned int bytesRead);
	void (**CallBackPtr)(unsigned int requestId, unsigned int bytesRead);
};
}
}
