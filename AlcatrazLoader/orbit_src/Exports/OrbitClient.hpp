#pragma once

#include "pch.h"
#include <codecvt>

#include "Macro.hpp"
#include "Consts.hpp"
#include "ProfileManager.hpp"

#include "Utils/Singleton.hpp"
#include "Objects/AlcatrazConfig.hpp"

#include "SavegameInfo.hpp"
#include "SavegameReader.hpp"
#include "SavegameWriter.hpp"

#include "Interfaces/IRemoveSavegameListener.hpp"
#include "Interfaces/IGetLoginDetailsListener.hpp"
#include "Interfaces/IGetSavegameListListener.hpp"
#include "Interfaces/IGetSavegameWriterListener.hpp"
#include "Interfaces/IGetSavegameReaderListener.hpp"
#include "Interfaces/IGetOrbitServerListener.hpp"
#include "Interfaces/IGetOrbitServerListener.hpp"

// ReSharper disable CppInconsistentNaming
// ReSharper disable CppParameterMayBeConst
// ReSharper disable CppMemberFunctionMayBeConst
// ReSharper disable CppMemberFunctionMayBeStatic
namespace mg {
namespace orbitclient {

class UPLAY_CPP_API OrbitClient // NOLINT
{
	volatile int RequestId;

public:
	OrbitClient();

	void StartProcess(unsigned short*, unsigned short*, unsigned short*);
	bool StartLauncher(unsigned int, unsigned int, char const*, char const*);

	void GetSavegameList(unsigned int requestId, IGetSavegameListListener* savegameListListenerCallBack,
		unsigned int productId);
	void GetSavegameWriter(unsigned int requestId, IGetSavegameWriterListener* savegameWriterListenerCallBack,
		unsigned int productId, unsigned int saveGameId, bool open);
	void GetSavegameReader(unsigned int requestId, IGetSavegameReaderListener* savegameReaderListenerCallBack,
		unsigned int productId, unsigned int saveGameId);
	void RemoveSavegame(unsigned int requestId, IRemoveSavegameListener* removeSavegameListenerCallBack,
		unsigned int productId, unsigned int saveGameId);

	void GetLoginDetails(unsigned int requestId, IGetLoginDetailsListener* loginDetailsListenerCallBack);
	void GetOrbitServer(unsigned int requestId, IGetOrbitServerListener*, unsigned int, unsigned int);

	unsigned int GetRequestUniqueId();
	unsigned short* GetInstallationErrorString(char const*);
	unsigned int GetInstallationErrorNum();

	void Update();

	~OrbitClient();
};

}
} // namespace mg::orbitclient

//------------------------------------------------------------------------------
inline mg::orbitclient::OrbitClient::OrbitClient()
{
	Debug::printf("OrbitClient constructor\n");
}

//------------------------------------------------------------------------------
inline void mg::orbitclient::OrbitClient::StartProcess(unsigned short *, unsigned short *, unsigned short *)
{
	Debug::printf("StartProcess\n");
}

//------------------------------------------------------------------------------
inline void mg::orbitclient::OrbitClient::GetSavegameList(unsigned int requestId,
														  IGetSavegameListListener *savegameListListenerCallBack,
														  unsigned int productId)
{
	Debug::printf("RequestId: %d GetSavegameListListenerCallBack: %x ProductId: %d", requestId,
						reinterpret_cast<void *>(&savegameListListenerCallBack), productId);
}

//------------------------------------------------------------------------------
inline void mg::orbitclient::OrbitClient::GetSavegameReader(unsigned int requestId,
															IGetSavegameReaderListener *savegameReaderListenerCallBack,
															unsigned int productId, unsigned int saveGameId)
{
	Debug::printf("RequestId: %d GetSavegameReaderListenerCallBack: %x ProductId: %d SaveGameId: %d", requestId,
						reinterpret_cast<void *>(&savegameReaderListenerCallBack),
						productId, saveGameId);

}

std::wstring stringToWstring(const String& t_str)
{
	//setup converter
	typedef std::codecvt_utf8<wchar_t> convert_type;
	std::wstring_convert<convert_type, wchar_t> converter;

	//use converter (.to_bytes: wstr->str, .from_bytes: str->wstr)
	return converter.from_bytes(t_str);
}
//------------------------------------------------------------------------------
inline void mg::orbitclient::OrbitClient::Update()
{
	// LOGD << "Update";
}

//------------------------------------------------------------------------------
inline bool mg::orbitclient::OrbitClient::StartLauncher(unsigned int a, unsigned int b, char const *langCode, char const *arguments)
{
	Debug::printf("StartLauncher");
	return false;
}

//------------------------------------------------------------------------------
inline unsigned short *mg::orbitclient::OrbitClient::GetInstallationErrorString(char const *err)
{
	Debug::printf("GetInstallationErrorString");
	return nullptr;
}

//------------------------------------------------------------------------------
inline unsigned int mg::orbitclient::OrbitClient::GetInstallationErrorNum()
{
	Debug::printf("GetInstallationErrorNum");
	return 0;
}

//------------------------------------------------------------------------------
inline void mg::orbitclient::OrbitClient::GetSavegameWriter(unsigned int requestId,
															IGetSavegameWriterListener *savegameWriterListenerCallBack,
															unsigned int productId, unsigned int saveGameId, bool open)
{
	Debug::printf("RequestId: %s GetSavegameWriterListenerCallBack: %x ProductId: %d SaveGameId: %d", requestId,
						reinterpret_cast<void *>(&savegameWriterListenerCallBack), productId, saveGameId);

}

//------------------------------------------------------------------------------
inline void mg::orbitclient::OrbitClient::RemoveSavegame(unsigned int requestId,
														 IRemoveSavegameListener *removeSavegameListenerCallBack,
														 unsigned int productId, unsigned int saveGameId)
{
	Debug::printf("RequestId: %d RemoveSavegameListenerCallBack: %x", requestId,
						reinterpret_cast<void *>(&removeSavegameListenerCallBack));
}

//------------------------------------------------------------------------------
inline void mg::orbitclient::OrbitClient::GetLoginDetails(unsigned int requestId,
														  IGetLoginDetailsListener *loginDetailsListenerCallBack)
{
	Debug::printf("RequestId: %d LoginDetailsListenerCallBack: %x", requestId,
						reinterpret_cast<void *>(&loginDetailsListenerCallBack));

	// m_loginCallbacks[requestId] = loginDetailsListenerCallBack;

	const auto callBack = reinterpret_cast<IGetLoginDetailsListener::CallBackPtrType>(**loginDetailsListenerCallBack->CallBackPtr);

	if (callBack == nullptr)
	{
		return;
	}

	auto& profile = Singleton<ProfileData>::Instance().Get();

	std::wstring passwordWstr = stringToWstring(profile.Password);

	callBack(loginDetailsListenerCallBack, requestId, profile.AccountId, passwordWstr.c_str(), profile.GameKey);
	
}

//------------------------------------------------------------------------------
void mg::orbitclient::OrbitClient::GetOrbitServer(unsigned int requestId, IGetOrbitServerListener* callback, unsigned int, unsigned int)
{

}

//------------------------------------------------------------------------------
inline unsigned int mg::orbitclient::OrbitClient::GetRequestUniqueId()
{
	Debug::printf("GetRequestUniqueId");
	return Atomic::increment(RequestId);
}

//------------------------------------------------------------------------------
inline mg::orbitclient::OrbitClient::~OrbitClient()
{
	Debug::printf("~OrbitClient");
}

// ReSharper restore CppMemberFunctionMayBeStatic
// ReSharper restore CppMemberFunctionMayBeConst
// ReSharper restore CppInconsistentNaming
