
namespace AlcatrazLauncher.Dialogs
{
	partial class AddUbiProfileDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddUbiProfileDialog));
			this.m_cdkeyText = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.m_loginText = new System.Windows.Forms.TextBox();
			this.m_passText = new System.Windows.Forms.TextBox();
			this.m_doneBtn = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_cdkeyText
			// 
			this.m_cdkeyText.Location = new System.Drawing.Point(195, 65);
			this.m_cdkeyText.Name = "m_cdkeyText";
			this.m_cdkeyText.Size = new System.Drawing.Size(151, 20);
			this.m_cdkeyText.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(25, 68);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(43, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "CD Key";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(25, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "Password";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(25, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 13);
			this.label1.TabIndex = 9;
			this.label1.Text = "Login";
			// 
			// m_loginText
			// 
			this.m_loginText.Location = new System.Drawing.Point(195, 13);
			this.m_loginText.MaxLength = 16;
			this.m_loginText.Name = "m_loginText";
			this.m_loginText.Size = new System.Drawing.Size(151, 20);
			this.m_loginText.TabIndex = 1;
			// 
			// m_passText
			// 
			this.m_passText.Location = new System.Drawing.Point(195, 39);
			this.m_passText.Name = "m_passText";
			this.m_passText.PasswordChar = '*';
			this.m_passText.Size = new System.Drawing.Size(151, 20);
			this.m_passText.TabIndex = 2;
			// 
			// m_doneBtn
			// 
			this.m_doneBtn.Location = new System.Drawing.Point(237, 227);
			this.m_doneBtn.Name = "m_doneBtn";
			this.m_doneBtn.Size = new System.Drawing.Size(109, 33);
			this.m_doneBtn.TabIndex = 4;
			this.m_doneBtn.Text = "Done";
			this.m_doneBtn.UseVisualStyleBackColor = true;
			this.m_doneBtn.Click += new System.EventHandler(this.m_doneBtn_Click);
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label4.Location = new System.Drawing.Point(15, 97);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(331, 127);
			this.label4.TabIndex = 12;
			this.label4.Text = resources.GetString("label4.Text");
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// AddUbiProfileDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(371, 270);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.m_doneBtn);
			this.Controls.Add(this.m_cdkeyText);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_loginText);
			this.Controls.Add(this.m_passText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "AddUbiProfileDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add Ubisoft account";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox m_cdkeyText;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox m_loginText;
		private System.Windows.Forms.TextBox m_passText;
		private System.Windows.Forms.Button m_doneBtn;
		private System.Windows.Forms.Label label4;
	}
}