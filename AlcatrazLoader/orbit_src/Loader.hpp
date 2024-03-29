#pragma once

#include "pch.h"

#include "Consts.hpp"
#include "Hooks.hpp"

// ReSharper disable CppUnusedIncludeDirective
#include "Exports/OrbitClient.hpp"
#include "Exports/SavegameInfo.hpp"
#include "Exports/SavegameReader.hpp"
#include "Exports/SavegameWriter.hpp"
// ReSharper restore CppUnusedIncludeDirective

#include "Helpers/Failure.hpp"
#include "Utils/Singleton.hpp"
#include "Objects/AlcatrazConfig.hpp"

namespace AlcatrazUplayR2
{
	inline void UtilOpenURL(const char* url)
	{
		/*
		CoInitialize(nullptr);

		SHELLEXECUTEINFO info{ sizeof(info) };
		info.fMask = SEE_MASK_NOASYNC; // because we exit process after ShellExecuteEx()
		info.lpVerb = "open";
		info.lpFile = url;
		info.nShow = SW_SHOWNORMAL;

		if (!ShellExecuteExA(&info))
		{
			DWORD err = ::GetLastError();
			printf("ShellExecuteEx failed with error %d\n", err);
		}

		CoUninitialize();
		*/
	}

	inline void InitConfig()
	{
		const auto currentPath = Directory::getCurrentDirectory();
		const auto configPath = File::simplifyPath(currentPath + "/" + String::fromCString(CONFIG_NAME));

		// ReSharper disable CppRedundantQualifier

		if (!File::exists(configPath))
		{
			UtilOpenURL(String::fromPrintf("%s/Account/Manage", ALCATRAZ_PORTAL_URL));
			Fail(String::fromPrintf("%s not found.\n\nPlease get one at Alcatraz portal and put to the game folder.", CONFIG_NAME), false);
		}

		File fs;
		if (!fs.open(configPath, File::readFlag))
		{
			Fail(String::fromPrintf("Unable to open %s for read.", CONFIG_NAME), false);
		}

		String data;
		if (!fs.readAll(data))
		{
			Fail(String::fromPrintf("Unable to read %s.", CONFIG_NAME), false);
		}

		AlcatrazConfig config;
		if (!Json::parse(data, config.dataVariant))
		{
			Fail(String::fromPrintf("%s parse error!\n\nPlease get one at Alcatraz portal and put to the game folder.", CONFIG_NAME), false);
		}

		HashMap<String, Variant> configVariant = config.dataVariant.toMap();

		const auto& borderlessWindowVariant = configVariant.find("BorderlessWindow");
		const auto& discordRichPresenceVariant = configVariant.find("DiscordRichPresence");
		const auto& exceptionHandlerVariant = configVariant.find("ExceptionHandler");

		if (!borderlessWindowVariant->isNull()) {
			config.borderlessWindow = borderlessWindowVariant->toBool();
		}
		if (!discordRichPresenceVariant->isNull()) {
			config.discordRichPresence = discordRichPresenceVariant->toBool();
		}
		if (!exceptionHandlerVariant->isNull()) {
			config.exceptionHandler = exceptionHandlerVariant->toBool();
		}

		Singleton<AlcatrazConfig>::Instance().Set(config);
	}
} // namespace AlcatrazUplayR2
