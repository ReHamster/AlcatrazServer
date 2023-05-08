#pragma once

#include "pch.h"

#include "Macro.hpp"
#include "Utils/Singleton.hpp"
#include "Objects/AlcatrazConfig.hpp"

#include "Interfaces/ISavegameWriteListener.hpp"

// ReSharper disable CppInconsistentNaming
// ReSharper disable CppMemberFunctionMayBeConst
// ReSharper disable CppMemberFunctionMayBeStatic
// ReSharper disable CppParameterMayBeConst
namespace mg {
namespace orbitclient {

using namespace AlcatrazUplayR2;

class UPLAY_CPP_API SavegameWriter
{
	String FilePath;
	String NamePath;
	unsigned int SaveId;

public:
	SavegameWriter();
	void Close(bool arg);
	void Write(unsigned int requestId, ISavegameWriteListener* savegameWriteListenerCallBack, void* buff,
		unsigned int numberOfBytes);
	bool SetName(unsigned short* name);
};

}
} // namespace mg::orbitclient

//------------------------------------------------------------------------------
inline mg::orbitclient::SavegameWriter::SavegameWriter()
{

}

//------------------------------------------------------------------------------
inline void mg::orbitclient::SavegameWriter::Close(bool arg)
{
	Debug::printf("Close");
}

//------------------------------------------------------------------------------
inline void mg::orbitclient::SavegameWriter::Write(unsigned int requestId,
												   ISavegameWriteListener *savegameWriteListenerCallBack, void *buff,
												   unsigned int numberOfBytes)
{
	Debug::printf("RequestId: %d SavegameWriteListenerCallBack: %x Buff: %x NumberOfBytes: %d", requestId,
						reinterpret_cast<void *>(&savegameWriteListenerCallBack), buff, numberOfBytes);
}

//------------------------------------------------------------------------------
inline bool mg::orbitclient::SavegameWriter::SetName(unsigned short *name)
{
	//LOGD << fmt::format("Name: {}", reinterpret_cast<wchar_t *>(name));
	return true;
}

// ReSharper restore CppInconsistentNaming
// ReSharper restore CppMemberFunctionMayBeConst
// ReSharper restore CppMemberFunctionMayBeStatic
// ReSharper restore CppParameterMayBeConst
