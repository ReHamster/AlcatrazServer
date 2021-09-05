
namespace AlcatrazLauncher.Dialogs
{
	partial class EditAlcatrazProfileDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditAlcatrazProfileDialog));
			this.m_saveBtn = new System.Windows.Forms.Button();
			this.m_cancelBtn = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.m_gameNickname = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.m_loginText = new System.Windows.Forms.TextBox();
			this.m_passText = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// m_saveBtn
			// 
			this.m_saveBtn.Location = new System.Drawing.Point(233, 162);
			this.m_saveBtn.Name = "m_saveBtn";
			this.m_saveBtn.Size = new System.Drawing.Size(106, 31);
			this.m_saveBtn.TabIndex = 0;
			this.m_saveBtn.Text = "Save";
			this.m_saveBtn.UseVisualStyleBackColor = true;
			this.m_saveBtn.Click += new System.EventHandler(this.m_saveBtn_Click);
			// 
			// m_cancelBtn
			// 
			this.m_cancelBtn.Location = new System.Drawing.Point(121, 162);
			this.m_cancelBtn.Name = "m_cancelBtn";
			this.m_cancelBtn.Size = new System.Drawing.Size(106, 31);
			this.m_cancelBtn.TabIndex = 1;
			this.m_cancelBtn.Text = "Cancel";
			this.m_cancelBtn.UseVisualStyleBackColor = true;
			this.m_cancelBtn.Click += new System.EventHandler(this.m_cancelBtn_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(18, 67);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(84, 13);
			this.label3.TabIndex = 23;
			this.label3.Text = "Game nickname";
			// 
			// m_gameNickname
			// 
			this.m_gameNickname.Location = new System.Drawing.Point(188, 64);
			this.m_gameNickname.MaxLength = 16;
			this.m_gameNickname.Name = "m_gameNickname";
			this.m_gameNickname.Size = new System.Drawing.Size(151, 20);
			this.m_gameNickname.TabIndex = 20;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(18, 118);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 13);
			this.label2.TabIndex = 22;
			this.label2.Text = "Change password";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(18, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 13);
			this.label1.TabIndex = 21;
			this.label1.Text = "Login";
			// 
			// m_loginText
			// 
			this.m_loginText.Location = new System.Drawing.Point(188, 12);
			this.m_loginText.MaxLength = 64;
			this.m_loginText.Name = "m_loginText";
			this.m_loginText.Size = new System.Drawing.Size(151, 20);
			this.m_loginText.TabIndex = 18;
			// 
			// m_passText
			// 
			this.m_passText.Location = new System.Drawing.Point(188, 115);
			this.m_passText.Name = "m_passText";
			this.m_passText.PasswordChar = '*';
			this.m_passText.Size = new System.Drawing.Size(151, 20);
			this.m_passText.TabIndex = 19;
			// 
			// EditAlcatrazProfileDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(364, 209);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.m_gameNickname);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_loginText);
			this.Controls.Add(this.m_passText);
			this.Controls.Add(this.m_cancelBtn);
			this.Controls.Add(this.m_saveBtn);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EditAlcatrazProfileDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Profile properties";
			this.Load += new System.EventHandler(this.EditAlcatrazProfileDialog_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button m_saveBtn;
		private System.Windows.Forms.Button m_cancelBtn;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox m_gameNickname;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox m_loginText;
		private System.Windows.Forms.TextBox m_passText;
	}
}