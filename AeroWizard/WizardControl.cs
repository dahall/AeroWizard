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
        private static ImageList iconList;

        private string finishBtnText;
        private Point formMoveLastMousePos;
        private bool formMoveTracking;
        private bool initialized = false;
        private bool initializing = false;
        private string nextBtnText;
        private Stack<WizardPage> pageHistory;
        private Form parentForm;
        private WizardPage selectedPage;

        static WizardControl()
        {
            iconList = new ImageList();
            iconList.Images.Add(Properties.Resources.WizardControlIcon);
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

            // Get localized defaults for button text
            ResetBackButtonToolTipText();
            ResetCancelButtonText();
            ResetFinishButtonText();
            ResetNextButtonText();
            ResetTitle();
            ResetTitleImage();
        }

        /// <summary>
        /// Occurs when the user clicks the Cancel button.
        /// </summary>
        public event EventHandler Cancelled;

        /// <summary>
        /// Occurs when the user clicks the Next/Finish button and the page is set to <see cref="WizardPage.IsFinishPage"/> or this is the last page in the <see cref="Pages"/> collection.
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// Occurs when the <see cref="WizardControl.SelectedPage"/> property has changed.
        /// </summary>
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
        [Category("Wizard"),
        Localizable(true)]
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
        [Category("Wizard"),
        Localizable(true)]
        public string CancelButtonText
        {
            get { return cancelButton.Text; }
            set { cancelButton.Text = value; base.Invalidate(); }
        }

		/// <summary>
		/// Gets or sets the finish button text.
		/// </summary>
		/// <value>The finish button text.</value>
        [Category("Wizard"),
        Localizable(true)]
        public string FinishButtonText
        {
            get { return finishBtnText; }
            set
            {
                finishBtnText = value;
                if (selectedPage != null && selectedPage.IsFinishPage && !DesignMode)
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
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string HeaderText
        {
            get { return headerLabel.Text; }
            set { headerLabel.Text = value; base.Invalidate(); }
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
        [Category("Wizard"),
        Localizable(true)]
        public string NextButtonText
        {
            get { return nextBtnText; }
            set
            {
                nextBtnText = value;
                if (!DesignMode && (selectedPage == null || !selectedPage.IsFinishPage))
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
        [Category("Wizard")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public WizardPageCollection Pages
        {
            get; private set;
        }

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
                    if (selectedPage != null)
                        selectedPage.Visible = false;
                    selectedPage = value;
                    if (value != null)
                    {
                        this.HeaderText = value.Text;
                        value.InitializePage(selectedPage);
                        selectedPage.Dock = DockStyle.Fill;
                        selectedPage.Visible = true;
                        selectedPage.BringToFront();
                        selectedPage.Focus();
                    }
                    UpdateButtons();
                    OnSelectedPageChanged();
                }
            }
        }

		/// <summary>
		/// Gets or sets the title for the wizard.
		/// </summary>
		/// <value>The title text.</value>
        [Category("Wizard"),
        Localizable(true)]
        public string Title
        {
            get { return title.Text; }
            set { title.Text = value; base.Invalidate(); }
        }

		/// <summary>
		/// Gets or sets the optionally displayed image next to the wizard title.
		/// </summary>
		/// <value>The title image.</value>
        [Category("Wizard"),
        Localizable(true)]
        public Image TitleImage
        {
            get { return titleImage.Image; }
            set { titleImage.Image = value; base.Invalidate(); }
        }

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
            if (DesignMode)
            {
                int idx = SelectedPageIndex;
                if (idx < Pages.Count - 1)
                    SelectedPage = Pages[idx + 1];
                return;
            }

            if (nextPage == null)
            {
                if (SelectedPage.IsFinishPage || Pages.IndexOf(SelectedPage) == Pages.Count - 1)
                {
                    OnFinished();
                    return;
                }
            }
            else if (!Pages.Contains(nextPage))
                throw new ArgumentException("When specifying a value for nextPage, it must already be in the Pages collection.", "nextPage");

            pageHistory.Push(SelectedPage);
            if (SelectedPage.CommitPage())
            {
                if (nextPage != null)
                    SelectedPage = nextPage;
                else if (SelectedPage.NextPage != null)
                    SelectedPage = SelectedPage.NextPage;
                else
                    SelectedPage = Pages[Pages.IndexOf(SelectedPage) + 1];
            }
        }

		/// <summary>
		/// Returns to the previous page.
		/// </summary>
        public virtual void PreviousPage()
        {
            if (DesignMode)
            {
                int idx = SelectedPageIndex;
                if (idx > 0)
                    SelectedPage = Pages[idx - 1];
                return;
            }

            if (SelectedPage.RollbackPage())
                SelectedPage = pageHistory.Pop();
        }

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
        protected virtual void OnCancelled()
        {
            EventHandler h = Cancelled;
            if (h != null)
                h(this, EventArgs.Empty);

            if (!DesignMode)
                CloseForm(DialogResult.Cancel);
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

            if (!DesignMode)
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
            base.OnHandleCreated(e);
            InitialSetup();
        }

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (parentForm != null)
                parentForm.HandleCreated -= parentForm_HandleCreated;
            parentForm = base.Parent as Form;
            this.Dock = DockStyle.Fill;
            if (parentForm != null)
            {
                parentForm.HandleCreated += parentForm_HandleCreated;
                //Margins margins = new Margins(0) { Top = titleBar.Height };
            }
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
		protected void UpdateButtons()
		{
			System.Diagnostics.Debug.WriteLine(string.Format("UpdBtn: hstCnt={0},pgIdx={1}:{2},isFin={3}", pageHistory.Count, SelectedPageIndex, Pages.Count, selectedPage == null ? false : selectedPage.IsFinishPage));
			if (DesignMode)
				BackButtonState = SelectedPageIndex <= 0 ? WizardCommandButtonState.Disabled : WizardCommandButtonState.Enabled;
			else
				BackButtonState = pageHistory.Count == 0 ? WizardCommandButtonState.Disabled : WizardCommandButtonState.Enabled;
			if (selectedPage == null)
			{
				CancelButtonState = this.DesignMode ? WizardCommandButtonState.Disabled : WizardCommandButtonState.Enabled;
				NextButtonState = WizardCommandButtonState.Hidden;
			}
			else
			{
				if (DesignMode)
				{
					CancelButtonState = WizardCommandButtonState.Disabled;
					NextButtonState = SelectedPageIndex == Pages.Count - 1 ? WizardCommandButtonState.Disabled : WizardCommandButtonState.Enabled;
				}
				else
				{
					CancelButtonState = selectedPage.ShowCancel ? (selectedPage.AllowCancel && !this.DesignMode ? WizardCommandButtonState.Enabled : WizardCommandButtonState.Disabled) : WizardCommandButtonState.Hidden;
					NextButtonState = selectedPage.ShowNext ? (selectedPage.AllowNext ? WizardCommandButtonState.Enabled : WizardCommandButtonState.Disabled) : WizardCommandButtonState.Hidden;
					if (selectedPage.IsFinishPage || Pages.IndexOf(SelectedPage) == Pages.Count - 1)
						nextButton.Text = FinishButtonText;
					else
						nextButton.Text = NextButtonText;
				}
			}
		}

		private void backButton_Click(object sender, EventArgs e)
        {
            PreviousPage();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            OnCancelled();
        }

        private void CloseForm(DialogResult dlgResult)
        {
            Form form = base.FindForm();
            if (form != null && form.Modal)
                form.DialogResult = dlgResult;
        }

        private void ConfigureWindowFrame(bool compositionEnabled)
        {
            if (compositionEnabled)
            {
                titleBar.BackColor = Color.Black;
                parentForm.ExtendFrameIntoClientArea(new Padding(0) { Top = titleBar.Height });
            }
            else
                titleBar.BackColor = SystemColors.ActiveCaption;
        }

        private void contentArea_Paint(object sender, PaintEventArgs pe)
        {
            if (this.DesignMode && this.Pages.Count == 0)
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
            ConfigureWindowFrame(DesktopWindowManager.IsCompositionEnabled());
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

        private void parentForm_HandleCreated(object sender, EventArgs e)
        {
            ConfigureWindowFrame(!DesignMode);
            if (!DesignMode)
                DesktopWindowManager.CompositionChanged += DesktopWindowManager_CompositionChanged;
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

        private void ResetTitleImage()
        {
            titleImage.Image = null;
            titleImage.ImageList = iconList;
            titleImage.ImageIndex = 0;
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
            const string aw = "AEROWIZARD";
            VisualStyleRenderer theme = new VisualStyleRenderer(aw, 0, 0);
            using (Graphics g = this.CreateGraphics())
            {
                // Title
                theme.SetParameters(aw, 1, 0);
                titleBar.Height = theme.GetMargins2(g, MarginProperty.ContentMargins).Top;

                // Header
                theme.SetParameters(aw, 2, 0);
                headerLabel.Margin = theme.GetMargins2(g, MarginProperty.ContentMargins);
                headerLabel.ForeColor = theme.GetColor(ColorProperty.TextColor);

                // Content
                theme.SetParameters(aw, 3, 0);
                this.BackColor = theme.GetColor(ColorProperty.FillColor);
                Padding cp = theme.GetMargins2(g, MarginProperty.ContentMargins);
                contentArea.ColumnStyles[0].Width = cp.Left;
                contentArea.RowStyles[1].Height = cp.Bottom;

                // Command
                theme.SetParameters(aw, 4, 0);
                cp = theme.GetMargins2(g, MarginProperty.ContentMargins);
                commandArea.RowStyles[0].Height = cp.Top;
                commandArea.RowStyles[2].Height = cp.Bottom;
                commandArea.ColumnStyles[2].Width = contentArea.ColumnStyles[2].Width = cp.Right;
                //commandArea.BackColor = theme.GetColor(4, 0, 3802);

                // Buttons
                theme.SetParameters(aw, 5, 0);
                int btnHeight = theme.GetInteger(IntegerProperty.Height);
                nextButton.Height = btnHeight;
                cancelButton.Height = btnHeight;
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

        private bool ShouldSerializeTitleImage()
        {
            return titleImage.ImageList == null;
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