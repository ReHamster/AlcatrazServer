#pragma once

#include "Objects/OrbitConfig.hpp"
#include "Helpers/Failure.hpp"
#include "Utils/Singleton.hpp"

#include "HF/HackingFrameworkFWD.h"

#include "HF/HackingFramework.hpp"

namespace AlcatrazUplayR2
{
	struct ProfileData
	{
		String Name;

		String AccountId;
		String Password;
		String GameKey;

		String ServiceUrl;
		String ConfigKey;
		String SandboxAccessKey;
	};

	inline void InitProfile()
	{
		List<String> commandLine;
		String::fromCString(GetCommandLineA()).split(commandLine, " ", true);

		ProfileData profile;

		auto& config = Singleton<OrbitConfig>::Instance().Get();
		auto& AllProfiles = config.dataVariant.toMap().find("Profiles")->toMap();

		profile.Name = config.dataVariant.toMap().find("UseProfile")->toString();

		auto profileSwitch = commandLine.find("-profile");

		if (profileSwitch != commandLine.end())
		{
			profile.Name = *(++profileSwitch);
		}

		auto& currentProfile = AllProfiles.find(profile.Name)->toMap();

		profile.AccountId = currentProfile.find("AccountId")->toString();
		profile.Password = currentProfile.find("Password")->toString();
		profile.GameKey = currentProfile.find("GameKey")->toString();
		profile.ServiceUrl = currentProfile.find("ServiceUrl")->toString();
		profile.ConfigKey = currentProfile.find("ConfigKey")->toString();
		profile.SandboxAccessKey = currentProfile.find("AccessKey")->toString();

		Singleton<ProfileData>::Instance().Set(profile);
	}
}