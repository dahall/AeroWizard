namespace TestWizard
{
	partial class AeroWizardTemplate
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
			this.wizardPage1 = new AeroWizard.WizardPage();
			((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).BeginInit();
			this.SuspendLayout();
			// 
			// wizardControl1
			// 
			this.wizardControl1.Location = new System.Drawing.Point(0, 0);
			this.wizardControl1.Name = "wizardControl1";
			this.wizardControl1.Pages.Add(this.wizardPage1);
			this.wizardControl1.Size = new System.Drawing.Size(574, 415);
			this.wizardControl1.TabIndex = 0;
			// 
			// wizardPage1
			// 
			this.wizardPage1.Name = "wizardPage1";
			this.wizardPage1.Size = new System.Drawing.Size(527, 260);
			this.wizardPage1.TabIndex = 0;
			this.wizardPage1.Text = "Page Title";
			// 
			// AeroWizardTemplate
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(574, 415);
			this.Controls.Add(this.wizardControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AeroWizardTemplate";
			((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private AeroWizard.WizardControl wizardControl1;
		private AeroWizard.WizardPage wizardPage1;
	}
}