
namespace AlcatrazLauncher
{
	partial class MainForm
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.m_launchBtn = new System.Windows.Forms.Button();
			this.m_settingsBtn = new System.Windows.Forms.Button();
			this.m_loginBtn = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.m_serverNameText = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// m_launchBtn
			// 
			this.m_launchBtn.Location = new System.Drawing.Point(484, 302);
			this.m_launchBtn.Name = "m_launchBtn";
			this.m_launchBtn.Size = new System.Drawing.Size(169, 32);
			this.m_launchBtn.TabIndex = 0;
			this.m_launchBtn.Text = "Launch Driver San Francisco";
			this.m_launchBtn.UseVisualStyleBackColor = true;
			this.m_launchBtn.Click += new System.EventHandler(this.m_launchBtn_Click);
			// 
			// m_settingsBtn
			// 
			this.m_settingsBtn.Location = new System.Drawing.Point(12, 302);
			this.m_settingsBtn.Name = "m_settingsBtn";
			this.m_settingsBtn.Size = new System.Drawing.Size(129, 32);
			this.m_settingsBtn.TabIndex = 1;
			this.m_settingsBtn.Text = "Settings";
			this.m_settingsBtn.UseVisualStyleBackColor = true;
			this.m_settingsBtn.Click += new System.EventHandler(this.m_settingsBtn_Click);
			// 
			// m_loginBtn
			// 
			this.m_loginBtn.Location = new System.Drawing.Point(369, 302);
			this.m_loginBtn.Name = "m_loginBtn";
			this.m_loginBtn.Size = new System.Drawing.Size(109, 32);
			this.m_loginBtn.TabIndex = 2;
			this.m_loginBtn.Text = "Test connection";
			this.m_loginBtn.UseVisualStyleBackColor = true;
			this.m_loginBtn.Click += new System.EventHandler(this.button1_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(641, 284);
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// m_serverNameText
			// 
			this.m_serverNameText.AutoSize = true;
			this.m_serverNameText.Location = new System.Drawing.Point(241, 312);
			this.m_serverNameText.Name = "m_serverNameText";
			this.m_serverNameText.Size = new System.Drawing.Size(65, 13);
			this.m_serverNameText.TabIndex = 4;
			this.m_serverNameText.Text = "server name";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(159, 312);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Current server:";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(665, 346);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_serverNameText);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.m_loginBtn);
			this.Controls.Add(this.m_settingsBtn);
			this.Controls.Add(this.m_launchBtn);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Alcatraz Launcher";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button m_launchBtn;
		private System.Windows.Forms.Button m_settingsBtn;
		private System.Windows.Forms.Button m_loginBtn;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label m_serverNameText;
		private System.Windows.Forms.Label label1;
	}
}

