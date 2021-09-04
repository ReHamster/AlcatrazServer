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
#include "Objects/OrbitConfig.hpp"

namespace AlcatrazUplayR2
{
	inline void InitConfig()
	{
		const auto currentPath = Directory::getCurrentDirectory();
		const auto configPath = File::simplifyPath(currentPath + "/" + String::fromCString(CONFIG_NAME));

		// ReSharper disable CppRedundantQualifier

		if (!File::exists(configPath))
		{
			Fail(String::fromPrintf("%s file not found!", CONFIG_NAME), false);
		}

		File fs;
		if (fs.open(configPath, File::readFlag))
		{
			String data;
			if (fs.readAll(data))
			{
				OrbitConfig config;
				if (!Json::parse(data, config.dataVariant))
				{
					Fail(String::fromPrintf("JSON %s parse error!", CONFIG_NAME), false);
				}

				Singleton<OrbitConfig>::Instance().Set(config);
			}
		}
	}
} // namespace AlcatrazUplayR2
