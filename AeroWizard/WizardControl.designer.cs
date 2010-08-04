namespace AeroWizard
{
	partial class WizardControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.titleBar = new System.Windows.Forms.TableLayoutPanel();
			this.title = new AeroWizard.ThemedLabel();
			this.titleImage = new AeroWizard.ThemedLabel();
			this.titleImageList = new System.Windows.Forms.ImageList(this.components);
			this.backButton = new AeroWizard.ThemeImageButton();
			this.header = new System.Windows.Forms.TableLayoutPanel();
			this.headerLabel = new System.Windows.Forms.Label();
			this.commandArea = new System.Windows.Forms.TableLayoutPanel();
			this.cancelButton = new System.Windows.Forms.Button();
			this.nextButton = new System.Windows.Forms.Button();
			this.contentArea = new System.Windows.Forms.TableLayoutPanel();
			this.commandAreaBorder = new System.Windows.Forms.Panel();
			this.titleBar.SuspendLayout();
			this.header.SuspendLayout();
			this.commandArea.SuspendLayout();
			this.SuspendLayout();
			// 
			// titleBar
			// 
			this.titleBar.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.titleBar.ColumnCount = 3;
			this.titleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
			this.titleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.titleBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.titleBar.Controls.Add(this.title, 2, 0);
			this.titleBar.Controls.Add(this.titleImage, 1, 0);
			this.titleBar.Controls.Add(this.backButton, 0, 0);
			this.titleBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.titleBar.Location = new System.Drawing.Point(0, 0);
			this.titleBar.Name = "titleBar";
			this.titleBar.RowCount = 1;
			this.titleBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.titleBar.Size = new System.Drawing.Size(609, 32);
			this.titleBar.TabIndex = 0;
			this.titleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
			this.titleBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseMove);
			this.titleBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseUp);
			// 
			// title
			// 
			this.title.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.title.AutoSize = true;
			this.title.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.title.Image = null;
			this.title.Location = new System.Drawing.Point(59, 6);
			this.title.Margin = new System.Windows.Forms.Padding(0);
			this.title.Name = "title";
			this.title.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.title.Size = new System.Drawing.Size(79, 19);
			this.title.TabIndex = 2;
			this.title.Text = "Wizard Title";
			this.title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// titleImage
			// 
			this.titleImage.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.titleImage.AutoSize = true;
			this.titleImage.Image = null;
			this.titleImage.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this.titleImage.ImageList = this.titleImageList;
			this.titleImage.Location = new System.Drawing.Point(36, 8);
			this.titleImage.Margin = new System.Windows.Forms.Padding(0, 0, 7, 0);
			this.titleImage.MinimumSize = new System.Drawing.Size(16, 16);
			this.titleImage.Name = "titleImage";
			this.titleImage.Size = new System.Drawing.Size(16, 16);
			this.titleImage.TabIndex = 1;
			// 
			// titleImageList
			// 
			this.titleImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.titleImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.titleImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// backButton
			// 
			this.backButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.backButton.Enabled = false;
			this.backButton.Location = new System.Drawing.Point(0, 0);
			this.backButton.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this.backButton.Name = "backButton";
			this.backButton.Size = new System.Drawing.Size(30, 30);
			this.backButton.StyleClass = "NAVIGATION";
			this.backButton.TabIndex = 0;
			this.backButton.UseVisualStyleBackColor = true;
			this.backButton.Click += new System.EventHandler(this.backButton_Click);
			// 
			// header
			// 
			this.header.AutoSize = true;
			this.header.ColumnCount = 1;
			this.header.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.header.Controls.Add(this.headerLabel, 0, 0);
			this.header.Dock = System.Windows.Forms.DockStyle.Top;
			this.header.Location = new System.Drawing.Point(0, 32);
			this.header.Name = "header";
			this.header.RowCount = 1;
			this.header.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.header.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
			this.header.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
			this.header.Size = new System.Drawing.Size(609, 59);
			this.header.TabIndex = 0;
			// 
			// headerLabel
			// 
			this.headerLabel.AutoSize = true;
			this.headerLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.headerLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(112)))), ((int)(((byte)(171)))));
			this.headerLabel.Location = new System.Drawing.Point(38, 19);
			this.headerLabel.Margin = new System.Windows.Forms.Padding(38, 19, 0, 19);
			this.headerLabel.Name = "headerLabel";
			this.headerLabel.Size = new System.Drawing.Size(77, 21);
			this.headerLabel.TabIndex = 0;
			this.headerLabel.Text = "Page Title";
			// 
			// commandArea
			// 
			this.commandArea.AutoSize = true;
			this.commandArea.BackColor = System.Drawing.SystemColors.Control;
			this.commandArea.ColumnCount = 3;
			this.commandArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.commandArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.commandArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
			this.commandArea.Controls.Add(this.cancelButton, 1, 1);
			this.commandArea.Controls.Add(this.nextButton, 0, 1);
			this.commandArea.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.commandArea.Location = new System.Drawing.Point(0, 371);
			this.commandArea.Name = "commandArea";
			this.commandArea.RowCount = 3;
			this.commandArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.commandArea.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.commandArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.commandArea.Size = new System.Drawing.Size(609, 43);
			this.commandArea.TabIndex = 2;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.Location = new System.Drawing.Point(520, 10);
			this.cancelButton.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(70, 23);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// nextButton
			// 
			this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.nextButton.Location = new System.Drawing.Point(443, 10);
			this.nextButton.Margin = new System.Windows.Forms.Padding(0);
			this.nextButton.Name = "nextButton";
			this.nextButton.Size = new System.Drawing.Size(70, 23);
			this.nextButton.TabIndex = 0;
			this.nextButton.Text = "&Next >";
			this.nextButton.UseVisualStyleBackColor = true;
			this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
			// 
			// contentArea
			// 
			this.contentArea.ColumnCount = 3;
			this.contentArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 38F));
			this.contentArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.contentArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
			this.contentArea.Dock = System.Windows.Forms.DockStyle.Fill;
			this.contentArea.Location = new System.Drawing.Point(0, 91);
			this.contentArea.Name = "contentArea";
			this.contentArea.RowCount = 2;
			this.contentArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.contentArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
			this.contentArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.contentArea.Size = new System.Drawing.Size(609, 280);
			this.contentArea.TabIndex = 1;
			this.contentArea.Paint += new System.Windows.Forms.PaintEventHandler(this.contentArea_Paint);
			// 
			// commandAreaBorder
			// 
			this.commandAreaBorder.BackColor = System.Drawing.SystemColors.ControlLight;
			this.commandAreaBorder.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.commandAreaBorder.Location = new System.Drawing.Point(0, 370);
			this.commandAreaBorder.Margin = new System.Windows.Forms.Padding(0);
			this.commandAreaBorder.Name = "commandAreaBorder";
			this.commandAreaBorder.Size = new System.Drawing.Size(609, 1);
			this.commandAreaBorder.TabIndex = 2;
			// 
			// WizardControl
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.commandAreaBorder);
			this.Controls.Add(this.contentArea);
			this.Controls.Add(this.commandArea);
			this.Controls.Add(this.header);
			this.Controls.Add(this.titleBar);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "WizardControl";
			this.Size = new System.Drawing.Size(609, 414);
			this.titleBar.ResumeLayout(false);
			this.titleBar.PerformLayout();
			this.header.ResumeLayout(false);
			this.header.PerformLayout();
			this.commandArea.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel titleBar;
		private System.Windows.Forms.TableLayoutPanel header;
		internal System.Windows.Forms.Label headerLabel;
		private System.Windows.Forms.TableLayoutPanel commandArea;
		private System.Windows.Forms.Button cancelButton;
		internal System.Windows.Forms.Button nextButton;
		private AeroWizard.ThemedLabel title;
		private AeroWizard.ThemedLabel titleImage;
		internal AeroWizard.ThemeImageButton backButton;
		private System.Windows.Forms.TableLayoutPanel contentArea;
		private System.Windows.Forms.Panel commandAreaBorder;
		private System.Windows.Forms.ImageList titleImageList;

	}
}
