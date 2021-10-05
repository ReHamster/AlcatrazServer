using AlcatrazLauncher.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Linq;
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
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			bool skipSearch = Environment.GetCommandLineArgs().Contains("-skipsearch");
			bool unInstall = Environment.GetCommandLineArgs().Contains("-uninstall");

			string GameInstallPath = "";
			
			if(!skipSearch)
				GameInstallPath = InstallationHelper.FindGameInstallationFolder();

			if(unInstall)
			{
				if(InstallationHelper.UnInstallAlcatraz(GameInstallPath))
				{
					MessageBox.Show($"Alcatraz is removed from {GameInstallPath}",
								"Information",
								MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

				return;
			}

			if (GameInstallPath.Length == 0)
			{
				MessageBox.Show("Driver San Francisco installation not found.\n\nYou will be prompted to located the game folder",
								"Warning",
								MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				GameInstallPath = InstallationHelper.PromptGameFolder();
				if (GameInstallPath.Length == 0)
					return;

				if(!InstallationHelper.StoreCustomGameFolder(GameInstallPath))
					return;
			}

			if(GameInstallPath.Length > 0)
			{
				if (!InstallationHelper.InstallAlcatraz(GameInstallPath))
					return;
			}
			else
			{
				MessageBox.Show($"Alcatraz cannot run as Driver San Francisco installation folder is not specified or cannot be found!",
							"Aborting...",
							MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// now application will work in game installation folder
			Directory.SetCurrentDirectory(GameInstallPath);

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

			Application.Run(new MainForm());
		}
	}
}
