#pragma once

namespace mg {
namespace orbitclient {
class IRemoveSavegameListener
{
public:
	typedef void(__thiscall* CallBackPtrType)(void*, unsigned int requestId, bool deleted);
	void (**CallBackPtr)(unsigned int requestId, bool deleted);
};
}
}
