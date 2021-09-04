#pragma once

#include "pch.h"

namespace AlcatrazUplayR2
{
	/*
	struct Logging
	{
		bool Active;
		string Path;

		// ReSharper disable CppInconsistentNaming
		template <class Archive>
		void serialize(Archive &ar)
		{
			ar(CEREAL_NVP(Active), CEREAL_NVP(Path));
		}

		// ReSharper restore CppInconsistentNaming
	};

	struct Backup
	{
		bool Active;
		string Path;

		// ReSharper disable CppInconsistentNaming
		template <class Archive>
		void serialize(Archive &ar)
		{
			ar(CEREAL_NVP(Active), CEREAL_NVP(Path));
		}

		// ReSharper restore CppInconsistentNaming
	};

	struct Saves
	{
		string Path;
		Backup Backup;

		// ReSharper disable CppInconsistentNaming
		template <class Archive>
		void serialize(Archive &ar)
		{
			ar(CEREAL_NVP(Path), CEREAL_NVP(Backup));
		}

		// ReSharper restore CppInconsistentNaming
	};

	struct Profile
	{
		string AccountId;
		string Password;
		string GameKey;

		// ReSharper disable CppInconsistentNaming
		template <class Archive>
		void serialize(Archive &ar)
		{
			ar(CEREAL_NVP(AccountId), CEREAL_NVP(Password), CEREAL_NVP(GameKey));
		}

		// ReSharper restore CppInconsistentNaming
	};

	struct Orbit
	{
		Profile Profile;
		Logging Logging;
		Saves Saves;

		// ReSharper disable CppInconsistentNaming
		template <class Archive>
		void serialize(Archive &ar)
		{
			ar(CEREAL_NVP(Profile), CEREAL_NVP(Logging), CEREAL_NVP(Saves));
		}

		// ReSharper restore CppInconsistentNaming
	};
	*/
	struct OrbitConfig
	{
		Variant dataVariant;
	};

} // namespace AlcatrazUplayR2
