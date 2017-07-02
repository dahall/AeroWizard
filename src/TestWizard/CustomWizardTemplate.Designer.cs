namespace TestWizard
{
	partial class CustomWizardTemplate
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
			this.backBtn = new System.Windows.Forms.Button();
			this.cancelBtn = new System.Windows.Forms.Button();
			this.nextBtn = new System.Windows.Forms.Button();
			this.wizardPageContainer1 = new AeroWizard.WizardPageContainer();
			this.wizardPage1 = new AeroWizard.WizardPage();
			this.headingControl = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.wizardPageContainer1)).BeginInit();
			this.wizardPageContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// backBtn
			// 
			this.backBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.backBtn.Location = new System.Drawing.Point(93, 204);
			this.backBtn.Name = "backBtn";
			this.backBtn.Size = new System.Drawing.Size(75, 23);
			this.backBtn.TabIndex = 1;
			this.backBtn.Text = "&Back";
			// 
			// cancelBtn
			// 
			this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelBtn.Location = new System.Drawing.Point(255, 204);
			this.cancelBtn.Name = "cancelBtn";
			this.cancelBtn.Size = new System.Drawing.Size(75, 23);
			this.cancelBtn.TabIndex = 1;
			this.cancelBtn.Text = "&Cancel";
			// 
			// nextBtn
			// 
			this.nextBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.nextBtn.Location = new System.Drawing.Point(174, 204);
			this.nextBtn.Name = "nextBtn";
			this.nextBtn.Size = new System.Drawing.Size(75, 23);
			this.nextBtn.TabIndex = 1;
			this.nextBtn.Text = "&Next";
			// 
			// wizardPageContainer1
			// 
			this.wizardPageContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.wizardPageContainer1.BackButton = this.backBtn;
			this.wizardPageContainer1.CancelButton = this.cancelBtn;
			this.wizardPageContainer1.Controls.Add(this.wizardPage1);
			this.wizardPageContainer1.Location = new System.Drawing.Point(12, 25);
			this.wizardPageContainer1.Name = "wizardPageContainer1";
			this.wizardPageContainer1.NextButton = this.nextBtn;
			this.wizardPageContainer1.Pages.Add(this.wizardPage1);
			this.wizardPageContainer1.Size = new System.Drawing.Size(318, 173);
			this.wizardPageContainer1.TabIndex = 2;
			this.wizardPageContainer1.Cancelling += new System.ComponentModel.CancelEventHandler(this.wizardPageContainer1_Cancelling);
			this.wizardPageContainer1.Finished += new System.EventHandler(this.wizardPageContainer1_Finished);
			this.wizardPageContainer1.SelectedPageChanged += new System.EventHandler(this.wizardPageContainer1_SelectedPageChanged);
			// 
			// wizardPage1
			// 
			this.wizardPage1.Name = "wizardPage1";
			this.wizardPage1.Size = new System.Drawing.Size(318, 173);
			this.wizardPage1.TabIndex = 0;
			this.wizardPage1.Text = "Page 1";
			// 
			// headingControl
			// 
			this.headingControl.AutoSize = true;
			this.headingControl.Location = new System.Drawing.Point(9, 9);
			this.headingControl.Name = "headingControl";
			this.headingControl.Size = new System.Drawing.Size(59, 13);
			this.headingControl.TabIndex = 3;
			this.headingControl.Text = "<Heading>";
			// 
			// CustomWizardTemplate
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(342, 239);
			this.Controls.Add(this.headingControl);
			this.Controls.Add(this.wizardPageContainer1);
			this.Controls.Add(this.cancelBtn);
			this.Controls.Add(this.nextBtn);
			this.Controls.Add(this.backBtn);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "CustomWizardTemplate";
			((System.ComponentModel.ISupportInitialize)(this.wizardPageContainer1)).EndInit();
			this.wizardPageContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button backBtn;
		private System.Windows.Forms.Button cancelBtn;
		private System.Windows.Forms.Button nextBtn;
		private AeroWizard.WizardPageContainer wizardPageContainer1;
		private AeroWizard.WizardPage wizardPage1;
		private System.Windows.Forms.Label headingControl;
	}
}