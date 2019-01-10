namespace TestWizard
{
	partial class MyWizard
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
			this.wizardControl1 = new AeroWizard.WizardControl();
			this.introPage = new AeroWizard.WizardPage();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.suppressedPage = new AeroWizard.WizardPage();
			this.label2 = new System.Windows.Forms.Label();
			this.questionPage = new AeroWizard.WizardPage();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.middlePage = new AeroWizard.WizardPage();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.endPage = new AeroWizard.WizardPage();
			this.label1 = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).BeginInit();
			this.introPage.SuspendLayout();
			this.suppressedPage.SuspendLayout();
			this.questionPage.SuspendLayout();
			this.middlePage.SuspendLayout();
			this.endPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// wizardControl1
			// 
			this.wizardControl1.BackColor = System.Drawing.Color.White;
			this.wizardControl1.ClassicStyle = AeroWizard.WizardClassicStyle.Automatic;
			this.wizardControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizardControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.wizardControl1.Location = new System.Drawing.Point(0, 0);
			this.wizardControl1.Name = "wizardControl1";
			this.wizardControl1.Pages.Add(this.introPage);
			this.wizardControl1.Pages.Add(this.suppressedPage);
			this.wizardControl1.Pages.Add(this.questionPage);
			this.wizardControl1.Pages.Add(this.middlePage);
			this.wizardControl1.Pages.Add(this.endPage);
			this.wizardControl1.ShowProgressInTaskbarIcon = true;
			this.wizardControl1.Size = new System.Drawing.Size(574, 415);
			this.wizardControl1.TabIndex = 0;
			this.wizardControl1.Title = "Modify System";
			// 
			// introPage
			// 
			this.introPage.AllowNext = false;
			this.introPage.Controls.Add(this.button2);
			this.introPage.Controls.Add(this.button1);
			this.introPage.HelpText = "Do you hate this?";
			this.introPage.Name = "introPage";
			this.introPage.Size = new System.Drawing.Size(527, 261);
			this.introPage.TabIndex = 0;
			this.introPage.Text = "Choose an activity";
			this.introPage.HelpClicked += new System.EventHandler(this.introPage_HelpClicked);
			this.introPage.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.introPage_Initialize);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(3, 32);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(227, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Trash everything";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.commandLink2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(3, 3);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(227, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Cleanup system";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.commandLink1_Click);
			// 
			// suppressedPage
			// 
			this.suppressedPage.Controls.Add(this.label2);
			this.suppressedPage.Name = "suppressedPage";
			this.suppressedPage.Size = new System.Drawing.Size(527, 261);
			this.suppressedPage.Suppress = true;
			this.suppressedPage.TabIndex = 3;
			this.suppressedPage.Text = "Suppressed";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(188, 113);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(138, 15);
			this.label2.TabIndex = 0;
			this.label2.Text = "You should never see me";
			// 
			// questionPage
			// 
			this.questionPage.AllowNext = false;
			this.questionPage.Controls.Add(this.checkBox2);
			this.questionPage.Name = "questionPage";
			this.questionPage.Size = new System.Drawing.Size(527, 261);
			this.questionPage.TabIndex = 4;
			this.questionPage.Text = "Are you sure?";
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new System.Drawing.Point(3, 0);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(43, 19);
			this.checkBox2.TabIndex = 0;
			this.checkBox2.Text = "Yes";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
			// 
			// middlePage
			// 
			this.middlePage.Controls.Add(this.checkBox1);
			this.middlePage.Controls.Add(this.linkLabel1);
			this.middlePage.IsFinishPage = true;
			this.middlePage.Name = "middlePage";
			this.middlePage.ShowCancel = false;
			this.middlePage.ShowNext = false;
			this.middlePage.Size = new System.Drawing.Size(527, 261);
			this.middlePage.TabIndex = 1;
			this.middlePage.Text = "Launch System Cleanup";
			this.middlePage.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.middlePage_Initialize);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(3, 31);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(233, 19);
			this.checkBox1.TabIndex = 1;
			this.checkBox1.Text = "Desktop Window Composition Enabled";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(0, 0);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(116, 15);
			this.linkLabel1.TabIndex = 0;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Launch SysClean.exe";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// endPage
			// 
			this.endPage.Controls.Add(this.label1);
			this.endPage.Controls.Add(this.progressBar1);
			this.endPage.Name = "endPage";
			this.endPage.Size = new System.Drawing.Size(527, 259);
			this.endPage.TabIndex = 2;
			this.endPage.Text = "Bad Choice";
			this.endPage.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.endPage_Initialize);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(185, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "Hosing your system. Please wait...";
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.Location = new System.Drawing.Point(3, 28);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(370, 23);
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.progressBar1.TabIndex = 0;
			// 
			// MyWizard
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(574, 415);
			this.Controls.Add(this.wizardControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "MyWizard";
			this.RightToLeftLayout = true;
			((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).EndInit();
			this.introPage.ResumeLayout(false);
			this.introPage.PerformLayout();
			this.suppressedPage.ResumeLayout(false);
			this.suppressedPage.PerformLayout();
			this.questionPage.ResumeLayout(false);
			this.questionPage.PerformLayout();
			this.middlePage.ResumeLayout(false);
			this.middlePage.PerformLayout();
			this.endPage.ResumeLayout(false);
			this.endPage.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private AeroWizard.WizardControl wizardControl1;
		private AeroWizard.WizardPage introPage;
		private AeroWizard.WizardPage middlePage;
		private AeroWizard.WizardPage endPage;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private AeroWizard.WizardPage suppressedPage;
		private System.Windows.Forms.Label label2;
		private AeroWizard.WizardPage questionPage;
		private System.Windows.Forms.CheckBox checkBox2;
	}
}

