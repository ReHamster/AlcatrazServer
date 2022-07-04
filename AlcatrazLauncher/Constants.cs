using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcatrazLauncher
{
	public class GameInstallRegProperty
	{
		public RegistryHive RegistryHive { get; set; }
		public string RegistryPath { get; set; }
		public string InstallPathKey { get; set; }
	}

	public class Constants
	{
		public static GameInstallRegProperty AlcatrazRegProperty = new GameInstallRegProperty
		{
			RegistryHive = RegistryHive.CurrentUser,
			RegistryPath = @"Software\Alcatraz\Launcher",
			InstallPathKey = "GameLocation"
		};
		// known game locations
		public static GameInstallRegProperty[] GameInstallRegProperties = new GameInstallRegProperty[] { 
			new GameInstallRegProperty{
				RegistryHive = RegistryHive.LocalMachine,
				RegistryPath = @"Software\Ubisoft\Driver San Francisco",
				InstallPathKey = "GameLocation"
			},
			new GameInstallRegProperty{
				RegistryHive = RegistryHive.LocalMachine,
				RegistryPath = @"Software\Ubisoft\Launcher\Installs\13",
				InstallPathKey = "InstallDir"
			},
			AlcatrazRegProperty,
		};

#if DEBUG
		public static string SERVICE_URL_KEY => "ServiceUrlDEV";
		public static string SANDBOX_CONFIGKEY_KEY => "ConfigKeyDEV";
		public static string SANDBOX_ACCESSKEY_KEY => "SandboxAccessKeyDEV";
		public static string AlcatrazProfileKey => "AlcatrazDEV";
#else
		public static string SERVICE_URL_KEY => "ServiceUrl";
		public static string SANDBOX_CONFIGKEY_KEY => "ConfigKey";
		public static string SANDBOX_ACCESSKEY_KEY => "SandboxAccessKey";
		public static string AlcatrazProfileKey => "Alcatraz";
#endif

		public static string ConfigFilename =>"Alcatraz.json";
		public static string NoProfile => "None";
		public static string OfficialProfileKey => "UbiOfficial";
		public static string OrbitLoaderFilename => "ubiorbitapi_r2_loader.dll";
		public static string OrbitLoaderSHA1 => "8F7CC25567F8996C0A5D082024E751EAD9430FF2"; // original checksum
	}
}
