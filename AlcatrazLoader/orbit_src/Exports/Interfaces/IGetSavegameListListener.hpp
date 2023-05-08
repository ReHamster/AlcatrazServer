#pragma once

#include "Exports/SavegameInfo.hpp"

namespace mg {
namespace orbitclient {
class IGetSavegameListListener
{
public:
	typedef void(__thiscall* CallBackPtrType)(void*, unsigned int requestId, SavegameInfo* saveGameInfoList,
		unsigned int listSize);
	void (**CallBackPtr)(unsigned int requestId, SavegameInfo* saveGameInfoList, unsigned int listSize);
};
}
}
