using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace AeroWizard.Design
{
	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal class WizardBaseDesigner : RichParentControlDesigner<WizardPageContainer, WizardBaseDesigner.ActionList>, IToolboxUser
	{
		private static string[] propsToRemove = new string[] { "AutoScrollOffset", "AutoSize", "BackColor",
			"BackgroundImage", "BackgroundImageLayout", "ContextMenuStrip", "Cursor", "Enabled", "Font",
			"ForeColor", /*"Location",*/ "MaximumSize", "MinimumSize", "Padding", /*"Size",*/ "TabStop",
			"Text", "UseWaitCursor" };

		private bool forwardOnDrag = false;

		public WizardBaseDesigner()
		{
		}

		public override System.Collections.ICollection AssociatedComponents => Control.Pages;

		public override SelectionRules SelectionRules => (SelectionRules.Visible | SelectionRules.AllSizeable | SelectionRules.Moveable);

		protected IDesignerHost DesignerHost => GetService<IDesignerHost>();

		protected override bool EnableDragRect => true;

		protected override System.Collections.Generic.IEnumerable<string> PropertiesToRemove => propsToRemove;

		public override bool CanBeParentedTo(IDesigner parentDesigner) => ((parentDesigner != null) && (parentDesigner.Component is Control));

		public override bool CanParent(Control control) => ((control is WizardPage) && !Control.Contains(control));

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			base.AutoResizeHandles = true;
			//base.Glyphs.Add(new WizardPageContainerDesignerGlyph(this));
			if (Control != null)
			{
				Control.SelectedPageChanged += WizardPageContainer_SelectedPageChanged;
				//this.Control.GotFocus += WizardPageContainer_OnGotFocus;
				Control.ControlAdded += WizardPageContainer_OnControlAdded;
			}
		}

		public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			Control.Text = Properties.Resources.WizardTitle;
		}

		internal void InsertPageIntoWizard(bool add)
		{
			IDesignerHost h = DesignerHost;
			IComponentChangeService c = ComponentChangeService;
			WizardPageContainer wiz = Control;
			if (h == null || c == null)
				throw new ArgumentNullException("Both IDesignerHost and IComponentChangeService arguments cannot be null.");

			DesignerTransaction dt = null;
			try
			{
				dt = h.CreateTransaction("Insert Wizard Page");
				WizardPage page = (WizardPage)h.CreateComponent(typeof(WizardPage));
				MemberDescriptor member = TypeDescriptor.GetProperties(wiz)["Pages"];
				base.RaiseComponentChanging(member);

				//Add a new page to the collection
				if (wiz.Pages.Count == 0 || add)
					wiz.Pages.Add(page);
				else
					wiz.Pages.Insert(wiz.SelectedPageIndex, page);

				base.RaiseComponentChanged(member, null, null);
			}
			finally
			{
				if (dt != null)
					dt.Commit();
			}
			RefreshDesigner();
		}

		bool IToolboxUser.GetToolSupported(ToolboxItem tool)
		{
			if (tool.TypeName == "AeroWizard.Control")
				return false;
			return (Control != null && Control.SelectedPage != null);
		}

		void IToolboxUser.ToolPicked(ToolboxItem tool)
		{
			if (tool.TypeName == "AeroWizard.Control")
				InsertPageIntoWizard(true);
			if (Control != null && Control.SelectedPage != null)
				AddControlToActivePage(tool.TypeName);
		}

		internal void RefreshDesigner()
		{
			DesignerActionUIService das = GetService<DesignerActionUIService>();
			if (das != null)
				das.Refresh(Control);
		}

		internal void RemovePageFromWizard(WizardPage page)
		{
			IDesignerHost h = DesignerHost;
			IComponentChangeService c = ComponentChangeService;
			if (h == null || c == null)
				throw new ArgumentNullException("Both IDesignerHost and IComponentChangeService arguments cannot be null.");

			if (Control == null || !Control.Pages.Contains(page))
				return;

			DesignerTransaction dt = null;
			try
			{
				dt = h.CreateTransaction("Remove Wizard Page");

				MemberDescriptor member = TypeDescriptor.GetProperties(Control)["Pages"];
				base.RaiseComponentChanging(member);

				if (page.Owner != null)
				{
					page.Owner.Pages.Remove(page);
					h.DestroyComponent(page);
				}
				else
				{
					page.Dispose();
				}
				base.RaiseComponentChanged(member, null, null);
			}
			finally
			{
				if (dt != null)
					dt.Commit();
			}
			RefreshDesigner();
		}

		protected override IComponent[] CreateToolCore(ToolboxItem tool, int x, int y, int width, int height, bool hasLocation, bool hasSize)
		{
			WizardPageDesigner pageDes = GetSelectedWizardPageDesigner();
			if (pageDes != null)
				ParentControlDesigner.InvokeCreateTool(pageDes, tool);
			return null;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (Control != null)
				{
					Control.SelectedPageChanged -= WizardPageContainer_SelectedPageChanged;
					//this.Control.GotFocus -= WizardPageContainer_OnGotFocus;
					Control.ControlAdded -= WizardPageContainer_OnControlAdded;
				}
			}
			base.Dispose(disposing);
		}

		protected override void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			CheckStatus();
		}

		protected override void OnDragDrop(DragEventArgs de)
		{
			if (forwardOnDrag)
			{
				WizardPageDesigner wizPageDesigner = GetSelectedWizardPageDesigner();
				if (wizPageDesigner != null)
				{
					wizPageDesigner.OnDragDropInternal(de);
				}
			}
			else
			{
				base.OnDragDrop(de);
			}
			forwardOnDrag = false;
		}

		protected override void OnDragEnter(DragEventArgs de)
		{
			forwardOnDrag = true;
			WizardPageDesigner wizPageDesigner = GetSelectedWizardPageDesigner();
			if (wizPageDesigner != null)
			{
				wizPageDesigner.OnDragEnterInternal(de);
			}
		}

		protected override void OnDragLeave(EventArgs e)
		{
			if (forwardOnDrag)
			{
				WizardPageDesigner wizPageDesigner = GetSelectedWizardPageDesigner();
				if (wizPageDesigner != null)
				{
					wizPageDesigner.OnDragLeaveInternal(e);
				}
			}
			else
			{
				base.OnDragLeave(e);
			}
			forwardOnDrag = false;
		}

		protected override void OnDragOver(DragEventArgs de)
		{
			if (forwardOnDrag)
			{
				var control = Control;
				Point pt = control.PointToClient(new Point(de.X, de.Y));
				if (!control.DisplayRectangle.Contains(pt))
				{
					de.Effect = DragDropEffects.None;
				}
				else
				{
					WizardPageDesigner wizPageDesigner = GetSelectedWizardPageDesigner();
					if (wizPageDesigner != null)
					{
						wizPageDesigner.OnDragOverInternal(de);
					}
				}
			}
			else
			{
				base.OnDragOver(de);
			}
		}

		protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
			if (forwardOnDrag)
			{
				WizardPageDesigner wizPageDesigner = GetSelectedWizardPageDesigner();
				if (wizPageDesigner != null)
				{
					wizPageDesigner.OnGiveFeedbackInternal(e);
				}
			}
			else
			{
				base.OnGiveFeedback(e);
			}
		}

		protected override void OnSelectionChanged(object sender, EventArgs e)
		{
			if (!(SelectionService.PrimarySelection is WizardPageContainer))
			{
				WizardPage p = SelectionService.PrimarySelection as WizardPage;
				if (p == null && SelectionService.PrimarySelection is Control)
					p = ((Control)SelectionService.PrimarySelection).GetParent<WizardPage>();
				if (p != null && Control.SelectedPage != p)
				{
					Control.SelectedPage = p;
				}
			}

			RefreshDesigner();
		}

		private void AddControlToActivePage(string typeName)
		{
			IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));
			IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));

			DesignerTransaction dt = h.CreateTransaction("Add Control");
			IComponent comp = h.CreateComponent(Type.GetType(typeName, false));
			c.OnComponentChanging(Control.SelectedPage, null);
			Control.SelectedPage.Container.Add(comp);
			c.OnComponentChanged(Control.SelectedPage, null, null, null);
			dt.Commit();
		}

		private void CheckStatus()
		{
			if (Verbs.Count >= 3)
			{
				Verbs[1].Enabled = (Control != null && Control.Pages.Count > 0);
				Verbs[2].Enabled = (Control != null && Control.SelectedPage != null);
			}
		}

		private WizardPageDesigner GetSelectedWizardPageDesigner()
		{
			if (Control.SelectedPage != null)
			{
				IDesignerHost host = DesignerHost;
				if (host != null)
					return host.GetDesigner(Control.SelectedPage) as WizardPageDesigner;
			}
			return null;
		}

		[DesignerVerb("Remove page")]
		private void RemovePage(object sender, EventArgs e)
		{
			if (Control != null && Control.SelectedPage != null)
			{
				RemovePageFromWizard(Control.SelectedPage);
				OnSelectionChanged(sender, e);
			}
		}

		private void SelectComponent(Component p)
		{
			if (SelectionService != null)
			{
				SelectionService.SetSelectedComponents(new object[] { Control }, SelectionTypes.Primary);
				if ((p != null) && (p.Site != null))
					SelectionService.SetSelectedComponents(new object[] { p });
				RefreshDesigner();
			}
		}

		private void WizardPageContainer_OnControlAdded(object sender, ControlEventArgs e)
		{
			if ((e.Control != null) && !e.Control.IsHandleCreated)
			{
				IntPtr handle = e.Control.Handle;
			}
		}

		private void WizardPageContainer_SelectedPageChanged(object sender, EventArgs e)
		{
			SelectComponent(Control.SelectedPage);
		}

		private void WizFirstPage(object sender, EventArgs e)
		{
			if (Control != null && Control.Pages.Count > 0)
				Control.SelectedPage = Control.Pages[0];
		}

		private void WizLastPage(object sender, EventArgs e)
		{
			if (Control != null && Control.Pages.Count > 0)
				Control.SelectedPage = Control.Pages[Control.Pages.Count - 1];
		}

		private void WizNextPage(object sender, EventArgs e)
		{
			if (Control != null)
				Control.NextPage();
		}

		private void WizPrevPage(object sender, EventArgs e)
		{
			if (Control != null)
				Control.PreviousPage();
		}

		internal class ActionList : RichDesignerActionList<WizardBaseDesigner, WizardPageContainer>
		{
			public ActionList(WizardBaseDesigner d, WizardPageContainer c) : base(d, c)
			{
			}

			[DesignerActionProperty("Go to page", 5)]
			public WizardPage GoToPage
			{
				get { return Component.SelectedPage; }
				set
				{
					if (value != null)
						Component.SelectedPage = value;
				}
			}

			[DesignerActionProperty("Edit pages...", 0)]
			public WizardPageCollection Pages
			{
				get
				{
					if (Component != null)
						return Component.Pages;
					return null;
				}
			}

			internal bool HasPages => (Pages != null && Pages.Count > 0);

			[DesignerActionMethod("Add page", 1, IncludeAsDesignerVerb = true)]
			private void AddPage()
			{
				ParentDesigner.InsertPageIntoWizard(true);
				ParentDesigner.OnSelectionChanged(this, EventArgs.Empty);
			}

			[DesignerActionMethod("Insert page", 2, Condition = "HasPages", IncludeAsDesignerVerb = true)]
			private void InsertPage()
			{
				ParentDesigner.InsertPageIntoWizard(false);
				ParentDesigner.OnSelectionChanged(this, EventArgs.Empty);
			}


			[DesignerActionMethod("Next page", 3, Condition = "HasPages")]
			private void NextPage()
			{
				WizardPageContainer wiz = Component as WizardPageContainer;
				Component.NextPage();
			}

			[DesignerActionMethod("Previous page", 4, Condition = "HasPages")]
			private void PrevPage()
			{
				WizardPageContainer wiz = Component as WizardPageContainer;
				Component.PreviousPage();
			}
		}
	}
}