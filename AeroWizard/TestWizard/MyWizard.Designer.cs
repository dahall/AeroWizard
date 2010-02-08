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
			this.commandLink2 = new Microsoft.WindowsAPICodePack.Controls.WindowsForms.CommandLink();
			this.commandLink1 = new Microsoft.WindowsAPICodePack.Controls.WindowsForms.CommandLink();
			this.middlePage = new AeroWizard.WizardPage();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.endPage = new AeroWizard.WizardPage();
			this.label1 = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).BeginInit();
			this.introPage.SuspendLayout();
			this.middlePage.SuspendLayout();
			this.endPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// wizardControl1
			// 
			this.wizardControl1.AllowDrop = true;
			this.wizardControl1.Location = new System.Drawing.Point(0, 0);
			this.wizardControl1.Name = "wizardControl1";
			this.wizardControl1.Pages.Add(this.introPage);
			this.wizardControl1.Pages.Add(this.middlePage);
			this.wizardControl1.Pages.Add(this.endPage);
			this.wizardControl1.Size = new System.Drawing.Size(574, 415);
			this.wizardControl1.TabIndex = 0;
			this.wizardControl1.Title = "Modify System";
			// 
			// introPage
			// 
			this.introPage.AllowNext = false;
			this.introPage.Controls.Add(this.commandLink2);
			this.introPage.Controls.Add(this.commandLink1);
			this.introPage.Name = "introPage";
			this.introPage.Size = new System.Drawing.Size(527, 265);
			this.introPage.TabIndex = 0;
			this.introPage.Text = "Choose an activity";
			// 
			// commandLink2
			// 
			this.commandLink2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.commandLink2.Location = new System.Drawing.Point(4, 70);
			this.commandLink2.Name = "commandLink2";
			this.commandLink2.NoteText = "Starting at C:\\, delete all files in all subdirectories.";
			this.commandLink2.ShieldIcon = true;
			this.commandLink2.Size = new System.Drawing.Size(453, 60);
			this.commandLink2.TabIndex = 1;
			this.commandLink2.Text = "Trash everything";
			this.commandLink2.UseVisualStyleBackColor = true;
			this.commandLink2.Click += new System.EventHandler(this.commandLink2_Click);
			// 
			// commandLink1
			// 
			this.commandLink1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.commandLink1.Location = new System.Drawing.Point(4, 4);
			this.commandLink1.Name = "commandLink1";
			this.commandLink1.NoteText = "Use standard system tools to clean up unnecessary files.";
			this.commandLink1.Size = new System.Drawing.Size(453, 60);
			this.commandLink1.TabIndex = 0;
			this.commandLink1.Text = "Clean up system safely";
			this.commandLink1.UseVisualStyleBackColor = true;
			this.commandLink1.Click += new System.EventHandler(this.commandLink1_Click);
			// 
			// middlePage
			// 
			this.middlePage.Controls.Add(this.checkBox1);
			this.middlePage.Controls.Add(this.linkLabel1);
			this.middlePage.IsFinishPage = true;
			this.middlePage.Name = "middlePage";
			this.middlePage.Size = new System.Drawing.Size(527, 265);
			this.middlePage.TabIndex = 1;
			this.middlePage.Text = "Launch System Cleanup";
			this.middlePage.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.middlePage_Initialize);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(6, 41);
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
			this.linkLabel1.Location = new System.Drawing.Point(3, 0);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(116, 15);
			this.linkLabel1.TabIndex = 0;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Launch SysClean.exe";
			// 
			// endPage
			// 
			this.endPage.Controls.Add(this.label1);
			this.endPage.Controls.Add(this.progressBar1);
			this.endPage.Name = "endPage";
			this.endPage.Size = new System.Drawing.Size(527, 265);
			this.endPage.TabIndex = 2;
			this.endPage.Text = "Bad Choice";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(4, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(185, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "Hosing your system. Please wait...";
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.Location = new System.Drawing.Point(7, 23);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(400, 23);
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.progressBar1.TabIndex = 0;
			// 
			// MyWizard
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(574, 415);
			this.Controls.Add(this.wizardControl1);
			this.MaximizeBox = false;
			this.Name = "MyWizard";
			this.ShowIcon = false;
			((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).EndInit();
			this.introPage.ResumeLayout(false);
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
		private Microsoft.WindowsAPICodePack.Controls.WindowsForms.CommandLink commandLink2;
		private Microsoft.WindowsAPICodePack.Controls.WindowsForms.CommandLink commandLink1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox checkBox1;
	}
}

