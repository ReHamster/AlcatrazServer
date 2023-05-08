#pragma once

#include "pch.h"
#include "Macro.hpp"

// ReSharper disable CppInconsistentNaming
// ReSharper disable CppMemberFunctionMayBeConst
namespace mg {
namespace orbitclient {

class UPLAY_CPP_API SavegameInfo
{
	std::wstring Name;
	unsigned int Id;
	unsigned long Size;

public:
	SavegameInfo(unsigned int, unsigned long, const std::wstring&);
	unsigned int GetSavegameId();
	unsigned int GetProductId(void);
	unsigned int GetSize();
	unsigned short const* GetName();
};

}
} // namespace mg::orbitclient

//------------------------------------------------------------------------------
inline mg::orbitclient::SavegameInfo::SavegameInfo(const unsigned int id,
												   const unsigned long size, const std::wstring &name)
{
	Id = id;
	Size = size;
	Name = name;
}

//------------------------------------------------------------------------------
inline unsigned int mg::orbitclient::SavegameInfo::GetProductId()
{
	Debug::printf("GetProductId\n");
	return 666;
}

//------------------------------------------------------------------------------
inline unsigned int mg::orbitclient::SavegameInfo::GetSavegameId()
{
	Debug::printf("GetSavegameId\n");
	return Id;
}

//------------------------------------------------------------------------------
inline unsigned int mg::orbitclient::SavegameInfo::GetSize()
{
	Debug::printf("GetSize\n");
	return Size;
}

//------------------------------------------------------------------------------
inline unsigned short const *mg::orbitclient::SavegameInfo::GetName()
{
	Debug::printf("GetName\n");
	return reinterpret_cast<const unsigned short *>(&Name.c_str()[0]);
}

// ReSharper restore CppMemberFunctionMayBeConst
// ReSharper restore CppInconsistentNaming
