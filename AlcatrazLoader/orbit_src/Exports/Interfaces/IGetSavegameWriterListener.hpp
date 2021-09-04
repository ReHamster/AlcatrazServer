#pragma once

#include "Exports/SavegameWriter.hpp"

// ReSharper disable CppInconsistentNaming
namespace mg::orbitclient
// ReSharper restore CppInconsistentNaming
{
	class IGetSavegameWriterListener
	{
	public:
		typedef void (__thiscall *CallBackPtrType)(void*, unsigned int requestId, int unk,
		                                       SavegameWriter* saveGameWriter);
		void (**CallBackPtr)(unsigned int requestId, int unk, SavegameWriter* saveGameWriter);
	};
}