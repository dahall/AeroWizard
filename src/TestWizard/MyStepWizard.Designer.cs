namespace TestWizard
{
	partial class MyStepWizard
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
			this.wizardControl1 = new AeroWizard.StepWizardControl();
			this.wizardPage1 = new AeroWizard.WizardPage();
			this.wizardPage3 = new AeroWizard.WizardPage();
			this.wizardPage4 = new AeroWizard.WizardPage();
			this.wizardPage5 = new AeroWizard.WizardPage();
			((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).BeginInit();
			this.SuspendLayout();
			// 
			// wizardControl1
			// 
			this.wizardControl1.ClassicStyle = AeroWizard.WizardClassicStyle.Automatic;
			this.wizardControl1.Location = new System.Drawing.Point(0, 0);
			this.wizardControl1.Name = "wizardControl1";
			this.wizardControl1.Pages.Add(this.wizardPage1);
			this.wizardControl1.Pages.Add(this.wizardPage3);
			this.wizardControl1.Pages.Add(this.wizardPage4);
			this.wizardControl1.Pages.Add(this.wizardPage5);
			this.wizardControl1.Size = new System.Drawing.Size(768, 482);
			this.wizardControl1.StepListFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.wizardControl1.TabIndex = 0;
			// 
			// wizardPage1
			// 
			this.wizardPage1.Name = "wizardPage1";
			this.wizardPage1.Size = new System.Drawing.Size(570, 330);
			this.wizardPage1.TabIndex = 0;
			this.wizardPage1.Text = "Page 1";
			// 
			// wizardPage3
			// 
			this.wizardPage3.Name = "wizardPage3";
			this.wizardPage3.Size = new System.Drawing.Size(570, 329);
			this.wizardPage3.TabIndex = 2;
			this.wizardPage3.Text = "Page 3";
			// 
			// wizardPage4
			// 
			this.wizardPage4.Name = "wizardPage4";
			this.wizardPage4.Size = new System.Drawing.Size(570, 329);
			this.wizardPage4.TabIndex = 3;
			this.wizardPage4.Text = "Page 4";
			// 
			// wizardPage5
			// 
			this.wizardPage5.Name = "wizardPage5";
			this.wizardPage5.Size = new System.Drawing.Size(571, 327);
			this.wizardPage5.TabIndex = 4;
			this.wizardPage5.Text = "Page 5";
			// 
			// MyStepWizard
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(768, 482);
			this.Controls.Add(this.wizardControl1);
			this.Name = "MyStepWizard";
			this.Text = "MyStepWizard";
			((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private AeroWizard.StepWizardControl wizardControl1;
        private AeroWizard.WizardPage wizardPage1;
		private AeroWizard.WizardPage wizardPage3;
		private AeroWizard.WizardPage wizardPage4;
        private AeroWizard.WizardPage wizardPage5;
	}
}