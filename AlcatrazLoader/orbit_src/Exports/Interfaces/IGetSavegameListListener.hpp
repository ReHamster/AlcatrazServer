#pragma once

#include "Exports/SavegameInfo.hpp"

// ReSharper disable CppInconsistentNaming
namespace mg::orbitclient
// ReSharper restore CppInconsistentNaming
{
	class IGetSavegameListListener
	{
	public:
		typedef void (__thiscall * CallBackPtrType)(void*, unsigned int requestId, SavegameInfo* saveGameInfoList,
		                                       unsigned int listSize);
		void (**CallBackPtr)(unsigned int requestId, SavegameInfo* saveGameInfoList, unsigned int listSize);
	};
}
