namespace TestWizard
{
	partial class Win10Wiz
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
			this.components = new System.ComponentModel.Container();
			this.backBtn = new System.Windows.Forms.Button();
			this.cancelBtn = new System.Windows.Forms.Button();
			this.nextBtn = new System.Windows.Forms.Button();
			this.wizardPageContainer1 = new AeroWizard.WizardPageContainer();
			this.wizardPage2 = new AeroWizard.WizardPage();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.wizardPage3 = new AeroWizard.WizardPage();
			this.label5 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.wizardPageContainer1)).BeginInit();
			this.wizardPageContainer1.SuspendLayout();
			this.wizardPage2.SuspendLayout();
			this.wizardPage3.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// backBtn
			// 
			this.backBtn.FlatAppearance.BorderSize = 0;
			this.backBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.backBtn.Font = new System.Drawing.Font("Segoe UI Symbol", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.backBtn.ForeColor = System.Drawing.Color.White;
			this.backBtn.Location = new System.Drawing.Point(3, 3);
			this.backBtn.Name = "backBtn";
			this.backBtn.Size = new System.Drawing.Size(47, 43);
			this.backBtn.TabIndex = 1;
			this.backBtn.Tag = AeroWizard.WizardCommandButtonState.Disabled;
			this.backBtn.Text = "º";
			this.backBtn.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.backBtn.UseVisualStyleBackColor = false;
			// 
			// cancelBtn
			// 
			this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelBtn.AutoSize = true;
			this.cancelBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.cancelBtn.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.cancelBtn.FlatAppearance.BorderSize = 2;
			this.cancelBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(116)))), ((int)(((byte)(188)))));
			this.cancelBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(57)))), ((int)(((byte)(92)))));
			this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cancelBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancelBtn.ForeColor = System.Drawing.Color.White;
			this.cancelBtn.Location = new System.Drawing.Point(661, 496);
			this.cancelBtn.MinimumSize = new System.Drawing.Size(90, 0);
			this.cancelBtn.Name = "cancelBtn";
			this.cancelBtn.Size = new System.Drawing.Size(90, 34);
			this.cancelBtn.TabIndex = 1;
			this.cancelBtn.Tag = AeroWizard.WizardCommandButtonState.Disabled;
			this.cancelBtn.Text = "&Cancel";
			this.cancelBtn.UseVisualStyleBackColor = false;
			this.cancelBtn.Click += new System.EventHandler(this.button1_Click);
			// 
			// nextBtn
			// 
			this.nextBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.nextBtn.AutoSize = true;
			this.nextBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.nextBtn.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.nextBtn.FlatAppearance.BorderSize = 2;
			this.nextBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(116)))), ((int)(((byte)(188)))));
			this.nextBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(57)))), ((int)(((byte)(92)))));
			this.nextBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.nextBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nextBtn.ForeColor = System.Drawing.Color.White;
			this.nextBtn.Location = new System.Drawing.Point(556, 496);
			this.nextBtn.MinimumSize = new System.Drawing.Size(90, 0);
			this.nextBtn.Name = "nextBtn";
			this.nextBtn.Size = new System.Drawing.Size(90, 34);
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
			this.wizardPageContainer1.BackButtonText = "º";
			this.wizardPageContainer1.CancelButton = this.cancelBtn;
			this.wizardPageContainer1.Controls.Add(this.wizardPage2);
			this.wizardPageContainer1.Controls.Add(this.wizardPage3);
			this.wizardPageContainer1.Location = new System.Drawing.Point(12, 93);
			this.wizardPageContainer1.Name = "wizardPageContainer1";
			this.wizardPageContainer1.NextButton = this.nextBtn;
			this.wizardPageContainer1.Pages.Add(this.wizardPage2);
			this.wizardPageContainer1.Pages.Add(this.wizardPage3);
			this.wizardPageContainer1.ShowProgressInTaskbarIcon = true;
			this.wizardPageContainer1.Size = new System.Drawing.Size(739, 399);
			this.wizardPageContainer1.TabIndex = 2;
			this.wizardPageContainer1.Finished += new System.EventHandler(this.wizardPageContainer1_Finished);
			this.wizardPageContainer1.SelectedPageChanged += new System.EventHandler(this.wizardPageContainer1_SelectedPageChanged);
			// 
			// wizardPage2
			// 
			this.wizardPage2.AllowNext = false;
			this.wizardPage2.Controls.Add(this.checkBox1);
			this.wizardPage2.Name = "wizardPage2";
			this.wizardPage2.Size = new System.Drawing.Size(739, 399);
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
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(56, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(154, 37);
			this.label4.TabIndex = 3;
			this.label4.Text = "<Heading>";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.BackColor = System.Drawing.Color.Crimson;
			this.button1.FlatAppearance.BorderSize = 0;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.ForeColor = System.Drawing.Color.White;
			this.button1.Location = new System.Drawing.Point(736, -1);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(27, 23);
			this.button1.TabIndex = 5;
			this.button1.Text = "X";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.backBtn);
			this.flowLayoutPanel1.Controls.Add(this.label4);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(13, 13);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(213, 49);
			this.flowLayoutPanel1.TabIndex = 6;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Font = new System.Drawing.Font("Webdings", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4});
			this.contextMenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.ShowImageMargin = false;
			this.contextMenuStrip1.Size = new System.Drawing.Size(69, 100);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(68, 24);
			this.toolStripMenuItem1.Text = "9";
			this.toolStripMenuItem1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(68, 24);
			this.toolStripMenuItem2.Text = "3";
			this.toolStripMenuItem2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(68, 24);
			this.toolStripMenuItem3.Text = "4";
			this.toolStripMenuItem3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(68, 24);
			this.toolStripMenuItem4.Text = ":";
			this.toolStripMenuItem4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// Win10Wiz
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(81)))), ((int)(((byte)(131)))));
			this.ClientSize = new System.Drawing.Size(763, 542);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.wizardPageContainer1);
			this.Controls.Add(this.cancelBtn);
			this.Controls.Add(this.nextBtn);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Win10Wiz";
			this.Text = "TestWizBase";
			((System.ComponentModel.ISupportInitialize)(this.wizardPageContainer1)).EndInit();
			this.wizardPageContainer1.ResumeLayout(false);
			this.wizardPage2.ResumeLayout(false);
			this.wizardPage2.PerformLayout();
			this.wizardPage3.ResumeLayout(false);
			this.wizardPage3.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
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
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
	}
}