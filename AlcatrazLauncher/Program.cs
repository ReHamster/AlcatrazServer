using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Security.Principal;
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

			string GameInstallPath = "";

			// search entry in regedit
			using (var view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
			{
				foreach(var gameInstallProp in Constants.GameInstallRegProperties)
				{
					using (var regPath = view32.OpenSubKey(gameInstallProp.RegistryPath, false))
					{
						if (regPath == null)
							continue;

						var installPath = regPath.GetValue(gameInstallProp.InstallPathKey);
						if (installPath != null)
						{
							GameInstallPath = installPath.ToString();
							break;
						}

					}
				}
			}

			if(GameInstallPath.Length == 0)
			{
				MessageBox.Show("Driver San Francisco installation not found.\n\nYou will be prompted to located the game folder", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				using (var openFileDialog = new OpenFileDialog())
				{
					openFileDialog.Title = "Locate Driver San Francisco";
					openFileDialog.InitialDirectory = "C:\\";
					openFileDialog.Filter = "Driver San Francisco executable (Driver.exe)|Driver.exe|All files (*.*)|*.*";
					openFileDialog.FilterIndex = 1;
					openFileDialog.RestoreDirectory = true;

					if (openFileDialog.ShowDialog() == DialogResult.OK)
					{
						//Get the path of specified file
						GameInstallPath = Path.GetDirectoryName(openFileDialog.FileName);

						// also request elevated access
						try
						{
							// this will also store Alcatraz configuration
							using (var view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
							{
								var regPath = view32.OpenSubKey(Constants.AlcatrazRegProperty.RegistryPath, true);
								{
									if (regPath == null)
										regPath = view32.CreateSubKey(Constants.AlcatrazRegProperty.RegistryPath, true);

									regPath.SetValue(Constants.AlcatrazRegProperty.InstallPathKey, GameInstallPath);
								}
							}
						}
						catch (Exception ex)
						{
							MessageBox.Show("Please run Alcatraz launcher as Administrator and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
					}
				}
			}

			if(GameInstallPath.Length > 0)
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
