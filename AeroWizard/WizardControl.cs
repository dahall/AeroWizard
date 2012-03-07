using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Microsoft.Win32.DesktopWindowManager;

namespace AeroWizard
{
	/// <summary>
	/// Button state for buttons controlling the wizard.
	/// </summary>
	public enum WizardCommandButtonState
	{
		/// <summary>Button is enabled and can be clicked.</summary>
		Enabled,
		/// <summary>Button is disabled and cannot be clicked.</summary>
		Disabled,
		/// <summary>Button is hidden from the user.</summary>
		Hidden
	}

	/// <summary>
	/// Control providing an "Aero Wizard" style interface.
	/// </summary>
	[Designer(typeof(Design.WizardControlDesigner))]
	[ToolboxItem(true), ToolboxBitmap(typeof(WizardControl), "WizardControl.bmp")]
	[Description("Creates an Aero Wizard interface.")]
	[DefaultProperty("Pages"), DefaultEvent("SelectedPageChanged")]
#if DEBUG
	public partial class WizardControl : UserControl, ISupportInitialize
#else
	public partial class WizardControl : Control, ISupportInitialize
#endif
	{
		private static bool isMin6;

		private string finishBtnText;
		private Point formMoveLastMousePos;
		private bool formMoveTracking;
		private bool initialized = false;
		private bool initializing = false;
		private bool nextButtonShieldEnabled = false;
		private string nextBtnText;
		private Stack<WizardPage> pageHistory;
		private Form parentForm;
		private WizardPage selectedPage;
		private Icon titleImageIcon;
		private bool titleImageIconSet = false;

		static WizardControl()
		{
			isMin6 = System.Environment.OSVersion.Version.Major >= 6;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WizardControl"/> class.
		/// </summary>
		public WizardControl()
		{
			pageHistory = new Stack<WizardPage>();

			Pages = new WizardPageCollection(this);
			Pages.ItemAdded += Pages_AddItem;
			Pages.ItemDeleted += Pages_RemoveItem;
			Pages.Reset += Pages_Reset;

			InitializeComponent();
			backButton.CompatibleImageStrip = Properties.Resources.BackBtnStrip;

			if (!Application.RenderWithVisualStyles)
				titleBar.BackColor = System.Drawing.SystemColors.Control;
			titleBar.SetTheme(VisualStyleElementEx.AeroWizard.TitleBar.Active);
			header.SetTheme(VisualStyleElementEx.AeroWizard.HeaderArea.Normal);
			contentArea.SetTheme(VisualStyleElementEx.AeroWizard.ContentArea.Normal);
			commandArea.SetTheme(VisualStyleElementEx.AeroWizard.CommandArea.Normal);

			// Get localized defaults for button text
			ResetBackButtonToolTipText();
			ResetCancelButtonText();
			ResetFinishButtonText();
			ResetNextButtonText();
			ResetTitle();
			ResetTitleIcon();
		}

		/// <summary>
		/// Occurs when the Cancel button has been clicked and the form is closing.
		/// </summary>
		/// <remarks>The <see cref="WizardControl.Cancelled"/> event is obsolete in version 1.2; use the <see cref="WizardControl.Cancelling"/> event instead.</remarks>
		[Obsolete("The Cancelled event is obsolete in version 1.2; use the Cancelling event instead.")]
		[Category("Behavior"), Description("Occurs when the Cancel button has been clicked and the form is closing.")]
		public event EventHandler Cancelled;

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
			get { return GetCmdButtonState(backButton); }
			internal set { SetCmdButtonState(backButton, value); }
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
			get { return GetCmdButtonState(cancelButton); }
			internal set { SetCmdButtonState(cancelButton, value); }
		}

		/// <summary>
		/// Gets or sets the cancel button text.
		/// </summary>
		/// <value>The cancel button text.</value>
		[Category("Wizard"), Localizable(true), Description("The cancel button text")]
		public string CancelButtonText
		{
			get { return cancelButton.Text; }
			set { cancelButton.Text = value; base.Invalidate(); }
		}

		/// <summary>
		/// Gets or sets the finish button text.
		/// </summary>
		/// <value>The finish button text.</value>
		[Category("Wizard"), Localizable(true), Description("The finish button text")]
		public string FinishButtonText
		{
			get { return finishBtnText; }
			set
			{
				finishBtnText = value;
				if (selectedPage != null && selectedPage.IsFinishPage && !this.IsDesignMode())
				{
					nextButton.Text = value;
					nextButton.Invalidate();
				}
			}
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
			set { headerLabel.Text = value; base.Invalidate(); }
		}

		[System.Runtime.InteropServices.DllImport("user32")]
		private static extern UInt32 SendMessage(IntPtr hWnd, UInt32 msg, UInt32 wParam, UInt32 lParam);

		/// <summary>
		/// Gets or sets the shield icon on the next button.
		/// </summary>
		/// <value><c>true</c> if Next button should display a shield; otherwise, <c>false</c>.</value>
		/// <exception cref="PlatformNotSupportedException">Setting a UAF shield on a button only works on Vista and later versions of Windows.</exception>
		[DefaultValue(false), Category("Wizard"), Description("Show a shield icon on the next button")]
		public Boolean NextButtonShieldEnabled
		{
			get { return nextButtonShieldEnabled; }
			set
			{
				if (System.Environment.OSVersion.Version.Major >= 6)
				{
					const int BCM_FIRST = 0x1600;                      //Normal button
					const int BCM_SETSHIELD = (BCM_FIRST + 0x000C);    //Elevated butto

					nextButtonShieldEnabled = value;

					if (value)
					{
						nextButton.FlatStyle = FlatStyle.System;
						SendMessage(nextButton.Handle, BCM_SETSHIELD, 0, 0xFFFFFFFF);
					}
					else
					{
						nextButton.FlatStyle = FlatStyle.Standard;
						SendMessage(nextButton.Handle, BCM_FIRST, 0, 0xFFFFFFFF);
					}

					nextButton.Invalidate();
				}
				else
					throw new PlatformNotSupportedException();
			}
		}

		/// <summary>
		/// Gets the state of the next button.
		/// </summary>
		/// <value>The state of the next button.</value>
		[Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WizardCommandButtonState NextButtonState
		{
			get { return GetCmdButtonState(nextButton); }
			internal set { SetCmdButtonState(nextButton, value); }
		}

		/// <summary>
		/// Gets or sets the next button text.
		/// </summary>
		/// <value>The next button text.</value>
		[Category("Wizard"), Localizable(true), Description("The next button text.")]
		public string NextButtonText
		{
			get { return nextBtnText; }
			set
			{
				nextBtnText = value;
				if (!this.IsDesignMode() && (selectedPage == null || !selectedPage.IsFinishPage))
				{
					nextButton.Text = value;
					nextButton.Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets the collection of wizard pages in this wizard control.
		/// </summary>
		/// <value>The <see cref="WizardPageCollection"/> that contains the <see cref="WizardPage"/> objects in this <see cref="WizardControl"/>.</value>
		[Category("Wizard"), Description("Collection of wizard pages.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public WizardPageCollection Pages { get; private set; }

		/// <summary>
		/// Gets the currently selected wizard page.
		/// </summary>
		/// <value>The selected wizard page. <c>null</c> if no page is active.</value>
		[Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual WizardPage SelectedPage
		{
			get
			{
				if ((this.selectedPage == null) || (this.Pages.Count == 0))
					return null;
				return this.selectedPage;
			}
			internal set
			{
				if (value != null && !Pages.Contains(value))
					throw new ArgumentException("WizardPage is not in the Pages collection for the control.");

				System.Diagnostics.Debug.WriteLine(string.Format("SelectPage: New={0},Prev={1}",
					value == null ? "null" : value.Name, selectedPage == null ? "null" : selectedPage.Name));
				if (value != selectedPage)
				{
					WizardPage prev = selectedPage;
					if (selectedPage != null)
						selectedPage.Hide();
					selectedPage = value;
					int idx = SelectedPageIndex;
					while (idx < Pages.Count - 1 && selectedPage.Suppress)
						selectedPage = Pages[++idx];
					if (selectedPage != null)
					{
						this.HeaderText = selectedPage.Text;
						selectedPage.InitializePage(prev);
						selectedPage.Dock = DockStyle.Fill;
						selectedPage.PerformLayout();
						selectedPage.Show();
						selectedPage.BringToFront();
						selectedPage.Focus();
					}
					UpdateButtons();
					OnSelectedPageChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to suppress changing the parent form's icon to match the wizard's <see cref="TitleIcon"/>.
		/// </summary>
		/// <value><c>true</c> to not change the parent form's icon to match this wizard's icon; otherwise, <c>false</c>.</value>
		[Category("Wizard"), DefaultValue(false), Description("Indicates whether to suppress changing the parent form's icon to match the wizard's")]
		public bool SuppressParentFormIconSync { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to spupress changing the parent form's caption to match the wizard's <see cref="Title"/>.
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
					titleImageList.Images.Add(value);
					titleImage.ImageIndex = 0;
				}
				titleImageIconSet = true;
				base.Invalidate();
			}
		}

		/// <summary>
		/// Gets the index of the currently selected page.
		/// </summary>
		/// <value>The index of the selected page.</value>
		internal int SelectedPageIndex
		{
			get
			{
				if (selectedPage == null)
					return -1;
				return Pages.IndexOf(selectedPage);
			}
		}

		/// <summary>
		/// Signals the object that initialization is starting.
		/// </summary>
		public void BeginInit()
		{
			initializing = true;
		}

		/// <summary>
		/// Signals the object that initialization is complete.
		/// </summary>
		public void EndInit()
		{
			initializing = false;
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
		public virtual void NextPage(WizardPage nextPage)
		{
			if (this.IsDesignMode())
			{
				int idx = SelectedPageIndex;
				if (idx < Pages.Count - 1)
					SelectedPage = Pages[idx + 1];
				return;
			}

			if (SelectedPage.CommitPage())
			{
				pageHistory.Push(SelectedPage);

				if (nextPage != null)
				{
					if (!Pages.Contains(nextPage))
						throw new ArgumentException("When specifying a value for nextPage, it must already be in the Pages collection.", "nextPage");
					SelectedPage = nextPage;
				}
				else
				{
					WizardPage selNext = GetNextPage(SelectedPage);

					// Check for last page
					if (SelectedPage.IsFinishPage || selNext == null)
					{
						OnFinished();
						return;
					}

					// Set new SelectedPage value
					SelectedPage = selNext;
				}

			}
		}

		/// <summary>
		/// Returns to the previous page.
		/// </summary>
		public virtual void PreviousPage()
		{
			if (this.IsDesignMode())
			{
				int idx = SelectedPageIndex;
				if (idx > 0)
					SelectedPage = Pages[idx - 1];
				return;
			}

			if (SelectedPage.RollbackPage())
				SelectedPage = pageHistory.Pop();
		}

		/// <summary>
		/// Restarts the wizard pages from the first page.
		/// </summary>
		public void RestartPages()
		{
			if (selectedPage != null)
				selectedPage.Hide();
			selectedPage = null;
			pageHistory.Clear();
			initialized = false;
		}

		/// <summary>
		/// Gets the content area rectangle.
		/// </summary>
		/// <param name="parentRelative">if set to <c>true</c> rectangle is relative to parent.</param>
		/// <returns>Coordinates of content area.</returns>
		internal Rectangle GetContentAreaRectangle(bool parentRelative)
		{
			int[] cw = contentArea.GetColumnWidths();
			int[] ch = contentArea.GetRowHeights();
			Rectangle r = new Rectangle(cw[0], 0, cw[1], ch[0]);
			if (parentRelative)
				r.Offset(contentArea.Location);
			return r;
		}

		/// <summary>
		/// Raises the <see cref="WizardControl.Cancelled"/> event.
		/// </summary>
		/// <remarks>The <see cref="WizardControl.OnCancelled"/> method is obsolete in version 1.2; use the <see cref="WizardControl.OnCancelling"/> method instead.</remarks>
		[Obsolete("The OnCancelled method is obsolete in version 1.2; use the OnCancelling method instead.")]
		protected virtual void OnCancelled()
		{
			EventHandler h = Cancelled;
			if (h != null)
				h(this, EventArgs.Empty);

			if (!this.IsDesignMode())
				CloseForm(DialogResult.Cancel);
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
#pragma warning disable 618
				OnCancelled();
#pragma warning restore 618
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
			if (selectedPage != null)
				selectedPage.Focus();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnHandleCreated(EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("OnHandleCreated");
			base.OnHandleCreated(e);
			InitialSetup();
			if (isMin6 && !this.IsDesignMode())
				DesktopWindowManager.CompositionChanged += DesktopWindowManager_CompositionChanged;
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (isMin6 && !this.IsDesignMode())
				DesktopWindowManager.CompositionChanged -= DesktopWindowManager_CompositionChanged;
			base.OnHandleDestroyed(e);
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
		/// Raises the <see cref="WizardControl.SelectedPageChanged"/> event.
		/// </summary>
		protected void OnSelectedPageChanged()
		{
			EventHandler temp = SelectedPageChanged;
			if (temp != null)
				temp(this, EventArgs.Empty);
		}

		/// <summary>
		/// Updates the buttons according to current sequence and history.
		/// </summary>
		protected internal void UpdateButtons()
		{
			System.Diagnostics.Debug.WriteLine(string.Format("UpdBtn: hstCnt={0},pgIdx={1}:{2},isFin={3}", pageHistory.Count, SelectedPageIndex, Pages.Count, selectedPage == null ? false : selectedPage.IsFinishPage));
			if (selectedPage == null)
			{
				CancelButtonState = this.IsDesignMode() ? WizardCommandButtonState.Disabled : WizardCommandButtonState.Enabled;
				NextButtonState = BackButtonState = WizardCommandButtonState.Hidden;
			}
			else
			{
				if (this.IsDesignMode())
				{
					CancelButtonState = WizardCommandButtonState.Disabled;
					NextButtonState = SelectedPageIndex == Pages.Count - 1 ? WizardCommandButtonState.Disabled : WizardCommandButtonState.Enabled;
					BackButtonState = SelectedPageIndex <= 0 ? WizardCommandButtonState.Disabled : WizardCommandButtonState.Enabled;
				}
				else
				{
					CancelButtonState = selectedPage.ShowCancel ? (selectedPage.AllowCancel && !this.IsDesignMode() ? WizardCommandButtonState.Enabled : WizardCommandButtonState.Disabled) : WizardCommandButtonState.Hidden;
					NextButtonState = selectedPage.ShowNext ? (selectedPage.AllowNext ? WizardCommandButtonState.Enabled : WizardCommandButtonState.Disabled) : WizardCommandButtonState.Hidden;
					if (selectedPage.IsFinishPage || IsLastPage(SelectedPage))
						nextButton.Text = FinishButtonText;
					else
						nextButton.Text = NextButtonText;
					BackButtonState = (pageHistory.Count == 0 || !selectedPage.AllowBack) ? WizardCommandButtonState.Disabled : WizardCommandButtonState.Enabled;
				}
			}
		}

		private void backButton_Click(object sender, EventArgs e)
		{
			PreviousPage();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			OnCancelling();
		}

		private void CloseForm(DialogResult dlgResult)
		{
			Form form = base.FindForm();
			if (form != null && form.Modal)
				form.DialogResult = dlgResult;
		}

		private void ConfigureWindowFrame()
		{
			System.Diagnostics.Debug.WriteLine(string.Format("ConfigureWindowFrame: hasGlass={0},parentForm={1}", HasGlass(), parentForm == null ? "null" : parentForm.Name));
			if (HasGlass())
			{
				titleBar.BackColor = Color.Black;
				try
				{
					if (parentForm != null)
						parentForm.ExtendFrameIntoClientArea(new Padding(0) { Top = titleBar.Height });
				}
				catch { titleBar.BackColor = commandArea.BackColor; }
			}
			else
				titleBar.BackColor = commandArea.BackColor;

			if (parentForm != null)
			{
				if (!this.SuppressParentFormCaptionSync)
					parentForm.Text = this.Title;
				if (!this.SuppressParentFormIconSync && this.titleImageIcon != null)
				{
					parentForm.Icon = this.TitleIcon;
					parentForm.ShowIcon = true;
				}
				parentForm.SetWindowThemeAttribute(VisualStyleRendererExtender.WindowThemeNonClientAttributes.NoDrawCaption | VisualStyleRendererExtender.WindowThemeNonClientAttributes.NoDrawIcon | VisualStyleRendererExtender.WindowThemeNonClientAttributes.NoSysMenu);
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

		private void DesktopWindowManager_CompositionChanged(object sender, EventArgs e)
		{
			ConfigureWindowFrame();
		}

		private WizardCommandButtonState GetCmdButtonState(ButtonBase btn)
		{
			if (!btn.Visible)
				return WizardCommandButtonState.Hidden;
			else
			{
				if (btn.Enabled)
					return WizardCommandButtonState.Enabled;
				else
					return WizardCommandButtonState.Disabled;
			}
		}

		private WizardPage GetNextPage(WizardPage page)
		{
			if (page == null || page.IsFinishPage)
				return null;

			do
			{
				int pgIdx = Pages.IndexOf(page);
				if (page.NextPage != null)
					page = page.NextPage;
				else if (pgIdx == Pages.Count - 1)
					page = null;
				else
					page = Pages[pgIdx + 1];
			} while (page != null && page.Suppress);

			return page;
		}

		private bool HasGlass()
		{
			return isMin6 && DesktopWindowManager.IsCompositionEnabled();
		}

		private void InitialSetup()
		{
			if (!initialized)
			{
				SetLayout();
				if (Pages.Count > 0)
					SelectedPage = Pages[0];
				else
					UpdateButtons();
				initialized = true;
			}
		}

		private bool IsLastPage(WizardPage page)
		{
			return GetNextPage(page) == null;
		}

		private void nextButton_Click(object sender, EventArgs e)
		{
			NextPage();
		}

		private void Pages_AddItem(object sender, EventedList<WizardPage>.ListChangedEventArgs<WizardPage> e)
		{
			Pages_AddItemHandler(e.Item, !initializing);
		}

		private void Pages_AddItemHandler(WizardPage item, bool selectAfterAdd)
		{
			System.Diagnostics.Debug.WriteLine(string.Format("AddPage: {0},sel={1}",
				item == null ? "null" : item.Text, selectAfterAdd));
			item.Owner = this;
			item.Visible = false;
			if (!contentArea.Contains(item))
				contentArea.Controls.Add(item, 1, 0);
			if (selectAfterAdd)
				SelectedPage = item;
		}

		private void Pages_RemoveItem(object sender, EventedList<WizardPage>.ListChangedEventArgs<WizardPage> e)
		{
			contentArea.Controls.Remove(e.Item);
			if (e.Item == selectedPage && Pages.Count > 0)
				SelectedPage = Pages[e.ItemIndex == Pages.Count ? e.ItemIndex - 1 : e.ItemIndex];
			else
				UpdateButtons();
		}

		private void Pages_Reset(object sender, EventedList<WizardPage>.ListChangedEventArgs<WizardPage> e)
		{
			WizardPage curPage = selectedPage;
			SelectedPage = null;
			contentArea.Controls.Clear();
			foreach (var item in Pages)
				Pages_AddItemHandler(item, false);
			if (Pages.Count > 0)
				SelectedPage = Pages.Contains(curPage) ? curPage : Pages[0];
		}

		private void parentForm_Load(object sender, EventArgs e)
		{
			ConfigureWindowFrame();
		}

		private void ResetBackButtonToolTipText()
		{
			BackButtonToolTipText = Properties.Resources.WizardBackButtonToolTip;
		}

		private void ResetCancelButtonText()
		{
			CancelButtonText = Properties.Resources.WizardCancelText;
		}

		private void ResetFinishButtonText()
		{
			FinishButtonText = Properties.Resources.WizardFinishText;
		}

		private void ResetNextButtonText()
		{
			NextButtonText = Properties.Resources.WizardNextText;
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

		private void SetCmdButtonState(ButtonBase btn, WizardCommandButtonState value)
		{
			switch (value)
			{
				case WizardCommandButtonState.Disabled:
					btn.Enabled = false;
					btn.Visible = true;
					break;
				case WizardCommandButtonState.Hidden:
					btn.Enabled = false;
					if (btn != backButton)
						btn.Visible = false;
					break;
				case WizardCommandButtonState.Enabled:
				default:
					btn.Enabled = btn.Visible = true;
					break;
			}

			if (btn == cancelButton || btn == nextButton)
			{
				commandArea.Visible = (cancelButton.Visible || nextButton.Visible);
			}

			base.Invalidate();
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
					titleBar.Height = Math.Max(theme.GetMargins2(g, MarginProperty.ContentMargins).Top, bbSize.Height + 2);
					titleBar.ColumnStyles[0].Width = bbSize.Width + 4F;
					titleBar.ColumnStyles[1].Width = titleImageIcon != null ? titleImageList.ImageSize.Width + 4F : 0;
					backButton.Size = bbSize;

					// Header
					theme.SetParameters(VisualStyleElementEx.AeroWizard.HeaderArea.Normal);
					headerLabel.Margin = theme.GetMargins2(g, MarginProperty.ContentMargins);
					headerLabel.ForeColor = theme.GetColor(ColorProperty.TextColor);

					// Content
					theme.SetParameters(VisualStyleElementEx.AeroWizard.ContentArea.Normal);
					this.BackColor = theme.GetColor(ColorProperty.FillColor);
					Padding cp = theme.GetMargins2(g, MarginProperty.ContentMargins);
					contentArea.ColumnStyles[0].Width = cp.Left;
					contentArea.RowStyles[1].Height = cp.Bottom;

					// Command
					theme.SetParameters(VisualStyleElementEx.AeroWizard.CommandArea.Normal);
					cp = theme.GetMargins2(g, MarginProperty.ContentMargins);
					commandArea.RowStyles[0].Height = cp.Top;
					commandArea.RowStyles[2].Height = cp.Bottom;
					commandArea.ColumnStyles[2].Width = contentArea.ColumnStyles[2].Width = cp.Right;
				}
			}
			else
			{
				backButton.Size = new Size(Properties.Resources.BackBtnStrip.Width, Properties.Resources.BackBtnStrip.Height / 4);
			}
		}

		private bool ShouldSerializeBackButtonToolTipText()
		{
			return BackButtonToolTipText != Properties.Resources.WizardBackButtonToolTip;
		}

		private bool ShouldSerializeCancelButtonText()
		{
			return CancelButtonText != Properties.Resources.WizardCancelText;
		}

		private bool ShouldSerializeFinishButtonText()
		{
			return FinishButtonText != Properties.Resources.WizardFinishText;
		}

		private bool ShouldSerializeNextButtonText()
		{
			return NextButtonText != Properties.Resources.WizardNextText;
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