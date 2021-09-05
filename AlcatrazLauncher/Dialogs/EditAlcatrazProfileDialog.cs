using Alcatraz.DTO.Models;
using AlcatrazLauncher.Helpers;
using AlcatrazLauncher.Session;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlcatrazLauncher.Dialogs
{
	public partial class EditAlcatrazProfileDialog : Form
	{
		public string AlcatrazProfileKey;

		public EditAlcatrazProfileDialog()
		{
			InitializeComponent();
		}

		private void m_cancelBtn_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void ChangePasswordRequest()
		{
			var config = AlcatrazClientConfig.Instance.Profiles[AlcatrazProfileKey];

			var api = new APISession(UIEventQueue.Get());

			var model = new ChangePasswordRequest
			{
				NewPassword = m_passText.Text
			};

			m_saveBtn.Enabled = false;

			api.Account.ChangePassword(model,
				response =>
				{
					m_saveBtn.Enabled = true;

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

					config.Password = m_passText.Text;

					// add models
					if (AlcatrazClientConfig.Instance.Profiles.Count == 0)
						AlcatrazClientConfig.Instance.UseProfile = AlcatrazProfileKey;

					// replace config
					AlcatrazClientConfig.Instance.Profiles[AlcatrazProfileKey] = config;

					DialogResult = DialogResult.OK;
				});
		}

		private void UpdateUserRequest()
		{
			var config = AlcatrazClientConfig.Instance.Profiles[AlcatrazProfileKey];

			var api = new APISession(UIEventQueue.Get());

			var model = new UserModel
			{
				Id = SessionInfo.Get().LoginData.Id,
				Username = m_loginText.Text,
				PlayerNickName = m_gameNickname.Text
			};

			m_saveBtn.Enabled = false;

			api.Account.UpdateUser(model,
				response =>
				{
					m_saveBtn.Enabled = true;

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

					config.Username = m_loginText.Text;
					config.AccountId = m_gameNickname.Text;

					// add models
					if (AlcatrazClientConfig.Instance.Profiles.Count == 0)
						AlcatrazClientConfig.Instance.UseProfile = AlcatrazProfileKey;

					// replace config
					AlcatrazClientConfig.Instance.Profiles[AlcatrazProfileKey] = config;

					DialogResult = DialogResult.OK;
				});
		}

		private void m_saveBtn_Click(object sender, EventArgs e)
		{
			UpdateUserRequest();

			if (m_passText.Text.Length > 0)
				ChangePasswordRequest();
		}

		private void EditAlcatrazProfileDialog_Load(object sender, EventArgs e)
		{
			var config = AlcatrazClientConfig.Instance.Profiles[AlcatrazProfileKey];

			var api = new APISession(UIEventQueue.Get());

			var model = new AuthenticateRequest
			{
				Username = config.Username,
				Password = config.Password
			};

			m_loginText.Text = config.Username;
			m_gameNickname.Text = config.AccountId;

			api.Account.Authenticate(model,
				response =>
				{
					if (response.StatusCode != HttpStatusCode.OK)
					{
						if (response.StatusCode == HttpStatusCode.Unauthorized)
						{
							var errorData = JsonConvert.DeserializeObject<ErrorModel>(response.Content);

							// messagebox (bad login)
							MessageBox.Show(this, "Unable to authenticate with selected profile - " + errorData.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							DialogResult = DialogResult.Cancel;
							return;
						}

						MessageBox.Show(this, "Unknown authorization error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						DialogResult = DialogResult.Cancel;
						return;
					}

					// use login data
					var loginData = JsonConvert.DeserializeObject<AuthenticateResponse>(response.Content);

					var session = SessionInfo.Get();
					session.LoginData = loginData;

				},
				error => {
					MessageBox.Show(this, error.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				});
		}
	}
}
