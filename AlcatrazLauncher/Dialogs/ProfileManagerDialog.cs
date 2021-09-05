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
			m_profileList.Items.AddRange(profiles.Select(x => $" {x} : { AlcatrazClientConfig.Instance.Profiles[x].Username ?? AlcatrazClientConfig.Instance.Profiles[x].AccountId }").ToArray());

			m_profileList.SelectedIndex = Array.IndexOf(profiles, AlcatrazClientConfig.Instance.UseProfile);
		}

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

		private void m_profileList_DoubleClick(object sender, EventArgs e)
		{
			if (m_profileList.SelectedIndex == -1)
				return;

			var str = m_profileList.SelectedItem.ToString();
			var splitArr = str.Split(':');
			var profileKey = splitArr[0].Trim();

			if (profileKey != Constants.OfficialProfileKey)
			{
				var dlg = new EditAlcatrazProfileDialog();

				dlg.AlcatrazProfileKey = profileKey;
				dlg.ShowDialog();

				RefreshProfileList();
			}
			else
			{
				MessageBox.Show(null, "Official profile has no properties.", "Alcatraz", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void m_profileList_KeyDown(object sender, KeyEventArgs e)
		{
			// Deletion of profile
			if(e.KeyCode == Keys.Delete)
			{
				if (m_profileList.SelectedIndex == -1)
					return;

				var result = MessageBox.Show(null, "Are you sure to delete selected profile?", "Alcatraz", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

				if (result == DialogResult.Yes)
				{
					var str = m_profileList.SelectedItem.ToString();
					var splitArr = str.Split(':');
					var profileKey = splitArr[0].Trim();

					AlcatrazClientConfig.Instance.Profiles.Remove(profileKey);

					if (AlcatrazClientConfig.Instance.UseProfile == profileKey)
						AlcatrazClientConfig.Instance.UseProfile = Constants.NoProfile;

					RefreshProfileList();
				}
			}
		}
	}
}
