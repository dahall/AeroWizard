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
	internal class WizardBaseDesigner : RichParentControlDesigner<WizardPageContainer, WizardBaseDesigner.ActionList>
#if NETFRAMEWORK
		, IToolboxUser
#endif
	{
		private static readonly string[] propsToRemove = new string[] { "AutoScrollOffset", "AutoSize", "BackColor",
			"BackgroundImage", "BackgroundImageLayout", "ContextMenuStrip", "Cursor", "Enabled", "Font",
			"ForeColor", /*"Location",*/ "MaximumSize", "MinimumSize", "Padding", /*"Size",*/ "TabStop",
			"Text", "UseWaitCursor" };

		private bool forwardOnDrag;

#if NETFRAMEWORK
		public override System.Collections.ICollection AssociatedComponents => Control.Pages;
#else
		public override System.Collections.Generic.IReadOnlyCollection<IComponent> AssociatedComponents => (System.Collections.Generic.IReadOnlyCollection<IComponent>)Control.Pages;
#endif
		public override SelectionRules SelectionRules => SelectionRules.Visible | SelectionRules.AllSizeable | SelectionRules.Moveable;

		protected IDesignerHost DesignerHost => GetService<IDesignerHost>();

		protected override bool EnableDragRect => true;

		protected override System.Collections.Generic.IEnumerable<string> PropertiesToRemove => propsToRemove;

		public override bool CanBeParentedTo(IDesigner parentDesigner) => parentDesigner?.Component is Control;

		public override bool CanParent(Control control) => control is WizardPage && !Control.Contains(control);

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			AutoResizeHandles = true;
			//base.Glyphs.Add(new WizardPageContainerDesignerGlyph(this));
			if (Control is null)
			{
				return;
			}

			Control.SelectedPageChanged += WizardPageContainer_SelectedPageChanged;
			//this.Control.GotFocus += WizardPageContainer_OnGotFocus;
			Control.ControlAdded += WizardPageContainer_OnControlAdded;
		}

		public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			Control.Text = Properties.Resources.WizardTitle;
		}

#if NETFRAMEWORK
		bool IToolboxUser.GetToolSupported(ToolboxItem tool)
		{
			if (tool.TypeName == typeof(WizardPage).FullName)
			{
				return false;
			}

			return Control?.SelectedPage is not null;
		}

		void IToolboxUser.ToolPicked(ToolboxItem tool)
		{
			if (tool.TypeName == typeof(WizardPage).FullName)
			{
				InsertPageIntoWizard(true);
			}

			if (Control?.SelectedPage is not null)
			{
				AddControlToActivePage(tool.TypeName);
			}
		}
#endif

		internal void InsertPageIntoWizard(bool add)
		{
			IDesignerHost h = DesignerHost;
			IComponentChangeService c = ComponentChangeService;
			WizardPageContainer wiz = Control;
			if (h is null || c is null)
			{
				throw new ArgumentException("Both IDesignerHost and IComponentChangeService arguments cannot be null.");
			}

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
					page.Owner.Pages.Remove(page);
					h.DestroyComponent(page);
				}
				else
				{
					page.Dispose();
				}
				RaiseComponentChanged(member, null, null);
			}
			finally
			{
				dt?.Commit();
			}
			RefreshDesigner();
		}

#if NETFRAMEWORK
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

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (Control is not null)
				{
					Control.SelectedPageChanged -= WizardPageContainer_SelectedPageChanged;
					//this.Control.GotFocus -= WizardPageContainer_OnGotFocus;
					Control.ControlAdded -= WizardPageContainer_OnControlAdded;
				}
			}
			base.Dispose(disposing);
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
				WizardPageContainer control = Control;
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
			if (SelectionService.PrimarySelection is not WizardPageContainer)
			{
				WizardPage p = SelectionService.PrimarySelection as WizardPage;
				if (p is null && SelectionService.PrimarySelection is Control)
				{
					p = ((Control)SelectionService.PrimarySelection).GetParent<WizardPage>();
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
			IComponentChangeService c = GetService<IComponentChangeService>();

			DesignerTransaction dt = DesignerHost?.CreateTransaction("Add Control");
			IComponent comp = DesignerHost?.CreateComponent(Type.GetType(typeName, false));
			if (comp is not null)
			{
				c.OnComponentChanging(Control.SelectedPage, null);
				Control.SelectedPage?.Container?.Add(comp);
				c.OnComponentChanged(Control.SelectedPage, null, null, null);
			}
			dt?.Commit();
		}

		private void CheckStatus()
		{
			if (Verbs.Count < 3)
			{
				return;
			}

			Verbs[1].Enabled = Control is not null && Control.Pages.Count > 0;
			Verbs[2].Enabled = Control?.SelectedPage is not null;
		}

		private WizardPageDesigner GetSelectedWizardPageDesigner() => Control.SelectedPage is not null ? DesignerHost?.GetDesigner(Control.SelectedPage) as WizardPageDesigner : null;

		[DesignerVerb("Remove page")]
		private void RemovePage(object sender, EventArgs e)
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
			if (SelectionService is not null)
			{
				SelectionService.SetSelectedComponents(new object[] { Control }, SelectionTypes.Primary);
				if (p?.Site is not null)
				{
					SelectionService.SetSelectedComponents(new object[] { p });
				}

				RefreshDesigner();
			}
		}

		private void WizardPageContainer_OnControlAdded(object sender, ControlEventArgs e)
		{
			/*if ((e.Control is not null) && !e.Control.IsHandleCreated)
			{
				var handle = e.Control.Handle;
			}*/
		}

		private void WizardPageContainer_SelectedPageChanged(object sender, EventArgs e) => SelectComponent(Control.SelectedPage);

		private void WizFirstPage(object sender, EventArgs e)
		{
			if (Control is not null && Control.Pages.Count > 0)
			{
				Control.SelectedPage = Control.Pages[0];
			}
		}

		private void WizLastPage(object sender, EventArgs e)
		{
			if (Control is not null && Control.Pages.Count > 0)
			{
				Control.SelectedPage = Control.Pages[Control.Pages.Count - 1];
			}
		}

		private void WizNextPage(object sender, EventArgs e) => Control?.NextPage();

		private void WizPrevPage(object sender, EventArgs e) => Control?.PreviousPage();

		internal class ActionList : RichDesignerActionList<WizardBaseDesigner, WizardPageContainer>
		{
			public ActionList(WizardBaseDesigner d, WizardPageContainer c) : base(d, c)
			{
			}

			[DesignerActionProperty("Go to page", 5)]
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

			internal bool HasPages => Pages is not null && Pages.Count > 0;

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
			private void NextPage() => Component.NextPage();

			[DesignerActionMethod("Previous page", 4, Condition = "HasPages")]
			private void PrevPage() => Component.PreviousPage();
		}
	}
}