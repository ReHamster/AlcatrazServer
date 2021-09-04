using AlcatrazLauncher.Helpers;
using AlcatrazLauncher.Session;
using System;
using Alcatraz.DTO.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;

namespace AlcatrazLauncher.Dialogs
{
	public partial class RegisterAlcatrazUserDialog : Form
	{
		public RegisterAlcatrazUserDialog()
		{
			InitializeComponent();
		}

		private void m_doneBtn_Click(object sender, EventArgs e)
		{
			if (m_loginText.Text.Length < 1)
			{
				MessageBox.Show(this, "Please create login", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if (m_gameNickname.Text.Length < 1)
			{
				MessageBox.Show(this, "Game nickname is required (you can always change it)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if (m_passText.Text.Length < 1)
			{
				MessageBox.Show(this, "Please create password", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			var serviceUrl = ConfigurationManager.AppSettings.Get(Constants.SERVICE_URL_KEY);
			var accessKey = ConfigurationManager.AppSettings.Get(Constants.SANDBOX_ACCESSKEY_KEY);
			var configKey = ConfigurationManager.AppSettings.Get(Constants.SANDBOX_CONFIGKEY_KEY);

			var config = new ProfileConfig
			{
				Username = m_loginText.Text,
				AccountId = m_gameNickname.Text,
				Password = m_passText.Text,

				ServiceUrl = serviceUrl,
				AccessKey = accessKey,
				ConfigKey = configKey
			};

			var api = new APISession(UIEventQueue.Get());

			var model = new UserRegisterModel
			{
				PlayerNickName = config.AccountId,
				Username = config.Username,
				Password = config.Password,
			};

			m_registerBtn.Enabled = false;

			// асинхронный запрос в API
			api.Account.Register(model,
				response => // если запрос выполнился
				{
					m_registerBtn.Enabled = true;

					if (response.StatusCode != HttpStatusCode.OK)
					{
						if (response.StatusCode == HttpStatusCode.Unauthorized)
						{
							// messagebox (bad login)
							MessageBox.Show(this, "Registration error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}

						MessageBox.Show(this, "Connection Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

						return;
					}

					MessageBox.Show(this, "Register success", "Noice", MessageBoxButtons.OK, MessageBoxIcon.Information);

					// add models
					if (AlcatrazClientConfig.Instance.Profiles.Count == 0)
						AlcatrazClientConfig.Instance.UseProfile = Constants.AlcatrazProfileKey;

					AlcatrazClientConfig.Instance.Profiles.Add(Constants.AlcatrazProfileKey, config);
					DialogResult = DialogResult.OK;
				},
				error => {
					MessageBox.Show(this, error.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					m_registerBtn.Enabled = true;
				});
		}
	}
}
