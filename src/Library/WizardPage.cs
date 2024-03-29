﻿using AeroWizard.Properties;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AeroWizard
{
	/// <summary>Represents a single page in a <see cref="WizardControl"/>.</summary>
	[Designer(typeof(Design.WizardPageDesigner)), DesignTimeVisible(true)]
	[DefaultProperty("Text"), DefaultEvent("Commit")]
	[ToolboxItem(false)]
	public partial class WizardPage : Control
	{
		private bool allowCancel = true, allowNext = true, allowBack = true;
		private LinkLabel helpLink;
		private string helpText;
		private bool isFinishPage;
		private bool showCancel = true, showNext = true, suppress;

		/// <summary>Initializes a new instance of the <see cref="WizardPage"/> class.</summary>
		public WizardPage()
		{
			InitializeComponent();
			Margin = Padding.Empty;
			base.Text = Resources.WizardHeader;
		}

		/// <summary>Occurs when the user has clicked the Next/Finish button but before the page is changed.</summary>
		[Category("Wizard"), Description("Occurs when the user has clicked the Next/Finish button but before the page is changed")]
		public event EventHandler<WizardPageConfirmEventArgs> Commit;

		/// <summary>Occurs when <see cref="HelpText"/> is set and the user has clicked the link at bottom of the content area.</summary>
		[Category("Wizard"), Description("Occurs when the user has clicked the help link")]
		public event EventHandler HelpClicked;

		/// <summary>Occurs when this page is entered.</summary>
		[Category("Wizard"), Description("Occurs when this page is entered")]
		public event EventHandler<WizardPageInitEventArgs> Initialize;

		/// <summary>Occurs when the user has clicked the Back button but before the page is changed.</summary>
		[Category("Wizard"), Description("Occurs when the user has clicked the Back button")]
		public event EventHandler<WizardPageConfirmEventArgs> Rollback;

		/// <summary>Gets or sets a value indicating whether to enable the Back button.</summary>
		/// <value><c>true</c> if Back button is enabled; otherwise, <c>false</c>.</value>
		[DefaultValue(true), Category("Behavior"), Description("Indicates whether to enable the Back button")]
		public virtual bool AllowBack
		{
			get => allowBack;
			set
			{
				if (allowBack == value)
				{
					return;
				}

				allowBack = value;
				UpdateOwner();
			}
		}

		/// <summary>Gets or sets a value indicating whether to enable the Cancel button.</summary>
		/// <value><c>true</c> if Cancel button is enabled; otherwise, <c>false</c>.</value>
		[DefaultValue(true), Category("Behavior"), Description("Indicates whether to enable the Cancel button")]
		public virtual bool AllowCancel
		{
			get => allowCancel;
			set
			{
				if (allowCancel == value)
				{
					return;
				}

				allowCancel = value;
				UpdateOwner();
			}
		}

		/// <summary>Gets or sets a value indicating whether to enable the Next/Finish button.</summary>
		/// <value><c>true</c> if Next/Finish button is enabled; otherwise, <c>false</c>.</value>
		[DefaultValue(true), Category("Behavior"), Description("Indicates whether to enable the Next/Finish button")]
		public virtual bool AllowNext
		{
			get => allowNext;
			set
			{
				if (allowNext == value)
				{
					return;
				}

				allowNext = value;
				UpdateOwner();
			}
		}

		/// <summary>
		/// Gets or sets the help text. When value is not <c>null</c>, a help link will be displayed at the bottom left of the content area. When
		/// clicked, the <see cref="OnHelpClicked"/> method will call the <see cref="HelpClicked"/> event.
		/// </summary>
		/// <value>The help text to display.</value>
		[DefaultValue(null), Category("Appearance"), Description("Help text to display on hyperlink at bottom left of content area.")]
		public string HelpText
		{
			get => helpText;
			set
			{
				if (helpLink is null)
				{
					helpLink = new LinkLabel() { AutoSize = true, Dock = DockStyle.Bottom, Text = Resources.WizardPageDefaultHelpText, Visible = false };
					helpLink.LinkClicked += helpLink_LinkClicked;
					Controls.Add(helpLink);
				}
				helpText = value;
				if (helpText is null)
				{
					helpLink.Visible = false;
				}
				else
				{
					helpLink.Text = helpText;
					helpLink.Visible = true;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this page is the last page in the sequence and should display the Finish text instead of the
		/// Next text on the Next/Finish button.
		/// </summary>
		/// <value><c>true</c> if this page is a finish page; otherwise, <c>false</c>.</value>
		[DefaultValue(false), Category("Behavior"), Description("Indicates whether this page is the last page")]
		public virtual bool IsFinishPage
		{
			get => isFinishPage;
			set
			{
				if (isFinishPage == value)
				{
					return;
				}

				isFinishPage = value;
				UpdateOwner();
			}
		}

		/// <summary>
		/// Gets or sets the next page that should be used when the user clicks the Next button or when the <see cref="WizardControl.NextPage"/>
		/// method is called. This is used to override the default behavior of going to the next page in the sequence defined within the <see
		/// cref="WizardControl.Pages"/> collection.
		/// </summary>
		/// <value>The wizard page to go to.</value>
		[DefaultValue(null), Category("Behavior"),
		Description("Specify a page other than the next page in the Pages collection as the next page.")]
		public virtual WizardPage NextPage { get; set; }

		/// <summary>Gets the <see cref="WizardControl"/> for this page.</summary>
		/// <value>The <see cref="WizardControl"/> for this page.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual WizardPageContainer Owner { get; internal set; }

		/// <summary>
		/// Gets or sets a value indicating whether to show the Cancel button. If both <see cref="ShowCancel"/> and <see cref="ShowNext"/> are
		/// <c>false</c>, then the bottom command area will not be shown.
		/// </summary>
		/// <value><c>true</c> if Cancel button should be shown; otherwise, <c>false</c>.</value>
		[DefaultValue(true), Category("Behavior"), Description("Indicates whether to show the Cancel button")]
		public virtual bool ShowCancel
		{
			get => showCancel;
			set
			{
				if (showCancel == value)
				{
					return;
				}

				showCancel = value;
				UpdateOwner();
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the Next/Finish button. If both <see cref="ShowCancel"/> and <see cref="ShowNext"/>
		/// are <c>false</c>, then the bottom command area will not be shown.
		/// </summary>
		/// <value><c>true</c> if Next/Finish button should be shown; otherwise, <c>false</c>.</value>
		[DefaultValue(true), Category("Behavior"), Description("Indicates whether to show the Next/Finish button")]
		public virtual bool ShowNext
		{
			get => showNext;
			set
			{
				if (showNext == value)
				{
					return;
				}

				showNext = value;
				UpdateOwner();
			}
		}

		/// <summary>Gets or sets the height and width of the control.</summary>
		/// <value></value>
		/// <returns>The <see cref="T:System.Drawing.Size"/> that represents the height and width of the control in pixels.</returns>
		[Browsable(false)]
		public new System.Drawing.Size Size { get => base.Size; set => base.Size = value; }

		/// <summary>Gets or sets a value indicating whether this <see cref="WizardPage"/> is suppressed and not shown in the normal flow.</summary>
		/// <value><c>true</c> if suppressed; otherwise, <c>false</c>.</value>
		[DefaultValue(false), Category("Behavior"), Description("Suppresses this page from viewing if selected as next.")]
		public virtual bool Suppress
		{
			get => suppress;
			set
			{
				if (suppress == value)
				{
					return;
				}

				suppress = value;
				UpdateOwner();
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>
		/// A <see cref="T:System.Windows.Forms.CreateParams"/> that contains the required creation parameters when the handle to the control is created.
		/// </returns>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				Form parent = FindForm();
				bool parentRightToLeftLayout = parent is not null && parent.RightToLeftLayout;
				if ((RightToLeft == RightToLeft.Yes) && parentRightToLeftLayout)
				{
					createParams.ExStyle |= 0x500000; // WS_EX_LAYOUTRTL | WS_EX_NOINHERITLAYOUT
					createParams.ExStyle &= ~0x7000; // WS_EX_RIGHT | WS_EX_RTLREADING | WS_EX_LEFTSCROLLBAR
				}
				return createParams;
			}
		}

		/// <summary>Returns a <see cref="string"/> that represents this wizard page.</summary>
		/// <returns>A <see cref="string"/> that represents this wizard page.</returns>
		public override string ToString() => $"{Name} (\"{Text}\")";

		internal bool CommitPage() => OnCommit();

		internal void InitializePage(WizardPage prevPage) => OnInitialize(prevPage);

		internal bool RollbackPage() => OnRollback();

		/// <summary>Raises the <see cref="Commit"/> event.</summary>
		/// <returns><c>true</c> if handler does not set the <see cref="WizardPageConfirmEventArgs.Cancel"/> to <c>true</c>; otherwise, <c>false</c>.</returns>
		protected virtual bool OnCommit()
		{
			WizardPageConfirmEventArgs e = new(this);
			Commit?.Invoke(this, e);
			return !e.Cancel;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"/> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			Control firstChild = GetNextControl(this, true);
			firstChild?.Focus();
		}

		/// <summary>Raises the <see cref="HelpClicked"/> event.</summary>
		protected virtual void OnHelpClicked() => HelpClicked?.Invoke(this, EventArgs.Empty);

		/// <summary>Raises the <see cref="Initialize"/> event.</summary>
		/// <param name="prevPage">The page that was previously selected.</param>
		protected virtual void OnInitialize(WizardPage prevPage) => Initialize?.Invoke(this, new WizardPageInitEventArgs(this, prevPage));

		/// <summary>Raises the <see cref="Rollback"/> event.</summary>
		/// <returns><c>true</c> if handler does not set the <see cref="WizardPageConfirmEventArgs.Cancel"/> to <c>true</c>; otherwise, <c>false</c>.</returns>
		protected virtual bool OnRollback()
		{
			WizardPageConfirmEventArgs e = new(this);
			Rollback?.Invoke(this, e);
			return !e.Cancel;
		}

		private void helpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => OnHelpClicked();

		private void UpdateOwner()
		{
			if (Owner is not null && this == Owner.SelectedPage)
			{
				Owner.UpdateUIDependencies();
			}
		}
	}

	/// <summary>Arguments supplied to the <see cref="WizardPage"/> events.</summary>
	public class WizardPageConfirmEventArgs : EventArgs
	{
		internal WizardPageConfirmEventArgs(WizardPage page)
		{
			Cancel = false;
			Page = page;
		}

		/// <summary>Gets or sets a value indicating whether this action is to be canceled or allowed.</summary>
		/// <value><c>true</c> if cancel; otherwise, <c>false</c> to allow. Default is <c>false</c>.</value>
		[DefaultValue(false)]
		public bool Cancel { get; set; }

		/// <summary>Gets the <see cref="WizardPage"/> that has raised the event.</summary>
		/// <value>The wizard page.</value>
		public WizardPage Page { get; }
	}

	/// <summary>Arguments supplied to the <see cref="WizardPage.Initialize"/> event.</summary>
	public class WizardPageInitEventArgs : WizardPageConfirmEventArgs
	{
		internal WizardPageInitEventArgs(WizardPage page, WizardPage prevPage)
			: base(page) => PreviousPage = prevPage;

		/// <summary>Gets the <see cref="WizardPage"/> that was previously selected when the event was raised.</summary>
		/// <value>The previous wizard page.</value>
		public WizardPage PreviousPage { get; }
	}
}