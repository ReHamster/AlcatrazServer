using Alcatraz.DTO.Models;
using AlcatrazLauncher.Dialogs;
using AlcatrazLauncher.Helpers;
using AlcatrazLauncher.Session;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlcatrazLauncher
{
	public partial class MainForm : Form
	{
		public static ProfileManagerDialog ProfilesDlg = new ProfileManagerDialog();
		public static event EventHandler Idle;

		public MainForm()
		{
			InitializeComponent();
		}

		private void Application_Idle(Object sender, EventArgs e)
		{
			if (APISession.WebClient.Authenticator == null)
			{
				// try login?
			}

			// execute the pushed events
			UIEventQueue.Get().ExecuteEvents();
		}

		private void RefreshProfileList()
		{
			var profiles = AlcatrazClientConfig.Instance.Profiles.Keys.ToArray();

			m_curProfileCombo.Items.Clear();
			m_curProfileCombo.Items.AddRange(profiles.Select(x => $" {x} : { AlcatrazClientConfig.Instance.Profiles[x].Username }").ToArray());

			m_curProfileCombo.SelectedIndex = Array.IndexOf(profiles, AlcatrazClientConfig.Instance.UseProfile);
		}

		private void SaveConfiguration()
		{

			var settings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				ContractResolver = new DefaultContractResolver(),
				DefaultValueHandling = DefaultValueHandling.Include,
				TypeNameHandling = TypeNameHandling.None,
				NullValueHandling = NullValueHandling.Ignore,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
			};

			string alcatrazConfig = JsonConvert.SerializeObject(AlcatrazClientConfig.Instance, settings);

			File.WriteAllText(Constants.ConfigFilename, alcatrazConfig);

			/*
			if (File.Exists(Constants.ConfigFilename))
			{
				string alcatrazConfig = File.ReadAllText(Constants.ConfigFilename);
				AlcatrazClientConfig.Instance = JsonConvert.DeserializeObject<AlcatrazClientConfig>(alcatrazConfig);
			}
			else
			{
				AlcatrazClientConfig.Instance = new AlcatrazClientConfig();
				AlcatrazClientConfig.Instance.UseProfile = Constants.NoProfile;
			}*/
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			// начинаем обработку сессии.
			Idle = new EventHandler(Application_Idle);
			Application.Idle += Idle;

			RefreshProfileList();

			if (AlcatrazClientConfig.Instance.UseProfile == Constants.NoProfile)
			{
				var result = MessageBox.Show(null, "Would you like to configure your profiles?", "Alcatraz", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (result == DialogResult.Yes)
				{
					ProfilesDlg.ShowDialog();
					RefreshProfileList();
					SaveConfiguration();
				}
			}
		}

		private void m_launchBtn_Click(object sender, EventArgs e)
		{
			try {
				SaveConfiguration();
				var process = Process.Start("Driver.exe");
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, "Unable to start Driver San Francisco!\n - " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void m_settingsBtn_Click(object sender, EventArgs e)
		{
			ProfilesDlg.ShowDialog();
			RefreshProfileList();
			SaveConfiguration();
		}

		private void m_curProfileCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			var str = m_curProfileCombo.SelectedItem.ToString();
			var splitArr = str.Split(':');
			AlcatrazClientConfig.Instance.UseProfile = splitArr[0].Trim();
		}
	}
}
