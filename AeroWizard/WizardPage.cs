using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AeroWizard
{
    [Designer(typeof(WizardPageDesigner)), DesignTimeVisible(false), DefaultProperty("Text")]
    public partial class WizardPage : Control
    {
        private bool initializing = false;

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

        public event EventHandler<WizardPageConfirmEventArgs> Commit;
        public event EventHandler<WizardPageInitEventArgs> Initialize;
        public event EventHandler<WizardPageConfirmEventArgs> Rollback;

        [DefaultValue(true), Category("Behavior")]
        public virtual bool AllowCancel { get; set; }

        [DefaultValue(true), Category("Behavior")]
        public virtual bool AllowNext { get; set; }

        [DefaultValue(false), Category("Behavior")]
        public virtual bool IsFinishPage { get; set; }

		[DefaultValue(null), Category("Behavior"), Description("Specify a page other than the next page in the Pages collection as the next page.")]
        public virtual WizardPage NextPage { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual WizardControl Owner { get; set; }

        [DefaultValue(true), Category("Behavior")]
        public virtual bool ShowCancel { get; set; }

        [DefaultValue(true), Category("Behavior")]
        public virtual bool ShowNext { get; set; }

        public override string ToString()
        {
            return this.Text;
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

        protected virtual bool OnCommit()
        {
            EventHandler<WizardPageConfirmEventArgs> handler = Commit;
            WizardPageConfirmEventArgs e =  new WizardPageConfirmEventArgs(this);
            if (handler != null)
                handler(this,e);
            return !e.Cancel;
        }

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			Control firstChild = this.GetNextControl(this, true);
			if (firstChild != null)
				firstChild.Focus();
		}

        protected virtual void OnInitialize(WizardPage prevPage)
        {
            EventHandler<WizardPageInitEventArgs> handler = Initialize;
            WizardPageInitEventArgs e = new WizardPageInitEventArgs(this, prevPage);
            if (handler != null)
                handler(this, e);
        }

        protected virtual bool OnRollback()
        {
            EventHandler<WizardPageConfirmEventArgs> handler = Rollback;
            WizardPageConfirmEventArgs e = new WizardPageConfirmEventArgs(this);
            if (handler != null)
                handler(this, e);
            return !e.Cancel;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!initializing && Owner != null && Owner.SelectedPage == this)
                Owner.HeaderText = base.Text;
        }
	}

    public class WizardPageConfirmEventArgs : EventArgs
    {
        public WizardPageConfirmEventArgs(WizardPage page)
        {
            Cancel = false;
            Page = page;
        }

        public bool Cancel
        {
            get; set;
        }

        public WizardPage Page
        {
            get; private set;
        }
    }

    public class WizardPageInitEventArgs : WizardPageConfirmEventArgs
    {
        public WizardPageInitEventArgs(WizardPage page, WizardPage prevPage)
            : base(page)
        {
            PreviousPage = prevPage;
        }

        public WizardPage PreviousPage
        {
            get; private set;
        }
    }
}