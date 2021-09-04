using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcatrazLauncher
{
	public class Constants
	{

#if DEBUG
		public static string SERVICE_URL_KEY => "ServiceUrlDEV";
		public static string SANDBOX_CONFIGKEY_KEY => "ConfigKeyDEV";
		public static string SANDBOX_ACCESSKEY_KEY => "SandboxAccessKeyDEV";
		public static string AlcatrazProfileKey => "AlcatrazDEV";
#else
		public static string SERVICE_URL_KEY => "ServiceUrl";
		public static string SANDBOX_CONFIGKEY_KEY => "ConfigKey";
		public static string SANDBOX_ACCESSKEY_KEY => "SandboxAccessKey";
		public static string AlcatrazProfileKey => "AlcatrazPROD";
#endif

		public static string ConfigFilename =>"Alcatraz.json";
		public static string NoProfile => "None";
		public static string OfficialProfileKey => "UbiOfficial";
	}
}
