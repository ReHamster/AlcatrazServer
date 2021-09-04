#pragma once

#include "Exports/SavegameReader.hpp"

// ReSharper disable CppInconsistentNaming
namespace mg::orbitclient
// ReSharper restore CppInconsistentNaming
{
	class IGetSavegameReaderListener
	{
	public:
		typedef void (__thiscall * CallBackPtrType)(void*, unsigned int requestId, int unk,
		                                       SavegameReader* saveGameReader);
		void (**CallBackPtr)(unsigned int requestId, int unk, SavegameReader* saveGameReader);
	};
}
