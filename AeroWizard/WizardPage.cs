using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AeroWizard
{
    /// <summary>
    /// Represents a single page in a <see cref="WizardControl"/>.
    /// </summary>
	[Designer(typeof(Design.WizardPageDesigner)), DesignTimeVisible(true)]
    [DefaultProperty("Text"), DefaultEvent("Commit")]
    [ToolboxItem(false)]
    public partial class WizardPage : Control
    {
        private bool initializing = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="WizardPage"/> class.
		/// </summary>
        public WizardPage()
        {
            initializing = true;
            InitializeComponent();
            Margin = Padding.Empty;
            AllowCancel = AllowNext = true;
            ShowCancel = ShowNext = true;
            IsFinishPage = false;
            base.Text = Properties.Resources.WizardHeader;
            initializing = false;
        }

		/// <summary>
		/// Occurs when the user has clicked the Next/Finish button but before the page is changed.
		/// </summary>
        public event EventHandler<WizardPageConfirmEventArgs> Commit;

		/// <summary>
		/// Occurs when this page is entered.
		/// </summary>
        public event EventHandler<WizardPageInitEventArgs> Initialize;

		/// <summary>
		/// Occurs when the user has clicked the Back button but before the page is changed.
		/// </summary>
        public event EventHandler<WizardPageConfirmEventArgs> Rollback;

		/// <summary>
		/// Gets or sets a value indicating whether to enable the Cancel button.
		/// </summary>
		/// <value><c>true</c> if Cancel button is enabled; otherwise, <c>false</c>.</value>
        [DefaultValue(true),
        Category("Behavior")]
        public virtual bool AllowCancel
        {
            get; set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether to enable the Next/Finish button.
		/// </summary>
		/// <value><c>true</c> if Next/Finish button is enabled; otherwise, <c>false</c>.</value>
		[DefaultValue(true),
        Category("Behavior")]
        public virtual bool AllowNext
        {
            get; set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether this page is the last page in the sequence and should display the Finish text instead of the Next text on the Next/Finish button.
		/// </summary>
		/// <value><c>true</c> if this page is a finish page; otherwise, <c>false</c>.</value>
        [DefaultValue(false),
        Category("Behavior")]
        public virtual bool IsFinishPage
        {
            get; set;
        }

		/// <summary>
		/// Gets or sets the next page that should be used when the user clicks the Next button or when the <see cref="WizardControl.NextPage()"/> method is called. This is used to override the default behavior of going to the next page in the sequence defined within the <see cref="WizardControl.Pages"/> collection.
		/// </summary>
		/// <value>The wizard page to go to.</value>
        [DefaultValue(null),
        Category("Behavior"),
        Description("Specify a page other than the next page in the Pages collection as the next page.")]
        public virtual WizardPage NextPage
        {
            get; set;
        }

		/// <summary>
		/// Gets the <see cref="WizardControl"/> for this page.
		/// </summary>
		/// <value>The <see cref="WizardControl"/> for this page.</value>
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual WizardControl Owner
        {
            get; internal set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether to show the Cancel button. If both <see cref="ShowCancel"/> and <see cref="ShowNext"/> are <c>false</c>, then the bottom command area will not be shown.
		/// </summary>
		/// <value><c>true</c> if Cancel button should be shown; otherwise, <c>false</c>.</value>
        [DefaultValue(true),
        Category("Behavior")]
        public virtual bool ShowCancel
        {
            get; set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether to show the Next/Finish button. If both <see cref="ShowCancel"/> and <see cref="ShowNext"/> are <c>false</c>, then the bottom command area will not be shown.
		/// </summary>
		/// <value><c>true</c> if Next/Finish button should be shown; otherwise, <c>false</c>.</value>
		[DefaultValue(true),
        Category("Behavior")]
        public virtual bool ShowNext
        {
            get; set;
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this wizard page.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this wizard page.
		/// </returns>
        public override string ToString()
        {
            return string.Format("{0} (\"{1}\")", this.Name, this.Text);
        }

        internal bool CommitPage()
        {
            return OnCommit();
        }

        internal void InitializePage(WizardPage prevPage)
        {
            OnInitialize(prevPage);
        }

        internal bool RollbackPage()
        {
            return OnRollback();
        }

		/// <summary>
		/// Raises the <see cref="Commit"/> event.
		/// </summary>
		/// <returns></returns>
        protected virtual bool OnCommit()
        {
            EventHandler<WizardPageConfirmEventArgs> handler = Commit;
            WizardPageConfirmEventArgs e =  new WizardPageConfirmEventArgs(this);
            if (handler != null)
                handler(this,e);
            return !e.Cancel;
        }

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Control firstChild = this.GetNextControl(this, true);
            if (firstChild != null)
                firstChild.Focus();
        }

		/// <summary>
		/// Raises the <see cref="Initialize"/> event.
		/// </summary>
		/// <param name="prevPage">The page that was previously selected.</param>
        protected virtual void OnInitialize(WizardPage prevPage)
        {
            EventHandler<WizardPageInitEventArgs> handler = Initialize;
            WizardPageInitEventArgs e = new WizardPageInitEventArgs(this, prevPage);
            if (handler != null)
                handler(this, e);
        }

		/// <summary>
		/// Raises the <see cref="Rollback"/> event.
		/// </summary>
		/// <returns></returns>
        protected virtual bool OnRollback()
        {
            EventHandler<WizardPageConfirmEventArgs> handler = Rollback;
            WizardPageConfirmEventArgs e = new WizardPageConfirmEventArgs(this);
            if (handler != null)
                handler(this, e);
            return !e.Cancel;
        }

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.TextChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnTextChanged(EventArgs e)
        {
            if (!initializing && Owner != null && Owner.SelectedPage == this)
                Owner.HeaderText = base.Text;
        }
    }

	/// <summary>
	/// Arguments supplied to the <see cref="WizardPage"/> events.
	/// </summary>
    public class WizardPageConfirmEventArgs : EventArgs
    {
        internal WizardPageConfirmEventArgs(WizardPage page)
        {
            Cancel = false;
            Page = page;
        }

		/// <summary>
		/// Gets or sets a value indicating whether this action is to be cancelled or allowed.
		/// </summary>
		/// <value><c>true</c> if cancel; otherwise, <c>false</c> to allow. Default is <c>false</c>.</value>
		[DefaultValue(false)]
        public bool Cancel
        {
            get; set;
        }

		/// <summary>
		/// Gets the <see cref="WizardPage"/> that has raised the event.
		/// </summary>
		/// <value>The wizard page.</value>
        public WizardPage Page
        {
            get; private set;
        }
    }

	/// <summary>
	/// Arguments supplied to the <see cref="WizardPage.Initialize"/> event.
	/// </summary>
    public class WizardPageInitEventArgs : WizardPageConfirmEventArgs
    {
        internal WizardPageInitEventArgs(WizardPage page, WizardPage prevPage)
            : base(page)
        {
            PreviousPage = prevPage;
        }

		/// <summary>
		/// Gets the <see cref="WizardPage"/> that was previously selected when the event was raised.
		/// </summary>
		/// <value>The previous wizard page.</value>
        public WizardPage PreviousPage
        {
            get; private set;
        }
    }
}