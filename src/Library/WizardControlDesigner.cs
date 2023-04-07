#if !NETFRAMEWORK
using Microsoft.DotNet.DesignTools.Designers;
using Microsoft.DotNet.DesignTools.Designers.Actions;
using Microsoft.DotNet.DesignTools.Designers.Behaviors;
using Microsoft.DotNet.DesignTools.Editors;
#else
using System.Drawing.Design;
using System.Windows.Forms.Design.Behavior;
#endif
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AeroWizard.Design
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal class WizardControlDesigner : RichParentControlDesigner<WizardControl, WizardControlDesignerActionList>
#if NETFRAMEWORK
		, IToolboxUser
#endif
	{
		private static readonly string[] propsToRemove = new string[] { "Anchor", "AutoScrollOffset", "AutoSize", "BackColor",
			"BackgroundImage", "BackgroundImageLayout", "ContextMenuStrip", "Cursor", "Dock", "Enabled", "Font",
			"ForeColor", /*"Location",*/ "Margin", "MaximumSize", "MinimumSize", "Padding", /*"Size",*/ "TabStop",
			"Text", "UseWaitCursor" };

		private bool forwardOnDrag;

#if NETFRAMEWORK
		public override System.Collections.ICollection AssociatedComponents => Control.Pages ?? base.AssociatedComponents;
#else
		public override System.Collections.Generic.IReadOnlyCollection<IComponent> AssociatedComponents => (System.Collections.Generic.IReadOnlyCollection<IComponent>)Control.Pages ?? base.AssociatedComponents;
#endif

		public override SelectionRules SelectionRules => SelectionRules.Visible | SelectionRules.Locked;

		protected IDesignerHost DesignerHost => GetService<IDesignerHost>();

		protected override bool EnableDragRect => false;

		protected override System.Collections.Generic.IEnumerable<string> PropertiesToRemove => propsToRemove;

		public override bool CanBeParentedTo(IDesigner parentDesigner) => parentDesigner?.Component is ContainerControl;

		public override bool CanParent(Control control) => control is WizardPage && !Control.Contains(control);

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			AutoResizeHandles = true;
			WizardControl wc = Control;
			if (wc is null)
			{
				return;
			}

			wc.SelectedPageChanged += WizardControl_SelectedPageChanged;
			//wc.GotFocus += WizardControl_OnGotFocus;
			wc.ControlAdded += WizardControl_OnControlAdded;
		}

		public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			Control.Text = Properties.Resources.WizardTitle;
		}

#if NETFRAMEWORK
		public bool GetToolSupported(ToolboxItem tool) => tool.TypeName != typeof(WizardControl).FullName && Control?.SelectedPage is not null;

		public void ToolPicked(ToolboxItem tool)
		{
			if (tool.TypeName == "AeroWizard.WizardPage")
			{
				InsertPageIntoWizard(true);
			}

			if (Control?.SelectedPage is not null)
			{
				AddControlToActivePage(tool.TypeName);
			}
		}

		protected override IComponent[] CreateToolCore(ToolboxItem tool, int x, int y, int width, int height, bool hasLocation, bool hasSize)
		{
			WizardPageDesigner pageDes = GetSelectedWizardPageDesigner();
			if (pageDes is not null)
			{
				InvokeCreateTool(pageDes, tool);
			}

			return null;
		}
#endif

		internal void InsertPageIntoWizard(bool add)
		{
			IDesignerHost h = DesignerHost;
			WizardControl wiz = Control;
			DesignerTransaction dt = null;
			try
			{
				dt = h.CreateTransaction("Insert Wizard Page");
				WizardPage page = (WizardPage)h.CreateComponent(typeof(WizardPage));
				MemberDescriptor member = TypeDescriptor.GetProperties(wiz)["Pages"];
				RaiseComponentChanging(member);

				//Add a new page to the collection
				if (wiz.Pages.Count == 0 || add)
				{
					wiz.Pages.Add(page);
				}
				else
				{
					wiz.Pages.Insert(wiz.SelectedPageIndex, page);
				}

				RaiseComponentChanged(member, null, null);
			}
			finally
			{
				dt?.Commit();
			}
			RefreshDesigner();
		}

		internal void RefreshDesigner()
		{
			DesignerActionUIService das = GetService<DesignerActionUIService>();
			das?.Refresh(Control);
		}

		internal void RemovePageFromWizard(WizardPage page)
		{
			IDesignerHost h = DesignerHost;
			IComponentChangeService c = ComponentChangeService;
			if (h is null || c is null)
			{
				throw new ArgumentException("Both IDesignerHost and IComponentChangeService arguments cannot be null.");
			}

			if (Control is null || !Control.Pages.Contains(page))
			{
				return;
			}

			DesignerTransaction dt = null;
			try
			{
				dt = h.CreateTransaction("Remove Wizard Page");

				MemberDescriptor member = TypeDescriptor.GetProperties(Control)["Pages"];
				RaiseComponentChanging(member);

				if (page.Owner is not null)
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
				RaiseComponentChanged(member, null, null);
			}
			finally
			{
				dt?.Commit();
			}
			RefreshDesigner();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Control.SelectedPageChanged -= WizardControl_SelectedPageChanged;
				ISelectionService ss = SelectionService;
				if (ss is not null)
				{
					ss.SelectionChanged -= OnSelectionChanged;
				}
			}

			base.Dispose(disposing);
		}

		protected override bool GetHitTest(Point point)
		{
			if (Control.nextButton.ClientRectangle.Contains(Control.nextButton.PointToClient(point)))
			{
				return true;
			}

			return Control.backButton.ClientRectangle.Contains(Control.backButton.PointToClient(point));
		}

		protected override void OnComponentChanged(object sender, ComponentChangedEventArgs e) => CheckStatus();

		protected override void OnDragDrop(DragEventArgs de)
		{
			if (forwardOnDrag)
			{
				WizardPageDesigner wizPageDesigner = GetSelectedWizardPageDesigner();
				wizPageDesigner?.OnDragDropInternal(de);
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
			wizPageDesigner?.OnDragEnterInternal(de);
		}

		protected override void OnDragLeave(EventArgs e)
		{
			if (forwardOnDrag)
			{
				WizardPageDesigner wizPageDesigner = GetSelectedWizardPageDesigner();
				wizPageDesigner?.OnDragLeaveInternal(e);
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
				WizardControl control = Control;
				Point pt = control.PointToClient(new Point(de.X, de.Y));
				if (!control.DisplayRectangle.Contains(pt))
				{
					de.Effect = DragDropEffects.None;
				}
				else
				{
					WizardPageDesigner wizPageDesigner = GetSelectedWizardPageDesigner();
					wizPageDesigner?.OnDragOverInternal(de);
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
				wizPageDesigner?.OnGiveFeedbackInternal(e);
			}
			else
			{
				base.OnGiveFeedback(e);
			}
		}

		protected override void OnSelectionChanged(object sender, EventArgs e)
		{
			if (SelectionService.PrimarySelection is not WizardControl)
			{
				WizardPage p = SelectionService.PrimarySelection as WizardPage;
				if (p is null && SelectionService.PrimarySelection is Control control)
				{
					p = control.GetParent<WizardPage>();
				}

				if (p is not null && Control.SelectedPage != p)
				{
					Control.SelectedPage = p;
				}
			}

			RefreshDesigner();
		}

		private void AddControlToActivePage(string typeName)
		{
			DesignerTransaction dt = DesignerHost?.CreateTransaction("Add Control");
			IComponent comp = DesignerHost?.CreateComponent(Type.GetType(typeName, false));
			if (comp is not null)
			{
				IComponentChangeService c = GetService<IComponentChangeService>();
				c.OnComponentChanging(Control.SelectedPage, null);
				Control.SelectedPage?.Container?.Add(comp);
				c.OnComponentChanged(Control.SelectedPage, null, null, null);
			}
			dt?.Commit();
		}

		private void CheckStatus()
		{
			Verbs[1].Enabled = Control is not null && Control.Pages.Count > 0;
			Verbs[2].Enabled = Control?.SelectedPage is not null;
		}

		private WizardPageDesigner GetSelectedWizardPageDesigner()
		{
			if (Control.SelectedPage is null)
			{
				return null;
			}

			return DesignerHost?.GetDesigner(Control.SelectedPage) as WizardPageDesigner;
		}

		[DesignerVerb("Add Page")]
		private void HandleAddPage(object sender, EventArgs e)
		{
			InsertPageIntoWizard(true);
			OnSelectionChanged(sender, e);
		}

		[DesignerVerb("Insert Page")]
		private void HandleInsertPage(object sender, EventArgs e) => InsertPageIntoWizard(false);

		[DesignerVerb("Remove Page")]
		private void HandleRemovePage(object sender, EventArgs e)
		{
			if (Control?.SelectedPage is null)
			{
				return;
			}

			RemovePageFromWizard(Control.SelectedPage);
			OnSelectionChanged(sender, e);
		}

		private void SelectComponent(Component p)
		{
			if (SelectionService is null)
			{
				return;
			}

			SelectionService.SetSelectedComponents(new object[] { Control }, SelectionTypes.Primary);
			if (p?.Site is not null)
			{
				SelectionService.SetSelectedComponents(new object[] { p });
			}

			RefreshDesigner();
		}

		/*private void WizardControl_OnGotFocus(object sender, EventArgs e)
		{
			IEventHandlerService service = (IEventHandlerService)this.GetService(typeof(IEventHandlerService));
			if (service is not null)
			{
				Control focusWindow = service.FocusWindow;
				if (focusWindow is not null)
				{
					focusWindow.Focus();
				}
			}
		}*/

		private void WizardControl_OnControlAdded(object sender, ControlEventArgs e)
		{
			/*if ((e.Control is not null) && !e.Control.IsHandleCreated)
			{
				var handle = e.Control.Handle;
			}*/
		}

		private void WizardControl_SelectedPageChanged(object sender, EventArgs e) => SelectComponent(Control.SelectedPage);
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
			get => Component.SelectedPage;
			set
			{
				if (value is not null)
				{
					Component.SelectedPage = value;
				}
			}
		}

		[DesignerActionProperty("Edit pages...")]
		public WizardPageCollection Pages => Component?.Pages;

		private bool HasPages => Pages is not null && Pages.Count > 0;

		[DesignerActionMethod("Add page", 1)]
		private void AddPage() => ParentDesigner.InsertPageIntoWizard(true);

		[DesignerActionMethod("Insert page", 2, Condition = "HasPages")]
		private void InsertPage() => ParentDesigner.InsertPageIntoWizard(false);
	}
}