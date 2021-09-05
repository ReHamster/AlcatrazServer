
namespace AlcatrazLauncher.Dialogs
{
	partial class ProfileManagerDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfileManagerDialog));
			this.m_profileList = new System.Windows.Forms.ListBox();
			this.m_addUbiProfile = new System.Windows.Forms.Button();
			this.m_signInToAlcatraz = new System.Windows.Forms.Button();
			this.m_registerBtn = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_profileList
			// 
			this.m_profileList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.m_profileList.FormattingEnabled = true;
			this.m_profileList.ItemHeight = 17;
			this.m_profileList.Location = new System.Drawing.Point(12, 12);
			this.m_profileList.Name = "m_profileList";
			this.m_profileList.Size = new System.Drawing.Size(250, 157);
			this.m_profileList.TabIndex = 1;
			this.m_profileList.DoubleClick += new System.EventHandler(this.m_profileList_DoubleClick);
			this.m_profileList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_profileList_KeyDown);
			// 
			// m_addUbiProfile
			// 
			this.m_addUbiProfile.Location = new System.Drawing.Point(306, 141);
			this.m_addUbiProfile.Name = "m_addUbiProfile";
			this.m_addUbiProfile.Size = new System.Drawing.Size(141, 31);
			this.m_addUbiProfile.TabIndex = 2;
			this.m_addUbiProfile.Text = "Add Ubisoft account";
			this.m_addUbiProfile.UseVisualStyleBackColor = true;
			this.m_addUbiProfile.Click += new System.EventHandler(this.m_addUbiProfile_Click);
			// 
			// m_signInToAlcatraz
			// 
			this.m_signInToAlcatraz.Location = new System.Drawing.Point(306, 12);
			this.m_signInToAlcatraz.Name = "m_signInToAlcatraz";
			this.m_signInToAlcatraz.Size = new System.Drawing.Size(141, 31);
			this.m_signInToAlcatraz.TabIndex = 3;
			this.m_signInToAlcatraz.Text = "Sign in to Alcatraz";
			this.m_signInToAlcatraz.UseVisualStyleBackColor = true;
			this.m_signInToAlcatraz.Click += new System.EventHandler(this.m_signInToAlcatraz_Click);
			// 
			// m_registerBtn
			// 
			this.m_registerBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.m_registerBtn.Location = new System.Drawing.Point(306, 49);
			this.m_registerBtn.Name = "m_registerBtn";
			this.m_registerBtn.Size = new System.Drawing.Size(141, 31);
			this.m_registerBtn.TabIndex = 4;
			this.m_registerBtn.Text = "Register";
			this.m_registerBtn.UseVisualStyleBackColor = true;
			this.m_registerBtn.Click += new System.EventHandler(this.m_registerBtn_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 179);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(149, 26);
			this.label1.TabIndex = 5;
			this.label1.Text = "Double click - show properties\r\nDelete - remove profile";
			// 
			// ProfileManagerDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(459, 217);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_registerBtn);
			this.Controls.Add(this.m_signInToAlcatraz);
			this.Controls.Add(this.m_addUbiProfile);
			this.Controls.Add(this.m_profileList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ProfileManagerDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Profile Manager";
			this.Load += new System.EventHandler(this.ProfileManagerDialog_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ListBox m_profileList;
		private System.Windows.Forms.Button m_addUbiProfile;
		private System.Windows.Forms.Button m_signInToAlcatraz;
		private System.Windows.Forms.Button m_registerBtn;
		private System.Windows.Forms.Label label1;
	}
}