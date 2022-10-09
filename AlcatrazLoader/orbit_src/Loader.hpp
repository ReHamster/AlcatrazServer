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
	inline void InitConfig()
	{
		const auto currentPath = Directory::getCurrentDirectory();
		const auto configPath = File::simplifyPath(currentPath + "/" + String::fromCString(CONFIG_NAME));

		// ReSharper disable CppRedundantQualifier

		if (!File::exists(configPath))
		{
			Fail(String::fromPrintf("%s file not found.", CONFIG_NAME), false);
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
			Fail(String::fromPrintf("JSON %s parse error!", CONFIG_NAME), false);
		}

		HashMap<String, Variant> configVariant = config.dataVariant.toMap();

		const auto& borderlessWindowVariant = configVariant.find("BorderlessWindow");
		const auto& discordRichPresenceVariant = configVariant.find("DiscordRichPresence");

		if (!borderlessWindowVariant->isNull()) {
			config.borderlessWindow = borderlessWindowVariant->toBool();
		}
		if (!discordRichPresenceVariant->isNull()) {
			config.discordRichPresence = discordRichPresenceVariant->toBool();
		}

		Singleton<AlcatrazConfig>::Instance().Set(config);
	}
} // namespace AlcatrazUplayR2
