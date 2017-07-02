namespace TestWizard
{
	partial class Main
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
			this.appRenderVS = new System.Windows.Forms.CheckBox();
			this.compEnabledCheck = new System.Windows.Forms.CheckBox();
			this.vsEnabledByUser = new System.Windows.Forms.CheckBox();
			this.vsOnOS = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.customBtn = new System.Windows.Forms.Button();
			this.stepBtn = new System.Windows.Forms.Button();
			this.wizBtn = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.oldButton = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// appRenderVS
			// 
			this.appRenderVS.AutoCheck = false;
			this.appRenderVS.AutoSize = true;
			this.appRenderVS.Location = new System.Drawing.Point(6, 42);
			this.appRenderVS.Name = "appRenderVS";
			this.appRenderVS.Size = new System.Drawing.Size(94, 17);
			this.appRenderVS.TabIndex = 0;
			this.appRenderVS.Text = "AppRenderVS";
			this.toolTip1.SetToolTip(this.appRenderVS, "Indicates if Visual Styles are enabled for this application.");
			this.appRenderVS.UseVisualStyleBackColor = true;
			// 
			// compEnabledCheck
			// 
			this.compEnabledCheck.AutoSize = true;
			this.compEnabledCheck.Location = new System.Drawing.Point(6, 19);
			this.compEnabledCheck.Name = "compEnabledCheck";
			this.compEnabledCheck.Size = new System.Drawing.Size(125, 17);
			this.compEnabledCheck.TabIndex = 0;
			this.compEnabledCheck.Text = "Composition Enabled";
			this.toolTip1.SetToolTip(this.compEnabledCheck, "Indicates if Desktop Window Composition (Aero) is enabled. For Windows Vista and " +
        "Windows 7, this setting is user configurable.");
			this.compEnabledCheck.UseVisualStyleBackColor = true;
			this.compEnabledCheck.CheckedChanged += new System.EventHandler(this.compEnabledCheck_CheckedChanged);
			// 
			// vsEnabledByUser
			// 
			this.vsEnabledByUser.AutoCheck = false;
			this.vsEnabledByUser.AutoSize = true;
			this.vsEnabledByUser.Location = new System.Drawing.Point(149, 19);
			this.vsEnabledByUser.Name = "vsEnabledByUser";
			this.vsEnabledByUser.Size = new System.Drawing.Size(113, 17);
			this.vsEnabledByUser.TabIndex = 0;
			this.vsEnabledByUser.Text = "VSEnabledByUser";
			this.toolTip1.SetToolTip(this.vsEnabledByUser, "Indicates if Visual Styles are enabled by the user.");
			this.vsEnabledByUser.UseVisualStyleBackColor = true;
			// 
			// vsOnOS
			// 
			this.vsOnOS.AutoCheck = false;
			this.vsOnOS.AutoSize = true;
			this.vsOnOS.Location = new System.Drawing.Point(149, 42);
			this.vsOnOS.Name = "vsOnOS";
			this.vsOnOS.Size = new System.Drawing.Size(67, 17);
			this.vsOnOS.TabIndex = 0;
			this.vsOnOS.Text = "VSonOS";
			this.toolTip1.SetToolTip(this.vsOnOS, "Indicates if Visual Styles are enabled by the Operating System.");
			this.vsOnOS.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.compEnabledCheck);
			this.groupBox1.Controls.Add(this.appRenderVS);
			this.groupBox1.Controls.Add(this.vsOnOS);
			this.groupBox1.Controls.Add(this.vsEnabledByUser);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(268, 67);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Environment";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.oldButton);
			this.groupBox2.Controls.Add(this.customBtn);
			this.groupBox2.Controls.Add(this.stepBtn);
			this.groupBox2.Controls.Add(this.wizBtn);
			this.groupBox2.Location = new System.Drawing.Point(12, 86);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(268, 82);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Wizard";
			// 
			// customBtn
			// 
			this.customBtn.Location = new System.Drawing.Point(7, 49);
			this.customBtn.Name = "customBtn";
			this.customBtn.Size = new System.Drawing.Size(107, 23);
			this.customBtn.TabIndex = 0;
			this.customBtn.Text = "Custom Wizard";
			this.toolTip1.SetToolTip(this.customBtn, "Launches a custom wizard.");
			this.customBtn.UseVisualStyleBackColor = true;
			this.customBtn.Click += new System.EventHandler(this.customBtn_Click);
			// 
			// stepBtn
			// 
			this.stepBtn.Location = new System.Drawing.Point(149, 20);
			this.stepBtn.Name = "stepBtn";
			this.stepBtn.Size = new System.Drawing.Size(107, 23);
			this.stepBtn.TabIndex = 0;
			this.stepBtn.Text = "Step Wizard";
			this.toolTip1.SetToolTip(this.stepBtn, "Lauches a modification of the Aero Wizard that includes a checked step list on th" +
        "e left.");
			this.stepBtn.UseVisualStyleBackColor = true;
			this.stepBtn.Click += new System.EventHandler(this.stepBtn_Click);
			// 
			// wizBtn
			// 
			this.wizBtn.Location = new System.Drawing.Point(7, 20);
			this.wizBtn.Name = "wizBtn";
			this.wizBtn.Size = new System.Drawing.Size(107, 23);
			this.wizBtn.TabIndex = 0;
			this.wizBtn.Text = "Aero Wizard";
			this.toolTip1.SetToolTip(this.wizBtn, "Launches a wizard using the Aero Wizard styling defined for Operating Systems aft" +
        "er Windows Vista.");
			this.wizBtn.UseVisualStyleBackColor = true;
			this.wizBtn.Click += new System.EventHandler(this.wizBtn_Click);
			// 
			// oldButton
			// 
			this.oldButton.Location = new System.Drawing.Point(149, 49);
			this.oldButton.Name = "oldButton";
			this.oldButton.Size = new System.Drawing.Size(107, 23);
			this.oldButton.TabIndex = 0;
			this.oldButton.Text = "Old Wizard";
			this.oldButton.UseVisualStyleBackColor = true;
			this.oldButton.Click += new System.EventHandler(this.oldButton_Click);
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 180);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "Main";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Wizard Test";
			this.Load += new System.EventHandler(this.Main_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckBox appRenderVS;
		private System.Windows.Forms.CheckBox compEnabledCheck;
		private System.Windows.Forms.CheckBox vsEnabledByUser;
		private System.Windows.Forms.CheckBox vsOnOS;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button customBtn;
		private System.Windows.Forms.Button stepBtn;
		private System.Windows.Forms.Button wizBtn;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button oldButton;

    }
}