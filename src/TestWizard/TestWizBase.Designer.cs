namespace TestWizard
{
	partial class TestWizBase
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestWizBase));
			this.backBtn = new System.Windows.Forms.Button();
			this.cancelBtn = new System.Windows.Forms.Button();
			this.nextBtn = new System.Windows.Forms.Button();
			this.wizardPageContainer1 = new AeroWizard.WizardPageContainer();
			this.wizardPage1 = new AeroWizard.WizardPage();
			this.label1 = new System.Windows.Forms.Label();
			this.wizardPage2 = new AeroWizard.WizardPage();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.wizardPage3 = new AeroWizard.WizardPage();
			this.label5 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.wizardPageContainer1)).BeginInit();
			this.wizardPageContainer1.SuspendLayout();
			this.wizardPage1.SuspendLayout();
			this.wizardPage2.SuspendLayout();
			this.wizardPage3.SuspendLayout();
			this.SuspendLayout();
			// 
			// backBtn
			// 
			this.backBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.backBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.backBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
			this.backBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
			this.backBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.backBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.backBtn.Location = new System.Drawing.Point(141, 434);
			this.backBtn.Name = "backBtn";
			this.backBtn.Size = new System.Drawing.Size(75, 23);
			this.backBtn.TabIndex = 1;
			this.backBtn.Tag = AeroWizard.WizardCommandButtonState.Disabled;
			this.backBtn.Text = "&Back";
			this.backBtn.UseVisualStyleBackColor = false;
			// 
			// cancelBtn
			// 
			this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.cancelBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
			this.cancelBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
			this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cancelBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.cancelBtn.Location = new System.Drawing.Point(303, 434);
			this.cancelBtn.Name = "cancelBtn";
			this.cancelBtn.Size = new System.Drawing.Size(75, 23);
			this.cancelBtn.TabIndex = 1;
			this.cancelBtn.Tag = AeroWizard.WizardCommandButtonState.Disabled;
			this.cancelBtn.Text = "&Cancel";
			this.cancelBtn.UseVisualStyleBackColor = false;
			this.cancelBtn.Click += new System.EventHandler(this.button1_Click);
			// 
			// nextBtn
			// 
			this.nextBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.nextBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.nextBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
			this.nextBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
			this.nextBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.nextBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.nextBtn.Location = new System.Drawing.Point(222, 434);
			this.nextBtn.Name = "nextBtn";
			this.nextBtn.Size = new System.Drawing.Size(75, 23);
			this.nextBtn.TabIndex = 1;
			this.nextBtn.Tag = AeroWizard.WizardCommandButtonState.Enabled;
			this.nextBtn.Text = "&Next";
			this.nextBtn.UseVisualStyleBackColor = false;
			// 
			// wizardPageContainer1
			// 
			this.wizardPageContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.wizardPageContainer1.BackButton = this.backBtn;
			this.wizardPageContainer1.CancelButton = this.cancelBtn;
			this.wizardPageContainer1.Controls.Add(this.wizardPage1);
			this.wizardPageContainer1.Controls.Add(this.wizardPage2);
			this.wizardPageContainer1.Controls.Add(this.wizardPage3);
			this.wizardPageContainer1.Location = new System.Drawing.Point(12, 93);
			this.wizardPageContainer1.Name = "wizardPageContainer1";
			this.wizardPageContainer1.NextButton = this.nextBtn;
			this.wizardPageContainer1.Pages.Add(this.wizardPage1);
			this.wizardPageContainer1.Pages.Add(this.wizardPage2);
			this.wizardPageContainer1.Pages.Add(this.wizardPage3);
			this.wizardPageContainer1.ShowProgressInTaskbarIcon = true;
			this.wizardPageContainer1.Size = new System.Drawing.Size(366, 335);
			this.wizardPageContainer1.TabIndex = 2;
			this.wizardPageContainer1.Finished += new System.EventHandler(this.wizardPageContainer1_Finished);
			this.wizardPageContainer1.SelectedPageChanged += new System.EventHandler(this.wizardPageContainer1_SelectedPageChanged);
			// 
			// wizardPage1
			// 
			this.wizardPage1.Controls.Add(this.label1);
			this.wizardPage1.Name = "wizardPage1";
			this.wizardPage1.Size = new System.Drawing.Size(366, 335);
			this.wizardPage1.TabIndex = 0;
			this.wizardPage1.Text = "Page 1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(280, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "This is another wizard form that is totally customized.";
			// 
			// wizardPage2
			// 
			this.wizardPage2.AllowNext = false;
			this.wizardPage2.Controls.Add(this.checkBox1);
			this.wizardPage2.Name = "wizardPage2";
			this.wizardPage2.Size = new System.Drawing.Size(366, 335);
			this.wizardPage2.TabIndex = 1;
			this.wizardPage2.Text = "Page 2";
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(4, 12);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(144, 17);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "Click here to proceed...";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// wizardPage3
			// 
			this.wizardPage3.Controls.Add(this.label5);
			this.wizardPage3.Controls.Add(this.textBox1);
			this.wizardPage3.Name = "wizardPage3";
			this.wizardPage3.Size = new System.Drawing.Size(366, 335);
			this.wizardPage3.TabIndex = 2;
			this.wizardPage3.Text = "Page 3";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(4, 6);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 13);
			this.label5.TabIndex = 1;
			this.label5.Text = "Your name:";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(74, 3);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(247, 22);
			this.textBox1.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.label2.Location = new System.Drawing.Point(57, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(189, 30);
			this.label2.TabIndex = 3;
			this.label2.Text = "My Cooler Wizard";
			// 
			// label3
			// 
			this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
			this.label3.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this.label3.Location = new System.Drawing.Point(9, 4);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(52, 55);
			this.label3.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.label4.Location = new System.Drawing.Point(12, 69);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(97, 21);
			this.label4.TabIndex = 3;
			this.label4.Text = "<Heading>";
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.Crimson;
			this.button1.FlatAppearance.BorderSize = 0;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.ForeColor = System.Drawing.Color.White;
			this.button1.Location = new System.Drawing.Point(358, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(27, 23);
			this.button1.TabIndex = 5;
			this.button1.Text = "X";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// TestWizBase
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.DimGray;
			this.ClientSize = new System.Drawing.Size(390, 469);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.wizardPageContainer1);
			this.Controls.Add(this.cancelBtn);
			this.Controls.Add(this.nextBtn);
			this.Controls.Add(this.backBtn);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "TestWizBase";
			this.Text = "TestWizBase";
			((System.ComponentModel.ISupportInitialize)(this.wizardPageContainer1)).EndInit();
			this.wizardPageContainer1.ResumeLayout(false);
			this.wizardPage1.ResumeLayout(false);
			this.wizardPage1.PerformLayout();
			this.wizardPage2.ResumeLayout(false);
			this.wizardPage2.PerformLayout();
			this.wizardPage3.ResumeLayout(false);
			this.wizardPage3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button backBtn;
		private System.Windows.Forms.Button cancelBtn;
		private System.Windows.Forms.Button nextBtn;
		private AeroWizard.WizardPageContainer wizardPageContainer1;
		private AeroWizard.WizardPage wizardPage3;
		private AeroWizard.WizardPage wizardPage2;
		private System.Windows.Forms.CheckBox checkBox1;
		private AeroWizard.WizardPage wizardPage1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
	}
}