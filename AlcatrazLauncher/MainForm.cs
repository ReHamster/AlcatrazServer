using Alcatraz.DTO.Models;
using AlcatrazLauncher.Dialogs;
using AlcatrazLauncher.Helpers;
using AlcatrazLauncher.Session;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlcatrazLauncher
{
	public partial class MainForm : Form
	{
		public static SettingsDialog SettingsDlg = new SettingsDialog();
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

		private void MainForm_Load(object sender, EventArgs e)
		{
			// начинаем обработку сессии.
			Idle = new EventHandler(Application_Idle);
			Application.Idle += Idle;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var api = new APISession(UIEventQueue.Get());

			var model = new AuthenticateRequest
			{
				Username = "Jellysoapy",
				Password = "test"
			};

			m_loginBtn.Enabled = false;

			// асинхронный запрос в API
			api.Account.Authenticate(model,
				response => // если запрос выполнился
				{
					m_loginBtn.Enabled = true;

					if (response.StatusCode != HttpStatusCode.OK)
					{
						if (response.StatusCode == HttpStatusCode.Unauthorized)
						{
							// messagebox (bad login)
							MessageBox.Show(this, "Authorization error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}

						MessageBox.Show(this, "Connection Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

						return;
					}

					// use login data
					var loginData = JsonConvert.DeserializeObject<AuthenticateResponse>(response.Content);

					var session = SessionInfo.Get();
					session.LoginData = loginData;

					MessageBox.Show(this, "Login success", "Noice", MessageBoxButtons.OK, MessageBoxIcon.Information);
				},
				error => {
					MessageBox.Show(this, error.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					m_loginBtn.Enabled = true;
				});
		}

		private void m_launchBtn_Click(object sender, EventArgs e)
		{
			try {
				var process = Process.Start("Driver.exe");
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, "Unable to start Driver San Francisco!\n - " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void m_settingsBtn_Click(object sender, EventArgs e)
		{
			var result = SettingsDlg.ShowDialog();
			if(result == DialogResult.OK)
			{

			}
		}
	}
}
