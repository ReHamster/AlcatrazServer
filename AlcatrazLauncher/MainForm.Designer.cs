
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.m_curProfileCombo = new System.Windows.Forms.ComboBox();
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
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(641, 284);
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// m_curProfileCombo
			// 
			this.m_curProfileCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_curProfileCombo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.m_curProfileCombo.FormattingEnabled = true;
			this.m_curProfileCombo.Location = new System.Drawing.Point(223, 306);
			this.m_curProfileCombo.Name = "m_curProfileCombo";
			this.m_curProfileCombo.Size = new System.Drawing.Size(241, 25);
			this.m_curProfileCombo.TabIndex = 2;
			this.m_curProfileCombo.SelectedIndexChanged += new System.EventHandler(this.m_curProfileCombo_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(165, 309);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(52, 17);
			this.label1.TabIndex = 6;
			this.label1.Text = "Profile:";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(665, 346);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_curProfileCombo);
			this.Controls.Add(this.pictureBox1);
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
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ComboBox m_curProfileCombo;
		private System.Windows.Forms.Label label1;
	}
}

