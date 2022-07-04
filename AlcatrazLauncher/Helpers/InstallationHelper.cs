using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlcatrazLauncher.Helpers;
using Microsoft.Win32;
using System.Security.Principal;
using System.IO;
using System.Windows.Forms;

namespace AlcatrazLauncher.Helpers
{
	public static class InstallationHelper
	{
		public enum OrbitR2LoaderVersion
		{
			OriginalUbi,
			AlcatrazOrUnknown,
			DoesNotExist
		};

		public static OrbitR2LoaderVersion GetLoaderFileVersion(string fileName)
		{
			if (!File.Exists(fileName))
				return OrbitR2LoaderVersion.DoesNotExist;

			string fileSha1 = FileChecksumHelper.GetFileSHA1(fileName);

			if (fileSha1 == Constants.OrbitLoaderSHA1)
				return OrbitR2LoaderVersion.OriginalUbi;

			return OrbitR2LoaderVersion.AlcatrazOrUnknown;
		}

		public static string PromptGameFolder()
		{
			string GameInstallPath = "";

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
				}
			}

			return GameInstallPath;
		}

		public static bool StoreCustomGameFolder(string GameInstallPath)
		{
			// TODO: request elevated access
			try
			{
				GameInstallRegProperty defaultProperty = Constants.AlcatrazRegProperty;

				// this will also store Alcatraz configuration
				using (var view32 = RegistryKey.OpenBaseKey(defaultProperty.RegistryHive, RegistryView.Registry32))
				{
					var regPath = view32.OpenSubKey(defaultProperty.RegistryPath, true);
					{
						if (regPath == null)
							regPath = view32.CreateSubKey(defaultProperty.RegistryPath, true);

						regPath.SetValue(defaultProperty.InstallPathKey, GameInstallPath);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Please run Alcatraz launcher as Administrator and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			return true;
		}

		public static string FindGameInstallationFolder()
		{
			string GameInstallPath = "";

			// search entry in regedit
			foreach (var gameInstallProp in Constants.GameInstallRegProperties)
			{
				using (var view32 = RegistryKey.OpenBaseKey(gameInstallProp.RegistryHive, RegistryView.Registry32))
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

			return GameInstallPath;
		}

		public static bool InstallAlcatraz(string GameInstallPath)
		{
			var AlcatrazLoaderFullPath = Path.Combine(Directory.GetCurrentDirectory(), Constants.OrbitLoaderFilename);
			var OrbitLoaderFullPath = Path.Combine(GameInstallPath, Constants.OrbitLoaderFilename);
			var OrbitLoaderBackupPath = Path.Combine(GameInstallPath, Constants.OrbitLoaderFilename + ".bak");

			var AlcatrazLoaderVersion = GetLoaderFileVersion(AlcatrazLoaderFullPath);
			var OrbitLoaderVersion = GetLoaderFileVersion(OrbitLoaderFullPath);

			if(OrbitLoaderVersion != AlcatrazLoaderVersion)
			{
				// make backup of original file
				if(OrbitLoaderVersion == OrbitR2LoaderVersion.OriginalUbi)
				{
					// TODO: request elevated access
					try
					{
						File.Move(OrbitLoaderFullPath, OrbitLoaderBackupPath);
						MessageBox.Show($"Backup created:\n\n{OrbitLoaderBackupPath}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					catch (Exception ex)
					{
						MessageBox.Show("Please run Alcatraz launcher as Administrator and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return false;
					}
				}

				try
				{
					// copy new loader file to game folder
					File.Copy(AlcatrazLoaderFullPath, OrbitLoaderFullPath);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Please run Alcatraz launcher as Administrator and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}
			}

			return true;
		}

		public static bool UnInstallAlcatraz(string GameInstallPath)
		{
			var OrbitLoaderFullPath = Path.Combine(GameInstallPath, Constants.OrbitLoaderFilename);
			var OrbitLoaderBackupPath = Path.Combine(GameInstallPath, Constants.OrbitLoaderFilename + ".bak");

			var OrbitLoaderVersion = GetLoaderFileVersion(OrbitLoaderFullPath);
			var OrbitLoaderBackupVersion = GetLoaderFileVersion(OrbitLoaderBackupPath);

			// also request elevated access
			try
			{
				// this will also store Alcatraz configuration
				using (var view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
				{
					bool deleteRegKey = false;
					using (var regPath = view32.OpenSubKey(Constants.AlcatrazRegProperty.RegistryPath, false))
					{
						if (regPath != null)
							deleteRegKey = true;
					}

					if(deleteRegKey)
						view32.DeleteSubKeyTree(Constants.AlcatrazRegProperty.RegistryPath);
				}

				// restore backup
				if(OrbitLoaderVersion != OrbitLoaderBackupVersion && 
					OrbitLoaderBackupVersion == OrbitR2LoaderVersion.OriginalUbi &&
					OrbitLoaderVersion != OrbitR2LoaderVersion.OriginalUbi)
				{
					File.Delete(OrbitLoaderFullPath);
					File.Move(OrbitLoaderBackupPath, OrbitLoaderFullPath);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Please run Alcatraz launcher as Administrator and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}

			return true;
		}

	}
}
