
namespace AlcatrazLauncher.Dialogs
{
	partial class SignInToAlcatrazDialog
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
			this.m_registerBtn = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.m_loginText = new System.Windows.Forms.TextBox();
			this.m_passText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// m_registerBtn
			// 
			this.m_registerBtn.Location = new System.Drawing.Point(220, 81);
			this.m_registerBtn.Name = "m_registerBtn";
			this.m_registerBtn.Size = new System.Drawing.Size(109, 33);
			this.m_registerBtn.TabIndex = 18;
			this.m_registerBtn.Text = "Sign In";
			this.m_registerBtn.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 13);
			this.label2.TabIndex = 20;
			this.label2.Text = "Password";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 13);
			this.label1.TabIndex = 19;
			this.label1.Text = "Login";
			// 
			// m_loginText
			// 
			this.m_loginText.Location = new System.Drawing.Point(178, 12);
			this.m_loginText.MaxLength = 64;
			this.m_loginText.Name = "m_loginText";
			this.m_loginText.Size = new System.Drawing.Size(151, 20);
			this.m_loginText.TabIndex = 16;
			// 
			// m_passText
			// 
			this.m_passText.Location = new System.Drawing.Point(178, 38);
			this.m_passText.Name = "m_passText";
			this.m_passText.PasswordChar = '*';
			this.m_passText.Size = new System.Drawing.Size(151, 20);
			this.m_passText.TabIndex = 17;
			// 
			// SignInToAlcatrazDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(346, 123);
			this.Controls.Add(this.m_registerBtn);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_loginText);
			this.Controls.Add(this.m_passText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SignInToAlcatrazDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Sign In to existing Alcatraz account";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button m_registerBtn;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox m_loginText;
		private System.Windows.Forms.TextBox m_passText;
	}
}