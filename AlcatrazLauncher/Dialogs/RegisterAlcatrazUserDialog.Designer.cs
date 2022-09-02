
namespace AlcatrazLauncher.Dialogs
{
	partial class RegisterAlcatrazUserDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterAlcatrazUserDialog));
			this.m_registerBtn = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.m_loginText = new System.Windows.Forms.TextBox();
			this.m_passText = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.m_gameNickname = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// m_registerBtn
			// 
			this.m_registerBtn.Location = new System.Drawing.Point(225, 102);
			this.m_registerBtn.Name = "m_registerBtn";
			this.m_registerBtn.Size = new System.Drawing.Size(109, 33);
			this.m_registerBtn.TabIndex = 4;
			this.m_registerBtn.Text = "Register";
			this.m_registerBtn.UseVisualStyleBackColor = true;
			this.m_registerBtn.Click += new System.EventHandler(this.m_doneBtn_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 13);
			this.label2.TabIndex = 15;
			this.label2.Text = "Password";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 13);
			this.label1.TabIndex = 14;
			this.label1.Text = "Login";
			// 
			// m_loginText
			// 
			this.m_loginText.Location = new System.Drawing.Point(183, 12);
			this.m_loginText.MaxLength = 64;
			this.m_loginText.Name = "m_loginText";
			this.m_loginText.Size = new System.Drawing.Size(151, 20);
			this.m_loginText.TabIndex = 1;
			// 
			// m_passText
			// 
			this.m_passText.Location = new System.Drawing.Point(183, 38);
			this.m_passText.Name = "m_passText";
			this.m_passText.PasswordChar = '*';
			this.m_passText.Size = new System.Drawing.Size(151, 20);
			this.m_passText.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(13, 67);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(84, 13);
			this.label3.TabIndex = 17;
			this.label3.Text = "Game nickname";
			// 
			// m_gameNickname
			// 
			this.m_gameNickname.Location = new System.Drawing.Point(183, 64);
			this.m_gameNickname.MaxLength = 16;
			this.m_gameNickname.Name = "m_gameNickname";
			this.m_gameNickname.Size = new System.Drawing.Size(151, 20);
			this.m_gameNickname.TabIndex = 3;
			this.m_gameNickname.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.m_gameNickname_KeyPress);
			// 
			// RegisterAlcatrazUserDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(352, 147);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.m_gameNickname);
			this.Controls.Add(this.m_registerBtn);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_loginText);
			this.Controls.Add(this.m_passText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "RegisterAlcatrazUserDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Alcatraz Account registration";
			this.Load += new System.EventHandler(this.RegisterAlcatrazUserDialog_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button m_registerBtn;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox m_loginText;
		private System.Windows.Forms.TextBox m_passText;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox m_gameNickname;
	}
}