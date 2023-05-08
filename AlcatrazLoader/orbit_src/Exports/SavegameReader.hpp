#pragma once

#include "pch.h"

#include "Macro.hpp"

#include "Interfaces/ISavegameReadListener.hpp"

// ReSharper disable CppInconsistentNaming
// ReSharper disable CppParameterMayBeConst
// ReSharper disable CppMemberFunctionMayBeConst
namespace mg {
namespace orbitclient {
using namespace AlcatrazUplayR2;

class UPLAY_CPP_API SavegameReader
{
	String FilePath;

public:
	SavegameReader();
	void Close();
	void Read(unsigned int requestId, ISavegameReadListener* savegameReadListenerCallBack,
		unsigned int offset, void* buff, unsigned int numberOfBytes);
};
}
} // namespace mg::orbitclient

//------------------------------------------------------------------------------
inline mg::orbitclient::SavegameReader::SavegameReader()
{

}

//------------------------------------------------------------------------------
inline void mg::orbitclient::SavegameReader::Close()
{
	Debug::printf("Close");
}

//------------------------------------------------------------------------------
inline void mg::orbitclient::SavegameReader::Read(unsigned int requestId,
                                                  ISavegameReadListener* savegameReadListenerCallBack,
                                                  unsigned int offset, void* buff, unsigned int numberOfBytes)
{
	Debug::printf("RequestId: %d SavegameReadListenerCallBack: %x Offset: %d Buff: %x NumberOfBytes: %d",
	                    requestId,
	                    reinterpret_cast<void *>(&savegameReadListenerCallBack), offset, buff, numberOfBytes);
}

// ReSharper restore CppInconsistentNaming
// ReSharper restore CppParameterMayBeConst
// ReSharper restore CppMemberFunctionMayBeConst
