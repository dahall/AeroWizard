using AeroWizard.VisualStyles;
using Microsoft.Win32.DesktopWindowManager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace AeroWizard
{
	/// <summary>
	/// Styles that can be applied to the body of a <see cref="WizardControl"/> when on XP or earlier or when a Basic theme is applied.
	/// </summary>
	public enum WizardClassicStyle
	{
		/// <summary>Windows Vista style theme with large fonts and white background.</summary>
		AeroStyle,
		/// <summary>Windows XP style theme with control color background.</summary>
		BasicStyle,
		/// <summary>Use <see cref="BasicStyle"/> on Windows XP and <see cref="AeroStyle"/> for later versions.</summary>
		Automatic
	}

	/// <summary>
	/// Control providing an "Aero Wizard" style interface.
	/// </summary>
	[Designer(typeof(Design.WizardControlDesigner))]
	[ToolboxItem(true), ToolboxBitmap(typeof(WizardControl), "WizardControl.bmp")]
	[Description("Creates an Aero Wizard interface.")]
	[DefaultProperty("Pages"), DefaultEvent("SelectedPageChanged")]
	public partial class WizardControl :
#if DEBUG
		UserControl
#else
		Control
#endif
		, ISupportInitialize
	{
		private static bool isMin6 = System.Environment.OSVersion.Version.Major >= 6;

		private WizardClassicStyle classicStyle = WizardClassicStyle.AeroStyle;
		private bool themePropsSet = false;
		private Point formMoveLastMousePos;
		private bool formMoveTracking;
		private Form parentForm;
		private Icon titleImageIcon;
		private bool titleImageIconSet = false;

		internal int contentCol = 1;

		/// <summary>
		/// Initializes a new instance of the <see cref="WizardControl"/> class.
		/// </summary>
		public WizardControl()
		{
			InitializeComponent();

			OnRightToLeftChanged(EventArgs.Empty);

			// Get localized defaults for button text
			ResetBackButtonToolTipText();
			ResetTitle();
			ResetTitleIcon();

			// Connect to page add and remove events to track property changes
			this.Pages.ItemAdded += Pages_ItemAdded;
			this.Pages.ItemDeleted += Pages_ItemDeleted;
		}

		/// <summary>
		/// Occurs when the user clicks the Cancel button and allows for programmatic cancellation.
		/// </summary>
		[Category("Behavior"), Description("Occurs when the user clicks the Cancel button and allows for programmatic cancellation.")]
		public event CancelEventHandler Cancelling;

		/// <summary>
		/// Occurs when the user clicks the Next/Finish button and the page is set to <see cref="WizardPage.IsFinishPage"/> or this is the last page in the <see cref="Pages"/> collection.
		/// </summary>
		[Category("Behavior"), Description("Occurs when the user clicks the Next/Finish button on last page.")]
		public event EventHandler Finished;

		/// <summary>
		/// Occurs when the <see cref="WizardControl.SelectedPage"/> property has changed.
		/// </summary>
		[Category("Property Changed"), Description("Occurs when the SelectedPage property has changed.")]
		public event EventHandler SelectedPageChanged;

		/// <summary>
		/// Gets or sets the state of the back button.
		/// </summary>
		/// <value>The state of the back button.</value>
		[Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WizardCommandButtonState BackButtonState
		{
			get { return pageContainer.BackButtonState; }
		}

		/// <summary>
		/// Gets or sets the back button tool tip text.
		/// </summary>
		/// <value>The back button tool tip text.</value>
		[Category("Wizard"), Localizable(true), Description("The back button tool tip text")]
		public string BackButtonToolTipText
		{
			get { return backButton.ToolTipText; }
			set { backButton.ToolTipText = value; base.Invalidate(); }
		}

		/// <summary>
		/// Gets the state of the cancel button.
		/// </summary>
		/// <value>The state of the cancel button.</value>
		[Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WizardCommandButtonState CancelButtonState
		{
			get { return pageContainer.CancelButtonState; }
		}
		
		/// <summary>
		/// Gets or sets the cancel button text.
		/// </summary>
		/// <value>The cancel button text.</value>
		[Category("Wizard"), Localizable(true), Description("The cancel button text")]
		public string CancelButtonText
		{
			get { return this.pageContainer.CancelButtonText; }
			set { this.pageContainer.CancelButtonText = value; Refresh(); }
		}

		/// <summary>
		/// Gets or sets the style applied to the body of a <see cref="WizardControl"/> when on XP or earlier or when a Basic theme is applied.
		/// </summary>
		/// <value>A <see cref="WizardClassicStyle"/> value which determines the style.</value>
		[Category("Wizard"), DefaultValue(typeof(WizardClassicStyle), "AeroStyle"), Description("The style used in Windows Classic mode or on Windows XP")]
		public WizardClassicStyle ClassicStyle
		{
			get { return classicStyle; }
			set { classicStyle = value; ConfigureStyles(); base.Invalidate(); }
		}

		/// <summary>
		/// Gets or sets the finish button text.
		/// </summary>
		/// <value>The finish button text.</value>
		[Category("Wizard"), Localizable(true), Description("The finish button text")]
		public string FinishButtonText
		{
			get { return this.pageContainer.FinishButtonText; }
			set { this.pageContainer.FinishButtonText = value; Refresh(); }
		}

		/// <summary>
		/// Gets or sets the page header text.
		/// </summary>
		/// <value>The page header text.</value>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string HeaderText
		{
			get { return headerLabel.Text; }
			set { headerLabel.Text = value; Refresh(); }
		}

		/// <summary>
		/// Gets or sets the shield icon on the next button.
		/// </summary>
		/// <value><c>true</c> if Next button should display a shield; otherwise, <c>false</c>.</value>
		/// <exception cref="PlatformNotSupportedException">Setting a UAF shield on a button only works on Vista and later versions of Windows.</exception>
		[DefaultValue(false), Category("Wizard"), Description("Show a shield icon on the next button")]
		public Boolean NextButtonShieldEnabled
		{
			get { return this.pageContainer.NextButtonShieldEnabled; }
			set { this.pageContainer.NextButtonShieldEnabled = value; }
		}

		/// <summary>
		/// Gets the state of the next button.
		/// </summary>
		/// <value>The state of the next button.</value>
		[Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WizardCommandButtonState NextButtonState
		{
			get { return pageContainer.NextButtonState; }
		}

		/// <summary>
		/// Gets or sets the next button text.
		/// </summary>
		/// <value>The next button text.</value>
		[Category("Wizard"), Localizable(true), Description("The next button text.")]
		public string NextButtonText
		{
			get { return this.pageContainer.NextButtonText; }
			set { this.pageContainer.NextButtonText = value; Refresh(); }
		}

		/// <summary>
		/// Gets the collection of wizard pages in this wizard control.
		/// </summary>
		/// <value>The <see cref="WizardPageCollection"/> that contains the <see cref="WizardPage"/> objects in this <see cref="WizardControl"/>.</value>
		[Category("Wizard"), Description("Collection of wizard pages.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public WizardPageCollection Pages
		{
			get { return this.pageContainer.Pages; }
		}

		/// <summary>
		/// Gets how far the wizard has progressed, as a percentage.
		/// </summary>
		/// <value>A value between 0 and 100.</value>
		[Browsable(false), Description("The percentage of the current page against all pages at run-time.")]
		public ushort PercentComplete
		{
			get { return this.pageContainer.PercentComplete; }
		}

		/// <summary>
		/// Gets the currently selected wizard page.
		/// </summary>
		/// <value>The selected wizard page. <c>null</c> if no page is active.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual WizardPage SelectedPage
		{
			get { return this.pageContainer.SelectedPage; }
			internal set { this.pageContainer.SelectedPage = value; if (value != null) this.HeaderText = value.Text; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show progress in form's taskbar icon.
		/// </summary>
		/// <remarks>
		/// This will only work on Windows 7 or later and the parent form must be showing its icon in the taskbar. No exception is thrown on failure.
		/// </remarks>
		/// <value>
		/// <c>true</c> to show progress in taskbar icon; otherwise, <c>false</c>.
		/// </value>
		[Category("Wizard"), DefaultValue(false), Description("Indicates whether to show progress in form's taskbar icon")]
		public bool ShowProgressInTaskbarIcon
		{
			get { return this.pageContainer.ShowProgressInTaskbarIcon; }
			set { this.pageContainer.ShowProgressInTaskbarIcon = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to suppress changing the parent form's icon to match the wizard's <see cref="TitleIcon"/>.
		/// </summary>
		/// <value><c>true</c> to not change the parent form's icon to match this wizard's icon; otherwise, <c>false</c>.</value>
		[Category("Wizard"), DefaultValue(false), Description("Indicates whether to suppress changing the parent form's icon to match the wizard's")]
		public bool SuppressParentFormIconSync { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to suppress changing the parent form's caption to match the wizard's <see cref="Title"/>.
		/// </summary>
		/// <value><c>true</c> to not change the parent form's caption (Text) to match this wizard's title; otherwise, <c>false</c>.</value>
		[Category("Wizard"), DefaultValue(false), Description("Indicates whether to suppress changing the parent form's caption to match the wizard's")]
		public bool SuppressParentFormCaptionSync { get; set; }

		/// <summary>
		/// Gets or sets the title for the wizard.
		/// </summary>
		/// <value>The title text.</value>
		[Category("Wizard"), Localizable(true), Description("Title for the wizard")]
		public string Title
		{
			get { return title.Text; }
			set { title.Text = value; base.Invalidate(); }
		}

		/// <summary>
		/// Gets or sets the optionally displayed icon next to the wizard title.
		/// </summary>
		/// <value>The title icon.</value>
		[Category("Wizard"), Localizable(true), Description("Icon next to the wizard title")]
		public Icon TitleIcon
		{
			get { return titleImageIcon; }
			set
			{
				titleImageIcon = value;
				titleImageList.Images.Clear();
				if (titleImageIcon != null)
				{
					// Resolve for different DPI settings and ensure that if icon is not a standard size, such as 20x20, 
					// that the larger one (24x24) is downsized and not the smaller up-sized. (thanks demidov)
					titleImage.Size = titleImageList.ImageSize = SystemInformation.SmallIconSize;
					titleImageList.Images.Add(new Icon(value, SystemInformation.SmallIconSize + new Size(1, 1)));
					titleImage.ImageIndex = 0;
				}
				titleImageIconSet = true;
				base.Invalidate();
			}
		}

		internal int SelectedPageIndex
		{
			get { return this.pageContainer.SelectedPageIndex; }
		}

		private bool UseAeroStyle
		{
			get { return classicStyle == WizardClassicStyle.AeroStyle || (classicStyle == WizardClassicStyle.Automatic && DesktopWindowManager.CompositionSupported && Application.RenderWithVisualStyles); }
		}

		/// <summary>
		/// Adds a new control to the command bar.
		/// </summary>
		/// <remarks>This will cause your wizard to deviate from the Windows UI guidelines. All controls will display right to left in the order added and will cause
		/// the command bar to remain visible as long as the control is visible. The developer must fully manage the state of this added control.</remarks>
		/// <param name="ctrl">The control to add.</param>
		public void AddCommandControl(Control ctrl)
		{
			commandAreaButtonFlowLayout.Controls.Add(ctrl);
		}

		/// <summary>
		/// Signals the object that initialization is starting.
		/// </summary>
		public void BeginInit()
		{
			pageContainer.BeginInit();
		}

		/// <summary>
		/// Signals the object that initialization is complete.
		/// </summary>
		public void EndInit()
		{
			pageContainer.EndInit();
		}

		/// <summary>
		/// Advances to the next page in the sequence.
		/// </summary>
		public void NextPage()
		{
			NextPage(null);
		}

		/// <summary>
		/// Advances to the specified page.
		/// </summary>
		/// <param name="nextPage">The wizard page to go to next.</param>
		/// <param name="skipCommit">if set to <c>true</c> skip <see cref="WizardPage.Commit"/> event.</param>
		/// <exception cref="ArgumentException">When specifying a value for nextPage, it must already be in the Pages collection.</exception>
		public virtual void NextPage(WizardPage nextPage, bool skipCommit = false)
		{
			this.pageContainer.NextPage(nextPage, skipCommit);
		}

		/// <summary>
		/// Overrides the theme fonts provided by the system.
		/// </summary>
		/// <remarks>This is NOT recommended as it will cause the wizard to not match those provided by the system. This should be called only after the handle has been created or it will be overridden with the system theme values.</remarks>
		/// <param name="titleFont">The title font.</param>
		/// <param name="headerFont">The header font.</param>
		/// <param name="buttonFont">The command buttons font.</param>
		public virtual void OverrideThemeFonts(Font titleFont, Font headerFont, Font buttonFont)
		{
			title.Font = titleFont;
			headerLabel.Font = headerFont;
			foreach (Control ctrl in commandAreaButtonFlowLayout.Controls)
			{
				ctrl.Font = buttonFont;
			}
		}

		/// <summary>
		/// Returns to the previous page.
		/// </summary>
		public virtual void PreviousPage()
		{
			this.pageContainer.PreviousPage();
		}

		/// <summary>
		/// Restarts the wizard pages from the first page.
		/// </summary>
		public void RestartPages()
		{
			this.pageContainer.RestartPages();
		}

		/// <summary>
		/// Gets the unthemed back button image.
		/// </summary>
		/// <returns><see cref="Bitmap"/> with the four state images stacked on top of each other.</returns>
		protected virtual Bitmap GetUnthemedBackButtonImage()
		{
			if (System.Environment.OSVersion.Version >= new Version(6, 2))
				return Properties.Resources.BackBtnStrip2;
			else
				return Properties.Resources.BackBtnStrip;
		}

		/// <summary>
		/// Raises the <see cref="WizardControl.Cancelling"/> event.
		/// </summary>
		protected virtual void OnCancelling()
		{
			CancelEventHandler h = Cancelling;
			CancelEventArgs arg = new CancelEventArgs(true);
			if (h != null)
				h(this, arg);

			if (arg.Cancel)
			{
				if (!this.IsDesignMode())
					CloseForm(DialogResult.Cancel);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.ControlAdded"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ControlEventArgs"/> that contains the event data.</param>
		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			if (e.Control is WizardPage)
			{
				Controls.Remove(e.Control);
				Pages.Add(e.Control as WizardPage);
			}
		}

		/// <summary>
		/// Raises the <see cref="WizardControl.Finished"/> event.
		/// </summary>
		protected virtual void OnFinished()
		{
			EventHandler h = Finished;
			if (h != null)
				h(this, EventArgs.Empty);

			if (!this.IsDesignMode())
				CloseForm(DialogResult.OK);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			pageContainer.Focus();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnHandleCreated(EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("OnHandleCreated");
			base.OnHandleCreated(e);
			this.SetLayout();
			this.AddSystemEvents();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnHandleDestroyed(EventArgs e)
		{
			this.RemoveSystemEvents();
			base.OnHandleDestroyed(e);
		}

		private void AddSystemEvents()
		{
			if (!this.IsDesignMode())
			{
				if (DesktopWindowManager.CompositionSupported)
				{
					DesktopWindowManager.ColorizationColorChanged += DisplyColorOrCompositionChanged;
					DesktopWindowManager.CompositionChanged += DisplyColorOrCompositionChanged;
				}
				Microsoft.Win32.SystemEvents.DisplaySettingsChanged += DisplyColorOrCompositionChanged;
				this.SystemColorsChanged += DisplyColorOrCompositionChanged;
			}
		}

		private void RemoveSystemEvents()
		{
			if (!this.IsDesignMode())
			{
				if (DesktopWindowManager.CompositionSupported)
				{
					DesktopWindowManager.CompositionChanged -= DisplyColorOrCompositionChanged;
					DesktopWindowManager.ColorizationColorChanged -= DisplyColorOrCompositionChanged;
				}
				Microsoft.Win32.SystemEvents.DisplaySettingsChanged -= DisplyColorOrCompositionChanged;
				this.SystemColorsChanged -= DisplyColorOrCompositionChanged;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			if (parentForm != null)
				parentForm.Load -= parentForm_Load;
			parentForm = base.Parent as Form;
			this.Dock = DockStyle.Fill;
			if (parentForm != null)
				parentForm.Load += parentForm_Load;
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.RightToLeftChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			bool r2l = (this.GetRightToLeftProperty() == System.Windows.Forms.RightToLeft.Yes);
			Bitmap btnStrip = GetUnthemedBackButtonImage();
			if (r2l) btnStrip.RotateFlip(RotateFlipType.RotateNoneFlipX);
			backButton.SetImageListImageStrip(btnStrip, Orientation.Vertical);
			backButton.StylePart = r2l ? 2 : 1;
		}

		/// <summary>
		/// Raises the <see cref="WizardControl.SelectedPageChanged"/> event.
		/// </summary>
		protected void OnSelectedPageChanged()
		{
			EventHandler temp = SelectedPageChanged;
			if (temp != null)
				temp(this, EventArgs.Empty);
		}

		private void CloseForm(DialogResult dlgResult)
		{
			Form form = base.FindForm();
			if (form != null && form.Modal)
				form.DialogResult = dlgResult;
		}

		private void ConfigureStyles()
		{
			if (Application.RenderWithVisualStyles)
			{
				titleBar.SetTheme(VisualStyleElementEx.AeroWizard.TitleBar.Active);
				header.SetTheme(VisualStyleElementEx.AeroWizard.HeaderArea.Normal);
				contentArea.SetTheme(VisualStyleElementEx.AeroWizard.ContentArea.Normal);
				commandArea.SetTheme(VisualStyleElementEx.AeroWizard.CommandArea.Normal);
			}
			else
			{
				titleBar.ClearTheme();
				header.ClearTheme();
				contentArea.ClearTheme();
				commandArea.ClearTheme();
				titleBar.BackColor = System.Drawing.SystemColors.Control;
			}

			if (UseAeroStyle)
			{
				bodyPanel.BorderStyle = System.Windows.Forms.BorderStyle.None;
				header.BackColor = contentArea.BackColor = SystemColors.Window;
				if (!themePropsSet)
				{
					headerLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
					headerLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(112)))), ((int)(((byte)(171)))));
					title.Font = this.Font;
				}
			}
			else
			{
				bodyPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				header.BackColor = contentArea.BackColor = SystemColors.Control;
				headerLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
				headerLabel.ForeColor = SystemColors.ControlText;
				title.Font = new Font(this.Font, FontStyle.Bold);
			}
		}

		private void ConfigureWindowFrame()
		{
			System.Diagnostics.Debug.WriteLine(string.Format("ConfigureWindowFrame: compEnab={0},parentForm={1}", DesktopWindowManager.IsCompositionEnabled(), parentForm == null ? "null" : parentForm.Name));
			ConfigureStyles();
			if (DesktopWindowManager.IsCompositionEnabled())
			{
				titleBar.BackColor = Color.Black;
				try
				{
					if (parentForm != null)
					{
						//if (!parentForm.GetWindowAttribute<bool>(DesktopWindowManager.GetWindowAttr.NonClientRenderingEnabled))
						//	parentForm.SetWindowAttribute(DesktopWindowManager.SetWindowAttr.NonClientRenderingPolicy, DesktopWindowManager.NonClientRenderingPolicy.Enabled);
						//parentForm.ExtendFrameIntoClientArea(new Padding(0));
						// TODO: Need to figure out how to reset for Win7
						parentForm.ExtendFrameIntoClientArea(new Padding(0) { Top = titleBar.Height });
						//Microsoft.Win32.NativeMethods.SetWindowPos(this.Handle, IntPtr.Zero, this.Location.X, this.Location.Y, this.Width, this.Height, Microsoft.Win32.NativeMethods.SetWindowPosFlags.FrameChanged);
					}
				}
				catch { titleBar.BackColor = commandArea.BackColor; }
			}
			else
			{
				titleBar.BackColor = commandArea.BackColor;
			}

			if (parentForm != null)
			{
				if (!this.SuppressParentFormCaptionSync)
					parentForm.Text = this.Title;
				if (!this.SuppressParentFormIconSync && this.titleImageIcon != null)
				{
					parentForm.Icon = this.TitleIcon;
					parentForm.ShowIcon = true;
				}
				parentForm.CancelButton = this.cancelButton;
				parentForm.AcceptButton = this.nextButton;
				parentForm.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
				parentForm.SetWindowThemeAttribute(Microsoft.Win32.NativeMethods.WindowThemeNonClientAttributes.NoDrawCaption | Microsoft.Win32.NativeMethods.WindowThemeNonClientAttributes.NoDrawIcon | Microsoft.Win32.NativeMethods.WindowThemeNonClientAttributes.NoSysMenu);
				parentForm.Invalidate();
			}
		}

		private void contentArea_Paint(object sender, PaintEventArgs pe)
		{
			if (this.IsDesignMode() && this.Pages.Count == 0)
			{
				string noPagesText = Properties.Resources.WizardNoPagesNotice;
				Rectangle r = this.GetContentAreaRectangle(false);

				r.Inflate(-2, -2);
				//pe.Graphics.DrawRectangle(SystemPens.GrayText, r);
				ControlPaint.DrawFocusRectangle(pe.Graphics, r);

				SizeF textSize = pe.Graphics.MeasureString(noPagesText, this.Font);
				r.Inflate((r.Width - (int)textSize.Width) / -2, (r.Height - (int)textSize.Height) / -2);
				pe.Graphics.DrawString(noPagesText, this.Font, SystemBrushes.GrayText, r);
			}
		}

		private void DisplyColorOrCompositionChanged(object sender, EventArgs e)
		{
			SetLayout();
			ConfigureWindowFrame();
			if (parentForm != null)
				parentForm.Refresh();
		}

		/// <summary>
		/// Gets the content area rectangle.
		/// </summary>
		/// <param name="parentRelative">if set to <c>true</c> rectangle is relative to parent.</param>
		/// <returns>Coordinates of content area.</returns>
		private Rectangle GetContentAreaRectangle(bool parentRelative)
		{
			int[] cw = contentArea.GetColumnWidths();
			int[] ch = contentArea.GetRowHeights();
			Rectangle r = new Rectangle(cw[contentCol - 1], 0, cw[contentCol], ch[0]);
			if (parentRelative)
				r.Offset(contentArea.Location);
			return r;
		}

		private void pageContainer_ButtonStateChanged(object sender, EventArgs e)
		{
			bool vis = false;
			foreach (Control c in commandAreaButtonFlowLayout.Controls)
			{
				if (c.Visible || (c is ButtonBase && pageContainer.GetCmdButtonState(c as ButtonBase) != WizardCommandButtonState.Hidden))
					vis = true;
			}
			commandArea.Visible = vis;
		}

		private void pageContainer_Cancelling(object sender, CancelEventArgs e)
		{
			OnCancelling();
		}

		private void pageContainer_Finished(object sender, EventArgs e)
		{
			OnFinished();
		}

		private void pageContainer_SelectedPageChanged(object sender, EventArgs e)
		{
			if (this.pageContainer.SelectedPage != null)
				this.HeaderText = this.pageContainer.SelectedPage.Text;
			OnSelectedPageChanged();
		}

		private void Pages_ItemAdded(object sender, System.Collections.Generic.EventedList<WizardPage>.ListChangedEventArgs<WizardPage> e)
		{
			e.Item.TextChanged += Page_TextChanged;
		}

		private void Pages_ItemDeleted(object sender, System.Collections.Generic.EventedList<WizardPage>.ListChangedEventArgs<WizardPage> e)
		{
			e.Item.TextChanged -= Page_TextChanged;
		}

		private void Page_TextChanged(object sender, EventArgs e)
		{
			this.HeaderText = ((WizardPage)sender).Text;
		}

		private void parentForm_Load(object sender, EventArgs e)
		{
			ConfigureWindowFrame();
		}

		private void ResetBackButtonToolTipText()
		{
			BackButtonToolTipText = Properties.Resources.WizardBackButtonToolTip;
		}

		private void ResetBackButtonText()
		{
			pageContainer.ResetBackButtonText();
		}

		private void ResetCancelButtonText()
		{
			pageContainer.ResetCancelButtonText();
		}

		private void ResetFinishButtonText()
		{
			pageContainer.ResetFinishButtonText();
		}

		private void ResetNextButtonText()
		{
			pageContainer.ResetNextButtonText();
		}

		private void ResetTitle()
		{
			Title = Properties.Resources.WizardTitle;
		}

		private void ResetTitleIcon()
		{
			TitleIcon = Properties.Resources.WizardControlIcon;
			titleImageIconSet = false;
		}

		private void SetLayout()
		{
			if (isMin6 && Application.RenderWithVisualStyles)
			{
				VisualStyleRenderer theme;
				using (Graphics g = this.CreateGraphics())
				{
					// Back button
					theme = new VisualStyleRenderer(VisualStyleElementEx.Navigation.BackButton.Normal);
					Size bbSize = theme.GetPartSize(g, ThemeSizeType.Draw);

					// Title
					theme.SetParameters(VisualStyleElementEx.AeroWizard.TitleBar.Active);
					title.Font = theme.GetFont2(g);
					titleBar.Height = Math.Max(theme.GetMargins2(g, MarginProperty.ContentMargins).Top, bbSize.Height + 2);
					titleBar.ColumnStyles[0].Width = bbSize.Width + 4F;
					titleBar.ColumnStyles[1].Width = titleImageIcon != null ? titleImageList.ImageSize.Width + 4F : 0;
					backButton.Size = bbSize;

					// Header
					theme.SetParameters(VisualStyleElementEx.AeroWizard.HeaderArea.Normal);
					headerLabel.Font = theme.GetFont2(g);
					headerLabel.Margin = theme.GetMargins2(g, MarginProperty.ContentMargins);
					headerLabel.ForeColor = theme.GetColor(ColorProperty.TextColor);

					// Content
					theme.SetParameters(VisualStyleElementEx.AeroWizard.ContentArea.Normal);
					this.BackColor = theme.GetColor(ColorProperty.FillColor);
					contentArea.Font = theme.GetFont2(g);
					Padding cp = theme.GetMargins2(g, MarginProperty.ContentMargins);
					contentArea.ColumnStyles[0].Width = cp.Left;
					contentArea.RowStyles[1].Height = cp.Bottom;

					// Command
					theme.SetParameters(VisualStyleElementEx.AeroWizard.CommandArea.Normal);
					cp = theme.GetMargins2(g, MarginProperty.ContentMargins);
					commandArea.RowStyles[0].Height = cp.Top;
					commandArea.RowStyles[2].Height = cp.Bottom;
					commandArea.ColumnStyles[1].Width = contentArea.ColumnStyles[contentCol + 1].Width = cp.Right;
					commandAreaBorder.Height = 0;
					theme.SetParameters(VisualStyleElementEx.AeroWizard.Button.Normal);
					int btnHeight = theme.GetInteger(IntegerProperty.Height);
					commandAreaButtonFlowLayout.MinimumSize = new Size(0, btnHeight);
					Font btnFont = theme.GetFont2(g);
					foreach (Control ctrl in commandAreaButtonFlowLayout.Controls)
					{
						ctrl.Font = btnFont;
						ctrl.Height = btnHeight;
						ctrl.MaximumSize = new Size(0, btnHeight);
					}

					themePropsSet = true;
				}
			}
			else
			{
				commandAreaBorder.Height = 1;
				backButton.Size = new Size(GetUnthemedBackButtonImage().Width, GetUnthemedBackButtonImage().Height / 4);
				this.BackColor = UseAeroStyle ? SystemColors.Window : SystemColors.Control;
			}
		}

		private bool ShouldSerializeBackButtonToolTipText()
		{
			return BackButtonToolTipText != Properties.Resources.WizardBackButtonToolTip;
		}

		private bool ShouldSerializeBackButtonText()
		{
			return pageContainer.ShouldSerializeBackButtonText();
		}

		private bool ShouldSerializeCancelButtonText()
		{
			return pageContainer.ShouldSerializeCancelButtonText();
		}

		private bool ShouldSerializeFinishButtonText()
		{
			return pageContainer.ShouldSerializeFinishButtonText();
		}

		private bool ShouldSerializeNextButtonText()
		{
			return pageContainer.ShouldSerializeNextButtonText();
		}

		private bool ShouldSerializeTitle()
		{
			return Title != Properties.Resources.WizardTitle;
		}

		private bool ShouldSerializeTitleIcon()
		{
			return titleImageIconSet;
		}

		private void TitleBar_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Control c = titleBar.GetChildAtPoint(e.Location);
				if (c != backButton)
				{
					formMoveTracking = true;
					formMoveLastMousePos = this.PointToScreen(e.Location);
				}
			}

			base.OnMouseDown(e);
		}

		private void TitleBar_MouseMove(object sender, MouseEventArgs e)
		{
			if (formMoveTracking)
			{
				Point screen = this.PointToScreen(e.Location);

				Point diff = new Point(screen.X - formMoveLastMousePos.X, screen.Y - formMoveLastMousePos.Y);

				Point loc = this.parentForm.Location;
				loc.Offset(diff);
				this.parentForm.Location = loc;

				formMoveLastMousePos = screen;
			}

			base.OnMouseMove(e);
		}

		private void TitleBar_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				formMoveTracking = false;

			base.OnMouseUp(e);
		}
	}
}