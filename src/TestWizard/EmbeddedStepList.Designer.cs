namespace TestWizard
{
	partial class EmbeddedStepList
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.wizardPageContainer1 = new AeroWizard.WizardPageContainer();
			this.backButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.wizardPage1 = new AeroWizard.WizardPage();
			this.wizardPage3 = new AeroWizard.WizardPage();
			this.wizardPage2 = new AeroWizard.WizardPage();
			this.nextButton = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.wizardPageLabel = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.wizardPageContainer1)).BeginInit();
			this.wizardPageContainer1.SuspendLayout();
			this.wizardPage1.SuspendLayout();
			this.wizardPage3.SuspendLayout();
			this.wizardPage2.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
			this.tableLayoutPanel1.Controls.Add(this.wizardPageContainer1, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.listBox1, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.wizardPageLabel, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 2, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// wizardPageContainer1
			// 
			this.wizardPageContainer1.BackButton = this.backButton;
			this.wizardPageContainer1.BackButtonText = "Back";
			this.wizardPageContainer1.CancelButton = this.cancelButton;
			this.wizardPageContainer1.CancelButtonText = "Cancel";
			this.wizardPageContainer1.Controls.Add(this.wizardPage3);
			this.wizardPageContainer1.Controls.Add(this.wizardPage2);
			this.wizardPageContainer1.Controls.Add(this.wizardPage1);
			this.wizardPageContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizardPageContainer1.Location = new System.Drawing.Point(264, 63);
			this.wizardPageContainer1.Name = "wizardPageContainer1";
			this.wizardPageContainer1.NextButton = this.nextButton;
			this.wizardPageContainer1.Pages.Add(this.wizardPage1);
			this.wizardPageContainer1.Pages.Add(this.wizardPage2);
			this.wizardPageContainer1.Pages.Add(this.wizardPage3);
			this.wizardPageContainer1.Size = new System.Drawing.Size(533, 349);
			this.wizardPageContainer1.TabIndex = 0;
			this.wizardPageContainer1.SelectedPageChanged += new System.EventHandler(this.wizardPageContainer1_SelectedPageChanged);
			// 
			// backButton
			// 
			this.backButton.Location = new System.Drawing.Point(3, 3);
			this.backButton.Name = "backButton";
			this.backButton.Size = new System.Drawing.Size(75, 23);
			this.backButton.TabIndex = 2;
			this.backButton.Tag = AeroWizard.WizardCommandButtonState.Enabled;
			this.backButton.Text = "Back";
			this.backButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(165, 3);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 0;
			this.cancelButton.Tag = AeroWizard.WizardCommandButtonState.Disabled;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// wizardPage1
			// 
			this.wizardPage1.Controls.Add(this.label1);
			this.wizardPage1.Name = "wizardPage1";
			this.wizardPage1.Size = new System.Drawing.Size(533, 349);
			this.wizardPage1.TabIndex = 0;
			this.wizardPage1.Text = "Welcome";
			// 
			// wizardPage3
			// 
			this.wizardPage3.Controls.Add(this.label3);
			this.wizardPage3.IsFinishPage = true;
			this.wizardPage3.Name = "wizardPage3";
			this.wizardPage3.Size = new System.Drawing.Size(533, 349);
			this.wizardPage3.TabIndex = 2;
			this.wizardPage3.Text = "Last step";
			this.wizardPage3.Commit += new System.EventHandler<AeroWizard.WizardPageConfirmEventArgs>(this.wizardPage3_Commit);
			// 
			// wizardPage2
			// 
			this.wizardPage2.Controls.Add(this.label2);
			this.wizardPage2.Name = "wizardPage2";
			this.wizardPage2.Size = new System.Drawing.Size(533, 349);
			this.wizardPage2.TabIndex = 1;
			this.wizardPage2.Text = "Step 2";
			// 
			// nextButton
			// 
			this.nextButton.Location = new System.Drawing.Point(84, 3);
			this.nextButton.Name = "nextButton";
			this.nextButton.Size = new System.Drawing.Size(75, 23);
			this.nextButton.TabIndex = 1;
			this.nextButton.Tag = AeroWizard.WizardCommandButtonState.Disabled;
			this.nextButton.Text = "Next";
			this.nextButton.UseVisualStyleBackColor = true;
			// 
			// listBox1
			// 
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox1.Enabled = false;
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(33, 33);
			this.listBox1.Name = "listBox1";
			this.tableLayoutPanel1.SetRowSpan(this.listBox1, 3);
			this.listBox1.Size = new System.Drawing.Size(225, 414);
			this.listBox1.TabIndex = 1;
			// 
			// wizardPageLabel
			// 
			this.wizardPageLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.wizardPageLabel.AutoSize = true;
			this.wizardPageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.wizardPageLabel.Location = new System.Drawing.Point(264, 35);
			this.wizardPageLabel.Name = "wizardPageLabel";
			this.wizardPageLabel.Size = new System.Drawing.Size(57, 20);
			this.wizardPageLabel.TabIndex = 2;
			this.wizardPageLabel.Text = "label1";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.cancelButton);
			this.flowLayoutPanel1.Controls.Add(this.nextButton);
			this.flowLayoutPanel1.Controls.Add(this.backButton);
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(554, 418);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(243, 29);
			this.flowLayoutPanel1.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(97, 90);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "You\'re on page 1";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(86, 211);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "You\'re on page 2";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(240, 65);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "You\'re on page 3";
			// 
			// EmbeddedStepList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "EmbeddedStepList";
			this.Text = "EmbeddedStepList";
			this.Load += new System.EventHandler(this.EmbeddedStepList_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.wizardPageContainer1)).EndInit();
			this.wizardPageContainer1.ResumeLayout(false);
			this.wizardPage1.ResumeLayout(false);
			this.wizardPage1.PerformLayout();
			this.wizardPage3.ResumeLayout(false);
			this.wizardPage3.PerformLayout();
			this.wizardPage2.ResumeLayout(false);
			this.wizardPage2.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private AeroWizard.WizardPageContainer wizardPageContainer1;
		private AeroWizard.WizardPage wizardPage3;
		private AeroWizard.WizardPage wizardPage2;
		private AeroWizard.WizardPage wizardPage1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Label wizardPageLabel;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button nextButton;
		private System.Windows.Forms.Button backButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
	}
}