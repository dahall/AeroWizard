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
    internal class WizardControlDesigner : ParentControlDesigner, IToolboxUser
    {
        private static string[] propsToRemove = new string[] { "Anchor", "AutoScrollOffset", "AutoSize", "BackColor",
            "BackgroundImage", "BackgroundImageLayout", "ContextMenuStrip", "Cursor", "Dock", "Enabled", "Font",
            "ForeColor", /*"Location",*/ "Margin", "MaximumSize", "MinimumSize", "Padding", /*"Size",*/ "TabStop",
            "Text", "UseWaitCursor" };

        private WizardControlDesignerActionList actionList;
        private bool forwardOnDrag = false;
        private DesignerVerbCollection verbs;

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (actionList == null)
                    actionList = new WizardControlDesignerActionList(this);
                return new DesignerActionListCollection(new DesignerActionList[] { actionList });
            }
        }

        public override System.Collections.ICollection AssociatedComponents
        {
            get { return this.WizardControl.Pages; }
        }

        public override SelectionRules SelectionRules
        {
            get { return (SelectionRules.Visible | SelectionRules.Locked); }
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

        public virtual WizardControl WizardControl
        {
            get { return (this.Control as WizardControl); }
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
            get { return false; }
        }

        protected ISelectionService SelectionService
        {
            get { return (this.GetService(typeof(ISelectionService)) as ISelectionService); }
        }

        public override bool CanParent(Control control)
        {
            return ((control is WizardPage) && !this.Control.Contains(control));
        }

        public bool GetToolSupported(ToolboxItem tool)
        {
            if (tool.TypeName == "AeroWizard.WizardControl")
                return false;
            return (WizardControl != null && WizardControl.SelectedPage != null);
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
            WizardControl wc = this.WizardControl;
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
            this.Control.Text = Properties.Resources.WizardTitle;
        }

        public void ToolPicked(ToolboxItem tool)
        {
            if (tool.TypeName == "AeroWizard.WizardPage")
                InsertPageIntoWizard(true);
            if (WizardControl != null && WizardControl.SelectedPage != null)
                AddControlToActivePage(tool.TypeName);
        }

        internal static T GetParentOfComponent<T>(object component)
            where T : Control, new()
        {
            Control parent = component as Control;
            if (parent == null)
                return null;

            while (parent != null && !(parent is T))
                parent = parent.Parent;

            return parent as T;
        }

        internal void InsertPageIntoWizard(bool add)
        {
            IDesignerHost h = this.DesignerHost;
            IComponentChangeService c = this.ComponentChangeService;
            WizardControl wiz = this.Control as WizardControl;
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
                das.Refresh(this.WizardControl);
        }

        internal void RemovePageFromWizard(WizardPage page)
        {
            IDesignerHost h = this.DesignerHost;
            IComponentChangeService c = this.ComponentChangeService;
            if (h == null || c == null)
                throw new ArgumentNullException("Both IDesignerHost and IComponentChangeService arguments cannot be null.");

            if (WizardControl == null || !WizardControl.Pages.Contains(page))
                return;

            DesignerTransaction dt = null;
            try
            {
                dt = h.CreateTransaction("Remove Wizard Page");

                MemberDescriptor member = TypeDescriptor.GetProperties(this.WizardControl)["Pages"];
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
                this.WizardControl.SelectedPageChanged -= WizardControl_SelectedPageChanged;
                ISelectionService ss = SelectionService;
                if (ss != null)
                    ss.SelectionChanged -= OnSelectionChanged;
            }

            base.Dispose(disposing);
        }

        protected override bool GetHitTest(Point point)
        {
            if (this.WizardControl.nextButton.ClientRectangle.Contains(this.WizardControl.nextButton.PointToClient(point)))
                return true;
            if (this.WizardControl.backButton.ClientRectangle.Contains(this.WizardControl.backButton.PointToClient(point)))
                return true;
            return false;
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
                TabControl control = (TabControl)this.Control;
                Point pt = this.Control.PointToClient(new Point(de.X, de.Y));
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

        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
            base.OnPaintAdornments(pe);
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
            c.OnComponentChanging(WizardControl.SelectedPage, null);
            WizardControl.SelectedPage.Container.Add(comp);
            c.OnComponentChanged(WizardControl.SelectedPage, null, null, null);
            dt.Commit();
        }

        private void CheckStatus()
        {
            verbs[1].Enabled = (this.WizardControl != null && this.WizardControl.Pages.Count > 0);
            verbs[2].Enabled = (this.WizardControl != null && this.WizardControl.SelectedPage != null);
        }

        private WizardPageDesigner GetSelectedWizardPageDesigner()
        {
            if (WizardControl.SelectedPage != null)
            {
                IDesignerHost host = this.DesignerHost;
                if (host != null)
                    return host.GetDesigner(WizardControl.SelectedPage) as WizardPageDesigner;
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
            if (WizardControl != null && WizardControl.SelectedPage != null)
            {
                RemovePageFromWizard(WizardControl.SelectedPage);
                OnSelectionChanged(sender, e);
            }
        }

        private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
        {
            CheckStatus();
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            if (!(SelectionService.PrimarySelection is WizardControl))
            {
                WizardPage p = SelectionService.PrimarySelection as WizardPage;
                if (p == null)
                    p = GetParentOfComponent<WizardPage>(SelectionService.PrimarySelection);
                if (p != null && this.WizardControl.SelectedPage != p)
                {
                    this.WizardControl.SelectedPage = p;
                }
            }

            RefreshDesigner();
        }

        private void SelectComponent(Component p)
        {
            if (this.SelectionService != null)
            {
                this.SelectionService.SetSelectedComponents(new object[] { this.WizardControl }, SelectionTypes.Primary);
                if ((p != null) && (p.Site != null))
                    this.SelectionService.SetSelectedComponents(new object[] { p });
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
            SelectComponent(this.WizardControl.SelectedPage);
        }
    }

    internal class WizardControlDesignerActionList : DesignerActionList
    {
        private WizardControlDesigner wizardControlDesigner;

        public WizardControlDesignerActionList(WizardControlDesigner wizDesigner)
            : base(wizDesigner.Component)
        {
            wizardControlDesigner = wizDesigner;
            base.AutoShow = true;
        }

        public WizardPage GoToPage
        {
            get { return ((WizardControl)this.Component).SelectedPage; }
            set
            {
                if (value != null)
                    ((WizardControl)this.Component).SelectedPage = value;
            }
        }

        public WizardControl.WizardPageCollection Pages
        {
            get
            {
                WizardControl wiz = this.Component as WizardControl;
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
    }
}