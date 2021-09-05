using Alcatraz.DTO.Models;
using AlcatrazLauncher.Helpers;
using AlcatrazLauncher.Session;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlcatrazLauncher.Dialogs
{
	public partial class SignInToAlcatrazDialog : Form
	{
		public SignInToAlcatrazDialog()
		{
			InitializeComponent();
		}
		private void m_loginBtn_Click(object sender, EventArgs e)
		{
			var serviceUrl = ConfigurationManager.AppSettings.Get(Constants.SERVICE_URL_KEY);
			var accessKey = ConfigurationManager.AppSettings.Get(Constants.SANDBOX_ACCESSKEY_KEY);
			var configKey = ConfigurationManager.AppSettings.Get(Constants.SANDBOX_CONFIGKEY_KEY);

			var config = new ProfileConfig
			{
				Username = m_loginText.Text,
				AccountId = "",
				Password = m_passText.Text,

				ServiceUrl = serviceUrl,
				AccessKey = accessKey,
				ConfigKey = configKey
			};

			var api = new APISession(UIEventQueue.Get());

			var model = new AuthenticateRequest
			{
				Username = m_loginText.Text,
				Password = m_passText.Text
			};

			m_loginBtn.Enabled = false;

			api.Account.Authenticate(model,
				response =>
				{
					m_loginBtn.Enabled = true;

					if (response.StatusCode != HttpStatusCode.OK)
					{
						if (response.StatusCode == HttpStatusCode.Unauthorized)
						{
							var errorData = JsonConvert.DeserializeObject<ErrorModel>(response.Content);

							// messagebox (bad login)
							MessageBox.Show(this, errorData.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}

						MessageBox.Show(this, "Unknown authorization error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

						return;
					}

					// use login data
					var loginData = JsonConvert.DeserializeObject<AuthenticateResponse>(response.Content);

					var session = SessionInfo.Get();
					session.LoginData = loginData;
					config.AccountId = loginData.PlayerNickName;

					MessageBox.Show(this, $"You've successfully signed in as { loginData.Username }", "Login success", MessageBoxButtons.OK, MessageBoxIcon.Information);

					// add models
					if (AlcatrazClientConfig.Instance.Profiles.Count == 0)
						AlcatrazClientConfig.Instance.UseProfile = Constants.AlcatrazProfileKey;

					// replace config
					AlcatrazClientConfig.Instance.Profiles[Constants.AlcatrazProfileKey] = config;

					DialogResult = DialogResult.OK;
				},
				error => {
					MessageBox.Show(this, error.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					m_loginBtn.Enabled = true;
				});
		}

		private void SignInToAlcatrazDialog_Load(object sender, EventArgs e)
		{
			AcceptButton = m_loginBtn;
		}
	}
}
