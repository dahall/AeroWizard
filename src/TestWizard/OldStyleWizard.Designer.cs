namespace TestWizard
{
	partial class OldStyleWizard
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
			this.headerPanel = new System.Windows.Forms.Panel();
			this.headerImage = new System.Windows.Forms.PictureBox();
			this.subHeaderLabel = new System.Windows.Forms.Label();
			this.headerLabel = new System.Windows.Forms.Label();
			this.topDivider = new System.Windows.Forms.Label();
			this.bottomDivider = new System.Windows.Forms.Label();
			this.commandPanel = new System.Windows.Forms.Panel();
			this.backButton = new System.Windows.Forms.Button();
			this.nextButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.wizardPageContainer1 = new AeroWizard.WizardPageContainer();
			this.wizardPage2 = new AeroWizard.WizardPage();
			this.wizardPage3 = new AeroWizard.WizardPage();
			this.wizardPage1 = new AeroWizard.WizardPage();
			this.startEndPicture = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.headerPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.headerImage)).BeginInit();
			this.commandPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.wizardPageContainer1)).BeginInit();
			this.wizardPageContainer1.SuspendLayout();
			this.wizardPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.startEndPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// headerPanel
			// 
			this.headerPanel.BackColor = System.Drawing.SystemColors.Window;
			this.headerPanel.Controls.Add(this.headerImage);
			this.headerPanel.Controls.Add(this.subHeaderLabel);
			this.headerPanel.Controls.Add(this.headerLabel);
			this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.headerPanel.Location = new System.Drawing.Point(0, 0);
			this.headerPanel.Name = "headerPanel";
			this.headerPanel.Size = new System.Drawing.Size(480, 57);
			this.headerPanel.TabIndex = 2;
			// 
			// headerImage
			// 
			this.headerImage.Image = global::TestWizard.Properties.Resources.WizardHat_48;
			this.headerImage.Location = new System.Drawing.Point(426, 4);
			this.headerImage.Name = "headerImage";
			this.headerImage.Size = new System.Drawing.Size(49, 49);
			this.headerImage.TabIndex = 1;
			this.headerImage.TabStop = false;
			// 
			// subHeaderLabel
			// 
			this.subHeaderLabel.AutoSize = true;
			this.subHeaderLabel.Location = new System.Drawing.Point(12, 31);
			this.subHeaderLabel.Name = "subHeaderLabel";
			this.subHeaderLabel.Size = new System.Drawing.Size(74, 13);
			this.subHeaderLabel.TabIndex = 0;
			this.subHeaderLabel.Text = "<Sub-header>";
			// 
			// headerLabel
			// 
			this.headerLabel.AutoSize = true;
			this.headerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.headerLabel.Location = new System.Drawing.Point(12, 11);
			this.headerLabel.Name = "headerLabel";
			this.headerLabel.Size = new System.Drawing.Size(62, 13);
			this.headerLabel.TabIndex = 0;
			this.headerLabel.Text = "<Header>";
			// 
			// topDivider
			// 
			this.topDivider.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.topDivider.Dock = System.Windows.Forms.DockStyle.Top;
			this.topDivider.Location = new System.Drawing.Point(0, 57);
			this.topDivider.Name = "topDivider";
			this.topDivider.Size = new System.Drawing.Size(480, 2);
			this.topDivider.TabIndex = 3;
			// 
			// bottomDivider
			// 
			this.bottomDivider.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.bottomDivider.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomDivider.Enabled = false;
			this.bottomDivider.Location = new System.Drawing.Point(0, 313);
			this.bottomDivider.Name = "bottomDivider";
			this.bottomDivider.Size = new System.Drawing.Size(480, 2);
			this.bottomDivider.TabIndex = 4;
			// 
			// commandPanel
			// 
			this.commandPanel.Controls.Add(this.backButton);
			this.commandPanel.Controls.Add(this.nextButton);
			this.commandPanel.Controls.Add(this.cancelButton);
			this.commandPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.commandPanel.Location = new System.Drawing.Point(0, 315);
			this.commandPanel.Name = "commandPanel";
			this.commandPanel.Size = new System.Drawing.Size(480, 40);
			this.commandPanel.TabIndex = 5;
			// 
			// backButton
			// 
			this.backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.backButton.Location = new System.Drawing.Point(171, 9);
			this.backButton.Name = "backButton";
			this.backButton.Size = new System.Drawing.Size(97, 23);
			this.backButton.TabIndex = 2;
			this.backButton.Tag = AeroWizard.WizardCommandButtonState.Disabled;
			this.backButton.Text = "< Back";
			this.backButton.UseVisualStyleBackColor = true;
			// 
			// nextButton
			// 
			this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.nextButton.Location = new System.Drawing.Point(270, 9);
			this.nextButton.Name = "nextButton";
			this.nextButton.Size = new System.Drawing.Size(97, 23);
			this.nextButton.TabIndex = 3;
			this.nextButton.Tag = AeroWizard.WizardCommandButtonState.Enabled;
			this.nextButton.Text = "Next >";
			this.nextButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(373, 9);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(97, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Tag = AeroWizard.WizardCommandButtonState.Disabled;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// wizardPageContainer1
			// 
			this.wizardPageContainer1.BackButton = this.backButton;
			this.wizardPageContainer1.BackButtonText = "< Back";
			this.wizardPageContainer1.CancelButton = this.cancelButton;
			this.wizardPageContainer1.CancelButtonText = "Cancel";
			this.wizardPageContainer1.Controls.Add(this.wizardPage1);
			this.wizardPageContainer1.Controls.Add(this.wizardPage2);
			this.wizardPageContainer1.Controls.Add(this.wizardPage3);
			this.wizardPageContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizardPageContainer1.Location = new System.Drawing.Point(164, 59);
			this.wizardPageContainer1.Name = "wizardPageContainer1";
			this.wizardPageContainer1.NextButton = this.nextButton;
			this.wizardPageContainer1.Pages.Add(this.wizardPage1);
			this.wizardPageContainer1.Pages.Add(this.wizardPage2);
			this.wizardPageContainer1.Pages.Add(this.wizardPage3);
			this.wizardPageContainer1.Size = new System.Drawing.Size(316, 254);
			this.wizardPageContainer1.TabIndex = 0;
			this.wizardPageContainer1.Finished += new System.EventHandler(this.wizardPageContainer1_Finished);
			this.wizardPageContainer1.SelectedPageChanged += new System.EventHandler(this.wizardPageContainer1_SelectedPageChanged);
			// 
			// wizardPage2
			// 
			this.wizardPage2.Name = "wizardPage2";
			this.wizardPage2.Size = new System.Drawing.Size(316, 254);
			this.wizardPage2.TabIndex = 1;
			this.wizardPage2.Tag = "";
			this.wizardPage2.Text = "Page 2 - Middle|This is the middle page";
			this.wizardPage2.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPage2_Initialize);
			// 
			// wizardPage3
			// 
			this.wizardPage3.Name = "wizardPage3";
			this.wizardPage3.Size = new System.Drawing.Size(316, 254);
			this.wizardPage3.TabIndex = 2;
			this.wizardPage3.Tag = "";
			this.wizardPage3.Text = "Task Completed|You\'re all done!";
			// 
			// wizardPage1
			// 
			this.wizardPage1.Controls.Add(this.label1);
			this.wizardPage1.Name = "wizardPage1";
			this.wizardPage1.Size = new System.Drawing.Size(316, 254);
			this.wizardPage1.TabIndex = 0;
			this.wizardPage1.Text = "Welcom";
			this.wizardPage1.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPage1_Initialize);
			// 
			// startEndPicture
			// 
			this.startEndPicture.BackColor = System.Drawing.Color.Navy;
			this.startEndPicture.BackgroundImage = global::TestWizard.Properties.Resources.WizardHat_48;
			this.startEndPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.startEndPicture.Dock = System.Windows.Forms.DockStyle.Left;
			this.startEndPicture.Location = new System.Drawing.Point(0, 59);
			this.startEndPicture.Name = "startEndPicture";
			this.startEndPicture.Size = new System.Drawing.Size(164, 254);
			this.startEndPicture.TabIndex = 6;
			this.startEndPicture.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(20, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(95, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "<Introductory text>";
			// 
			// OldStyleWizard
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(480, 355);
			this.Controls.Add(this.wizardPageContainer1);
			this.Controls.Add(this.startEndPicture);
			this.Controls.Add(this.bottomDivider);
			this.Controls.Add(this.commandPanel);
			this.Controls.Add(this.topDivider);
			this.Controls.Add(this.headerPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OldStyleWizard";
			this.Text = "OldStyleWizard";
			this.headerPanel.ResumeLayout(false);
			this.headerPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.headerImage)).EndInit();
			this.commandPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.wizardPageContainer1)).EndInit();
			this.wizardPageContainer1.ResumeLayout(false);
			this.wizardPage1.ResumeLayout(false);
			this.wizardPage1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.startEndPicture)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private AeroWizard.WizardPageContainer wizardPageContainer1;
		private System.Windows.Forms.Panel headerPanel;
		private System.Windows.Forms.Label topDivider;
		private System.Windows.Forms.Label bottomDivider;
		private System.Windows.Forms.Button backButton;
		private System.Windows.Forms.Button cancelButton;
		private AeroWizard.WizardPage wizardPage3;
		private AeroWizard.WizardPage wizardPage2;
		private AeroWizard.WizardPage wizardPage1;
		private System.Windows.Forms.Button nextButton;
		private System.Windows.Forms.Panel commandPanel;
		private System.Windows.Forms.Label subHeaderLabel;
		private System.Windows.Forms.Label headerLabel;
		private System.Windows.Forms.PictureBox startEndPicture;
		private System.Windows.Forms.PictureBox headerImage;
		private System.Windows.Forms.Label label1;
	}
}