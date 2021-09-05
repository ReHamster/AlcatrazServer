using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Windows.Forms;

namespace AlcatrazLauncher
{
	static class Program
	{
		/// <summary>
		/// Главная точка входа для приложения.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// TODO: search entry in regedit
			// HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Uplay Install 13
			// HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Ubisoft\Driver San Francisco
			// HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Ubisoft\Driver San Francisco\GameUpdate
			// HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Ubisoft\Launcher\Installs\13

			JsonConvert.DefaultSettings = () => new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				DefaultValueHandling = DefaultValueHandling.Include,
				TypeNameHandling = TypeNameHandling.None,
				NullValueHandling = NullValueHandling.Ignore,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
			};

			if(File.Exists(Constants.ConfigFilename))
			{
				string alcatrazConfig = File.ReadAllText(Constants.ConfigFilename);
				AlcatrazClientConfig.Instance = JsonConvert.DeserializeObject<AlcatrazClientConfig>(alcatrazConfig);
			}
			else
			{
				AlcatrazClientConfig.Instance = new AlcatrazClientConfig();
				AlcatrazClientConfig.Instance.UseProfile = Constants.NoProfile;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
