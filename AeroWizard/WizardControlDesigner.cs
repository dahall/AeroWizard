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
	internal class WizardControlDesigner : RichParentControlDesigner<WizardControl, WizardControlDesignerActionList>, IToolboxUser
	{
		private static string[] propsToRemove = new string[] { "Anchor", "AutoScrollOffset", "AutoSize", "BackColor",
			"BackgroundImage", "BackgroundImageLayout", "ContextMenuStrip", "Cursor", "Dock", "Enabled", "Font",
			"ForeColor", /*"Location",*/ "Margin", "MaximumSize", "MinimumSize", "Padding", /*"Size",*/ "TabStop",
			"Text", "UseWaitCursor" };

		private bool forwardOnDrag = false;

		public override System.Collections.ICollection AssociatedComponents => (Control is WizardControl) ? Control.Pages : base.AssociatedComponents;

		public override SelectionRules SelectionRules => (SelectionRules.Visible | SelectionRules.Locked);

		protected IDesignerHost DesignerHost => GetService<IDesignerHost>();

		protected override bool EnableDragRect => false;

		public override bool CanBeParentedTo(IDesigner parentDesigner) => ((parentDesigner != null) && (parentDesigner.Component is Form));

		public override bool CanParent(Control control) => ((control is WizardPage) && !Control.Contains(control));

		public bool GetToolSupported(ToolboxItem tool)
		{
			if (tool.TypeName == "AeroWizard.WizardControl")
				return false;
			return (Control != null && Control.SelectedPage != null);
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			base.AutoResizeHandles = true;
			WizardControl wc = Control;
			if (wc != null)
			{
				wc.SelectedPageChanged += WizardControl_SelectedPageChanged;
				//wc.GotFocus += WizardControl_OnGotFocus;
				wc.ControlAdded += WizardControl_OnControlAdded;
			}
		}

		public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			Control.Text = Properties.Resources.WizardTitle;
		}

		public void ToolPicked(ToolboxItem tool)
		{
			if (tool.TypeName == "AeroWizard.WizardPage")
				InsertPageIntoWizard(true);
			if (Control != null && Control.SelectedPage != null)
				AddControlToActivePage(tool.TypeName);
		}

		internal void InsertPageIntoWizard(bool add)
		{
			IDesignerHost h = DesignerHost;
			IComponentChangeService c = ComponentChangeService;
			WizardControl wiz = Control as WizardControl;
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

		internal void RefreshDesigner()
		{
			DesignerActionUIService das = GetService(typeof(DesignerActionUIService)) as DesignerActionUIService;
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
				Control.SelectedPageChanged -= WizardControl_SelectedPageChanged;
				ISelectionService ss = SelectionService;
				if (ss != null)
					ss.SelectionChanged -= OnSelectionChanged;
			}

			base.Dispose(disposing);
		}

		protected override bool GetHitTest(Point point)
		{
			if (Control.nextButton.ClientRectangle.Contains(Control.nextButton.PointToClient(point)))
				return true;
			if (Control.backButton.ClientRectangle.Contains(Control.backButton.PointToClient(point)))
				return true;
			return false;
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
			Verbs[1].Enabled = (Control != null && Control.Pages.Count > 0);
			Verbs[2].Enabled = (Control != null && Control.SelectedPage != null);
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

		[DesignerVerb("Add Page")]
		private void handleAddPage(object sender, EventArgs e)
		{
			InsertPageIntoWizard(true);
			OnSelectionChanged(sender, e);
		}

		[DesignerVerb("Insert Page")]
		private void handleInsertPage(object sender, EventArgs e)
		{
			InsertPageIntoWizard(false);
		}

		[DesignerVerb("Remove Page")]
		private void handleRemovePage(object sender, EventArgs e)
		{
			if (Control != null && Control.SelectedPage != null)
			{
				RemovePageFromWizard(Control.SelectedPage);
				OnSelectionChanged(sender, e);
			}
		}

		protected override void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			CheckStatus();
		}

		protected override void OnSelectionChanged(object sender, EventArgs e)
		{
			if (!(SelectionService.PrimarySelection is WizardControl))
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

		/*private void WizardControl_OnGotFocus(object sender, EventArgs e)
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

		private void WizardControl_OnControlAdded(object sender, ControlEventArgs e)
		{
			if ((e.Control != null) && !e.Control.IsHandleCreated)
			{
				IntPtr handle = e.Control.Handle;
			}
		}

		void WizardControl_SelectedPageChanged(object sender, EventArgs e)
		{
			SelectComponent(Control.SelectedPage);
		}
	}

	internal class WizardControlDesignerActionList : RichDesignerActionList<WizardControlDesigner, WizardControl>
	{
		public WizardControlDesignerActionList(WizardControlDesigner wizDesigner, WizardControl control)
			: base(wizDesigner, control)
		{
		}

		[DesignerActionProperty("Go to page", 4, Condition = "HasPages")]
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
				WizardControl wiz = Component;
				if (wiz != null)
					return wiz.Pages;
				return null;
			}
		}

		private bool HasPages => (Pages != null && Pages.Count > 0);

		[DesignerActionMethod("Add page", 1)]
		private void AddPage()
		{
			ParentDesigner.InsertPageIntoWizard(true);
		}

		[DesignerActionMethod("Insert page", 2, Condition = "HasPages")]
		private void InsertPage()
		{
			ParentDesigner.InsertPageIntoWizard(false);
		}
	}
}