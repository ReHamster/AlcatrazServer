#pragma once

#include "Exports/SavegameReader.hpp"

namespace mg {
namespace orbitclient {
class IGetSavegameReaderListener
{
public:
	typedef void(__thiscall* CallBackPtrType)(void*, unsigned int requestId, int unk,
		SavegameReader* saveGameReader);
	void (**CallBackPtr)(unsigned int requestId, int unk, SavegameReader* saveGameReader);
};
}
}
