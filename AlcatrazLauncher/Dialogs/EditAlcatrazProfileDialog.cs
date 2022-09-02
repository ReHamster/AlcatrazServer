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

		private APISession CreateProfileAPISession(ProfileConfig config)
		{
			return new APISession(UIEventQueue.Get(), config.ServiceUrl);
		}

		private void ChangePasswordRequest()
		{
			var config = AlcatrazClientConfig.Instance.Profiles[AlcatrazProfileKey];
			var api = CreateProfileAPISession(config);

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
						if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.BadRequest)
						{
							var errorData = JsonConvert.DeserializeObject<ResultModel>(response.Content);

							// messagebox (bad login)
							MessageBox.Show(this, errorData.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
			var api = CreateProfileAPISession(config);

			var sessionInfo = SessionInfo.Get();
			var curLoginData = sessionInfo.LoginData;

			var model = new UserModel
			{
				Id = curLoginData.Id,
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
						if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.BadRequest)
						{
							var errorData = JsonConvert.DeserializeObject<ResultModel>(response.Content);

							// messagebox (bad login)
							MessageBox.Show(this, errorData.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}

						MessageBox.Show(this, "Unknown request error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

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
			if (m_loginText.Text.Length < 1)
			{
				MessageBox.Show(this, "Login cannot be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if (m_gameNickname.Text.Length < 1)
			{
				MessageBox.Show(this, "Game nickname cannot be empty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			UpdateUserRequest();

			if (m_passText.Text.Length > 0)
				ChangePasswordRequest();
		}

		private void EditAlcatrazProfileDialog_Load(object sender, EventArgs e)
		{
			var config = AlcatrazClientConfig.Instance.Profiles[AlcatrazProfileKey];
			var api = CreateProfileAPISession(config);

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
						if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.BadRequest)
						{
							var errorData = JsonConvert.DeserializeObject<ResultModel>(response.Content);

							// messagebox (bad login)
							MessageBox.Show(this, "Unable to authenticate with selected profile - " + errorData.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

		private void m_gameNickname_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = !Utils.CheckLoginCharacterAllowed(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete;
		}
	}
}
