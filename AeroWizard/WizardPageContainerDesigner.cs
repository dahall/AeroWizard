using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AeroWizard.Design
{
	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal class WizardBaseDesigner : ParentControlDesigner, IToolboxUser
	{
		private static string[] propsToRemove = new string[] { "AutoScrollOffset", "AutoSize", "BackColor",
			"BackgroundImage", "BackgroundImageLayout", "ContextMenuStrip", "Cursor", "Enabled", "Font",
			"ForeColor", /*"Location",*/ "MaximumSize", "MinimumSize", "Padding", /*"Size",*/ "TabStop",
			"Text", "UseWaitCursor" };

		private WizardBaseDesignerActionList actionList;
		private bool forwardOnDrag = false;
		private DesignerVerbCollection verbs;

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (actionList == null)
					actionList = new WizardBaseDesignerActionList(this);
				return new DesignerActionListCollection(new DesignerActionList[] { actionList });
			}
		}

		public override System.Collections.ICollection AssociatedComponents
		{
			get { return this.WizardPageContainer.Pages; }
		}

		public override SelectionRules SelectionRules
		{
			get { return (SelectionRules.Visible | SelectionRules.AllSizeable | SelectionRules.Moveable); }
		}

		public override DesignerVerbCollection Verbs
		{
			get
			{
				if (verbs == null)
				{
					verbs = new DesignerVerbCollection();
					verbs.Add(new DesignerVerb("Add Page", new EventHandler(handleAddPage)));
					verbs.Add(new DesignerVerb("Insert Page", new EventHandler(handleInsertPage)));
					verbs.Add(new DesignerVerb("Remove Page", new EventHandler(handleRemovePage)));
				}
				return verbs;
			}
		}

		public virtual WizardPageContainer WizardPageContainer
		{
			get { return (this.Control as WizardPageContainer); }
		}

		protected IComponentChangeService ComponentChangeService
		{
			get { return (this.GetService(typeof(IComponentChangeService)) as IComponentChangeService); }
		}

		protected IDesignerHost DesignerHost
		{
			get { return (this.GetService(typeof(IDesignerHost)) as IDesignerHost); }
		}

		protected override bool EnableDragRect
		{
			get { return true; }
		}

		protected ISelectionService SelectionService
		{
			get { return (this.GetService(typeof(ISelectionService)) as ISelectionService); }
		}

		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			return ((parentDesigner != null) && (parentDesigner.Component is Control));
		}

		public override bool CanParent(Control control)
		{
			return ((control is WizardPage) && !this.Control.Contains(control));
		}

		public bool GetToolSupported(ToolboxItem tool)
		{
			if (tool.TypeName == "AeroWizard.WizardPageContainer")
				return false;
			return (WizardPageContainer != null && WizardPageContainer.SelectedPage != null);
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			base.AutoResizeHandles = true;
			ISelectionService ss = SelectionService;
			if (ss != null)
				ss.SelectionChanged += OnSelectionChanged;
			IComponentChangeService ccs = ComponentChangeService;
			if (ccs != null)
				ccs.ComponentChanged += OnComponentChanged;
			WizardPageContainer wc = this.WizardPageContainer;
			if (wc != null)
			{
				wc.SelectedPageChanged += WizardPageContainer_SelectedPageChanged;
				//wc.GotFocus += WizardPageContainer_OnGotFocus;
				wc.ControlAdded += WizardPageContainer_OnControlAdded;
			}
		}

		public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			this.Control.Text = Properties.Resources.WizardTitle;
		}

		public void ToolPicked(ToolboxItem tool)
		{
			if (tool.TypeName == "AeroWizard.WizardPageContainer")
				InsertPageIntoWizard(true);
			if (WizardPageContainer != null && WizardPageContainer.SelectedPage != null)
				AddControlToActivePage(tool.TypeName);
		}

		internal void InsertPageIntoWizard(bool add)
		{
			IDesignerHost h = this.DesignerHost;
			IComponentChangeService c = this.ComponentChangeService;
			WizardPageContainer wiz = this.WizardPageContainer;
			if (h == null || c == null)
				throw new ArgumentNullException("Both IDesignerHost and IComponentChangeService arguments cannot be null.");

			DesignerTransaction dt = null;
			try
			{
				dt = h.CreateTransaction("Insert Wizard Page");
				WizardPage page = (WizardPage)h.CreateComponent(typeof(WizardPage));
				MemberDescriptor member = TypeDescriptor.GetProperties(wiz)["Pages"];
				base.RaiseComponentChanging(member);
				//c.OnComponentChanging(wiz, null);

				//Add a new page to the collection
				if (wiz.Pages.Count == 0 || add)
					wiz.Pages.Add(page);
				else
					wiz.Pages.Insert(wiz.SelectedPageIndex, page);

				//c.OnComponentChanged(wiz, null, null, null);
				base.RaiseComponentChanged(member, null, null);
			}
			finally
			{
				if (dt != null)
					dt.Commit();
			}
			RefreshDesigner();
		}

		internal void RefreshDesigner()
		{
			DesignerActionUIService das = GetService(typeof(DesignerActionUIService)) as DesignerActionUIService;
			if (das != null)
				das.Refresh(this.WizardPageContainer);
		}

		internal void RemovePageFromWizard(WizardPage page)
		{
			IDesignerHost h = this.DesignerHost;
			IComponentChangeService c = this.ComponentChangeService;
			if (h == null || c == null)
				throw new ArgumentNullException("Both IDesignerHost and IComponentChangeService arguments cannot be null.");

			if (WizardPageContainer == null || !WizardPageContainer.Pages.Contains(page))
				return;

			DesignerTransaction dt = null;
			try
			{
				dt = h.CreateTransaction("Remove Wizard Page");

				MemberDescriptor member = TypeDescriptor.GetProperties(this.WizardPageContainer)["Pages"];
				base.RaiseComponentChanging(member);

				if (page.Owner != null)
				{
					//c.OnComponentChanging(page.Owner, null);
					page.Owner.Pages.Remove(page);
					//c.OnComponentChanged(page.Owner, null, null, null);
					h.DestroyComponent(page);
				}
				else
				{
					//c.OnComponentChanging(page, null);
					page.Dispose();
					//c.OnComponentChanged(page, null, null, null);
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
				this.WizardPageContainer.SelectedPageChanged -= WizardPageContainer_SelectedPageChanged;
				ISelectionService ss = SelectionService;
				if (ss != null)
					ss.SelectionChanged -= OnSelectionChanged;
			}

			base.Dispose(disposing);
		}

		protected override void OnDragDrop(DragEventArgs de)
		{
			if (this.forwardOnDrag)
			{
				WizardPageDesigner wizPageDesigner = this.GetSelectedWizardPageDesigner();
				if (wizPageDesigner != null)
				{
					wizPageDesigner.OnDragDropInternal(de);
				}
			}
			else
			{
				base.OnDragDrop(de);
			}
			this.forwardOnDrag = false;
		}

		protected override void OnDragEnter(DragEventArgs de)
		{
			this.forwardOnDrag = true;
			WizardPageDesigner wizPageDesigner = this.GetSelectedWizardPageDesigner();
			if (wizPageDesigner != null)
			{
				wizPageDesigner.OnDragEnterInternal(de);
			}
		}

		protected override void OnDragLeave(EventArgs e)
		{
			if (this.forwardOnDrag)
			{
				WizardPageDesigner wizPageDesigner = this.GetSelectedWizardPageDesigner();
				if (wizPageDesigner != null)
				{
					wizPageDesigner.OnDragLeaveInternal(e);
				}
			}
			else
			{
				base.OnDragLeave(e);
			}
			this.forwardOnDrag = false;
		}

		protected override void OnDragOver(DragEventArgs de)
		{
			if (this.forwardOnDrag)
			{
				var control = this.WizardPageContainer;
				Point pt = control.PointToClient(new Point(de.X, de.Y));
				if (!control.DisplayRectangle.Contains(pt))
				{
					de.Effect = DragDropEffects.None;
				}
				else
				{
					WizardPageDesigner wizPageDesigner = this.GetSelectedWizardPageDesigner();
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
			if (this.forwardOnDrag)
			{
				WizardPageDesigner wizPageDesigner = this.GetSelectedWizardPageDesigner();
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

		protected override void PreFilterProperties(System.Collections.IDictionary properties)
		{
			base.PreFilterProperties(properties);
			foreach (string p in propsToRemove)
				if (properties.Contains(p))
					properties.Remove(p);
		}

		private void AddControlToActivePage(string typeName)
		{
			IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));
			IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));

			DesignerTransaction dt = h.CreateTransaction("Add Control");
			IComponent comp = h.CreateComponent(Type.GetType(typeName, false));
			c.OnComponentChanging(WizardPageContainer.SelectedPage, null);
			WizardPageContainer.SelectedPage.Container.Add(comp);
			c.OnComponentChanged(WizardPageContainer.SelectedPage, null, null, null);
			dt.Commit();
		}

		private void CheckStatus()
		{
			Verbs[1].Enabled = (this.WizardPageContainer != null && this.WizardPageContainer.Pages.Count > 0);
			Verbs[2].Enabled = (this.WizardPageContainer != null && this.WizardPageContainer.SelectedPage != null);
		}

		private WizardPageDesigner GetSelectedWizardPageDesigner()
		{
			if (WizardPageContainer.SelectedPage != null)
			{
				IDesignerHost host = this.DesignerHost;
				if (host != null)
					return host.GetDesigner(WizardPageContainer.SelectedPage) as WizardPageDesigner;
			}
			return null;
		}

		private void handleAddPage(object sender, EventArgs e)
		{
			InsertPageIntoWizard(true);
			OnSelectionChanged(sender, e);
		}

		private void handleEdit(object sender, EventArgs e)
		{
		}

		private void handleInsertPage(object sender, EventArgs e)
		{
			InsertPageIntoWizard(false);
		}

		private void handleRemovePage(object sender, EventArgs e)
		{
			if (WizardPageContainer != null && WizardPageContainer.SelectedPage != null)
			{
				RemovePageFromWizard(WizardPageContainer.SelectedPage);
				OnSelectionChanged(sender, e);
			}
		}

		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			CheckStatus();
		}

		private void OnSelectionChanged(object sender, EventArgs e)
		{
			if (!(SelectionService.PrimarySelection is WizardPageContainer))
			{
				WizardPage p = SelectionService.PrimarySelection as WizardPage;
				if (p == null && SelectionService.PrimarySelection is Control)
					p = ((Control)SelectionService.PrimarySelection).GetParent<WizardPage>();
				if (p != null && this.WizardPageContainer.SelectedPage != p)
				{
					this.WizardPageContainer.SelectedPage = p;
				}
			}

			RefreshDesigner();
		}

		private void SelectComponent(Component p)
		{
			if (this.SelectionService != null)
			{
				this.SelectionService.SetSelectedComponents(new object[] { this.WizardPageContainer }, SelectionTypes.Primary);
				if ((p != null) && (p.Site != null))
					this.SelectionService.SetSelectedComponents(new object[] { p });
				RefreshDesigner();
			}
		}

		/*private void WizardPageContainer_OnGotFocus(object sender, EventArgs e)
		{
			IEventHandlerService service = (IEventHandlerService)this.GetService(typeof(IEventHandlerService));
			if (service != null)
			{
				Control focusWindow = service.FocusWindow;
				if (focusWindow != null)
				{
					focusWindow.Focus();
				}
			}
		}*/
		private void WizardPageContainer_OnControlAdded(object sender, ControlEventArgs e)
		{
			if ((e.Control != null) && !e.Control.IsHandleCreated)
			{
				IntPtr handle = e.Control.Handle;
			}
		}

		void WizardPageContainer_SelectedPageChanged(object sender, EventArgs e)
		{
			SelectComponent(this.WizardPageContainer.SelectedPage);
		}
	}

	internal class WizardBaseDesignerActionList : DesignerActionList
	{
		private WizardBaseDesigner wizardControlDesigner;

		public WizardBaseDesignerActionList(WizardBaseDesigner wizDesigner)
			: base(wizDesigner.Component)
		{
			wizardControlDesigner = wizDesigner;
			base.AutoShow = true;
		}

		public WizardPage GoToPage
		{
			get { return ((WizardPageContainer)this.Component).SelectedPage; }
			set
			{
				if (value != null)
					((WizardPageContainer)this.Component).SelectedPage = value;
			}
		}

		public WizardPageCollection Pages
		{
			get
			{
				WizardPageContainer wiz = this.Component as WizardPageContainer;
				if (wiz != null)
					return wiz.Pages;
				return null;
			}
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection col = new DesignerActionItemCollection();
			col.Add(new DesignerActionPropertyItem("Pages", "Edit pages..."));
			col.Add(new DesignerActionMethodItem(this, "AddPage", "Add page", true));
			if (this.Pages != null && this.Pages.Count > 0)
			{
				col.Add(new DesignerActionMethodItem(this, "InsertPage", "Insert page", true));
				col.Add(new DesignerActionMethodItem(this, "NextPage", "Next page", true));
				col.Add(new DesignerActionMethodItem(this, "PrevPage", "Previous page", true));
				col.Add(new DesignerActionPropertyItem("GoToPage", "Go to page"));
			}
			return col;
		}

		private void AddPage()
		{
			wizardControlDesigner.InsertPageIntoWizard(true);
		}

		private void InsertPage()
		{
			wizardControlDesigner.InsertPageIntoWizard(false);
		}

		private void NextPage()
		{
			WizardPageContainer wiz = this.Component as WizardPageContainer;
			wiz.NextPage();
		}

		private void PrevPage()
		{
			WizardPageContainer wiz = this.Component as WizardPageContainer;
			wiz.PreviousPage();
		}
	}
}