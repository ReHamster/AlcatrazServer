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
	public partial class ProfileManagerDialog : Form
	{
		public ProfileManagerDialog()
		{
			InitializeComponent();
		}
		private void RefreshProfileList()
		{
			var profiles = AlcatrazClientConfig.Instance.Profiles.Keys.ToArray();

			m_addUbiProfile.Enabled = !profiles.Contains(Constants.OfficialProfileKey);
			m_registerBtn.Enabled = !profiles.Contains(Constants.AlcatrazProfileKey);
			//m_signInToAlcatraz.Enabled = !profiles.Contains(Constants.AlcatrazProfileKey);

			m_profileList.Items.Clear();
			m_profileList.Items.AddRange(profiles.Select(x => $" {x} : { AlcatrazClientConfig.Instance.Profiles[x].AccountId }").ToArray());

			m_profileList.SelectedIndex = Array.IndexOf(profiles, AlcatrazClientConfig.Instance.UseProfile);
		}

#if false
		private void m_loginBtn_Click(object sender, EventArgs e)
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
#endif

		private void m_addUbiProfile_Click(object sender, EventArgs e)
		{
			var dlg = new AddUbiProfileDialog();
			dlg.ShowDialog();

			RefreshProfileList();
		}

		private void ProfileManagerDialog_Load(object sender, EventArgs e)
		{
			RefreshProfileList();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void m_registerBtn_Click(object sender, EventArgs e)
		{
			var dlg = new RegisterAlcatrazUserDialog();
			dlg.ShowDialog();

			RefreshProfileList();
		}

		private void m_signInToAlcatraz_Click(object sender, EventArgs e)
		{
			var dlg = new SignInToAlcatrazDialog();
			dlg.ShowDialog();

			RefreshProfileList();
		}
	}
}
