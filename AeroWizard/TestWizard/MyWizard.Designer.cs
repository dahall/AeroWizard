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
			this.middlePage = new AeroWizard.WizardPage();
			this.endPage = new AeroWizard.WizardPage();
			((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).BeginInit();
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
			// 
			// introPage
			// 
			this.introPage.Name = "introPage";
			this.introPage.TabIndex = 0;
			this.introPage.Text = "Intro";
			// 
			// middlePage
			// 
			this.middlePage.Name = "middlePage";
			this.middlePage.TabIndex = 1;
			this.middlePage.Text = "Middle";
			// 
			// endPage
			// 
			this.endPage.Name = "endPage";
			this.endPage.TabIndex = 2;
			this.endPage.Text = "The End";
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
			this.ResumeLayout(false);

		}

		#endregion

		private AeroWizard.WizardControl wizardControl1;
		private AeroWizard.WizardPage introPage;
		private AeroWizard.WizardPage middlePage;
		private AeroWizard.WizardPage endPage;
	}
}

