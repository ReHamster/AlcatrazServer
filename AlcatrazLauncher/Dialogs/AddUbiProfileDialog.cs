using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlcatrazLauncher.Dialogs
{
	public partial class AddUbiProfileDialog : Form
	{
		public AddUbiProfileDialog()
		{
			InitializeComponent();
		}

		private void m_doneBtn_Click(object sender, EventArgs e)
		{
			if(m_loginText.Text.Length < 1)
			{
				MessageBox.Show(this, "Ubisoft login is required\n-Use old Ubisoft login", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if (m_passText.Text.Length < 1)
			{
				MessageBox.Show(this, "Password for Ubisoft login is required", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if (m_cdkeyText.Text.Length < 23)
			{
				MessageBox.Show(this, "CD key for Driver San Francisco is required", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			var config = new ProfileConfig
			{
				AccountId = m_loginText.Text,
				Password = m_passText.Text,
				GameKey = m_cdkeyText.Text
			};

			if (AlcatrazClientConfig.Instance.Profiles.Count == 0)
				AlcatrazClientConfig.Instance.UseProfile = Constants.OfficialProfileKey;

			AlcatrazClientConfig.Instance.Profiles.Add(Constants.OfficialProfileKey, config);

			DialogResult = DialogResult.OK;
		}
	}
}
