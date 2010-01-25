using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AeroWizard
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class WizardControlDesigner : ParentControlDesigner, IToolboxUser
    {
        private static string[] propsToRemove = new string[] { "Anchor", "AutoScrollOffset", "AutoSize", "BackColor",
            "BackgroundImage", "BackgroundImageLayout", "ContextMenuStrip", "Cursor", "Dock", "Enabled", "Font",
            "ForeColor", /*"Location",*/ "Margin", "MaximumSize", "MinimumSize", "Padding", /*"Size",*/ "TabStop",
            "Text", "UseWaitCursor" };

        private DesignerVerbCollection verbs;

        /*private enum WizardElement
        {
            NoElement,
            BackButton,
            Title,
            TitleIcon,
            HeaderText,
            ContentArea,
            NextButton,
            CancelButton
        }*/

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
                    //verbs.Add(new DesignerVerb("Edit All Pages", new EventHandler(handleEdit)));
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

        protected bool IsSelected
        {
            get
            {
                if (this.SelectionService == null)
                    return false;

                if (this.SelectionService.GetComponentSelected(this.WizardControl))
                    return true;

                WizardPage primarySelection = this.SelectionService.PrimarySelection as WizardPage;
                return ((primarySelection != null) && (primarySelection.Owner == this.WizardControl));
            }
        }

        protected ISelectionService SelectionService
        {
            get { return (this.GetService(typeof(ISelectionService)) as ISelectionService); }
        }

        public override bool CanParent(Control control)
        {
            return (control is WizardPage);
        }

        public override bool CanParent(ControlDesigner controlDesigner)
        {
            return (controlDesigner is WizardPageDesigner);
        }

        public bool GetToolSupported(ToolboxItem tool)
        {
            if (tool.TypeName == "AeroWizard.WizardControl")
                return false;
            return (WizardControl != null && WizardControl.SelectedPage != null);
        }

        public override void Initialize(System.ComponentModel.IComponent component)
        {
            base.Initialize(component);
            this.WizardControl.SelectedPageChanged += WizardControl_SelectedPageChanged;
            ISelectionService ss = SelectionService;
            if (ss != null)
                ss.SelectionChanged += OnSelectionChanged;
        }

        public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            this.Control.Text = Properties.Resources.WizardTitle;
            /*WizardPage page = new WizardPage();
            InsertPage(0, page);
            if (page != null)
                this.WizardControl.SelectedPage = page;*/
        }

        public void ToolPicked(ToolboxItem tool)
        {
            if (tool.TypeName == "AeroWizard.WizardPage")
                InsertPageIntoWizard(this.DesignerHost, this.ComponentChangeService, this.WizardControl, true);
            if (WizardControl != null && WizardControl.SelectedPage != null)
                AddControlToActivePage(tool.TypeName);
        }

        internal static void InsertPageIntoWizard(IDesignerHost h, IComponentChangeService c, WizardControl wiz, bool add)
        {
            if (h == null || c == null)
                throw new ArgumentNullException("Both IDesignerHost and IComponentChangeService arguments cannot be null.");

            DesignerTransaction dt = h.CreateTransaction("Insert Page");
            WizardPage page = (WizardPage)h.CreateComponent(typeof(WizardPage));
            c.OnComponentChanging(wiz, null);

            //Add a new page to the collection
            if (wiz.Pages.Count == 0 || add)
                wiz.Pages.Add(page);
            else
                wiz.Pages.Insert(wiz.SelectedPageIndex, page);

            c.OnComponentChanged(wiz, null, null, null);
            dt.Commit();
        }

        internal static void RemovePageFromWizard(IDesignerHost h, IComponentChangeService c, WizardPage page)
        {
            if (h == null || c == null)
                throw new ArgumentNullException("Both IDesignerHost and IComponentChangeService arguments cannot be null.");

            DesignerTransaction dt = h.CreateTransaction("Remove Page");

            if (page.Owner != null)
            {
                c.OnComponentChanging(page.Owner, null);
                page.Owner.Pages.Remove(page);
                c.OnComponentChanged(page.Owner, null, null, null);
                h.DestroyComponent(page);
            }
            else
            {
                c.OnComponentChanging(page, null);
                page.Dispose();
                c.OnComponentChanged(page, null, null, null);
            }

            dt.Commit();
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
            if (!this.IsSelected)
                return false;

            if (this.WizardControl.nextButton.ClientRectangle.Contains(this.WizardControl.nextButton.PointToClient(point)))
                return true;
            if (this.WizardControl.backButton.ClientRectangle.Contains(this.WizardControl.backButton.PointToClient(point)))
                return true;
            /*Control ctrl = this.WizardControl.GetChildAtPoint(this.WizardControl.PointToClient(point));
            switch (ctrl.Name)
            {
                case "nextButton":
                    e = WizardElement.NextButton;
                    break;
                case "backButton":
                    e = WizardElement.BackButton;
                    break;
            }

            if ((e != WizardElement.NextButton) && (e != WizardElement.BackButton))
                return false; // this.RefreshButtons(false);

            return true;// this.RefreshButtons(true);*/

            return false;
        }

        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
			base.OnPaintAdornments(pe);
			/*if (WizardControl.Pages.Count == 0)
            {
                string noPagesText = Properties.Resources.WizardNoPagesNotice;
				Rectangle r = WizardControl.GetContentAreaRectangle(true);

                r.Inflate(-2, -2);
				//pe.Graphics.DrawRectangle(SystemPens.GrayText, r);
				ControlPaint.DrawFocusRectangle(pe.Graphics, r);

                SizeF textSize = pe.Graphics.MeasureString(noPagesText, WizardControl.Font);
                r.Inflate((r.Width - (int)textSize.Width) / -2, (r.Height - (int)textSize.Height) / -2);
                pe.Graphics.DrawString(noPagesText, WizardControl.Font, new SolidBrush(SystemColors.GrayText), r);
            }*/
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

        private void handleAddPage(object sender, EventArgs e)
        {
            InsertPageIntoWizard(this.DesignerHost, this.ComponentChangeService, this.WizardControl, true);
			OnSelectionChanged(sender, e);
        }

        private void handleEdit(object sender, EventArgs e)
        {
            PropertyDescriptor pd = TypeDescriptor.GetProperties(WizardControl)["Pages"];
            UITypeEditor editor = (UITypeEditor)pd.GetEditor(typeof(UITypeEditor));
            IServiceProvider sp = this.GetService(typeof(IServiceProvider)) as IServiceProvider;
            editor.EditValue(sp, WizardControl.Pages);
        }

        private void handleInsertPage(object sender, EventArgs e)
        {
            InsertPageIntoWizard(this.DesignerHost, this.ComponentChangeService, this.WizardControl, false);
        }

        private void handleRemovePage(object sender, EventArgs e)
        {
			if (WizardControl != null && WizardControl.SelectedPage != null)
			{
				RemovePageFromWizard(this.DesignerHost, this.ComponentChangeService, WizardControl.SelectedPage);
				OnSelectionChanged(sender, e);
			}
		}

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            Control c = SelectionService.PrimarySelection as Control;
			verbs[1].Enabled = (this.WizardControl != null && this.WizardControl.Pages.Count > 0);
			verbs[2].Enabled = (this.WizardControl != null && this.WizardControl.SelectedPage != null);
		}

		private void SelectComponent(Component p)
		{
			if (this.SelectionService != null)
			{
				this.SelectionService.SetSelectedComponents(new object[] { this.WizardControl }, SelectionTypes.Primary);
				if ((p != null) && (p.Site != null))
					this.SelectionService.SetSelectedComponents(new object[] { p });
			}
		}

        void WizardControl_SelectedPageChanged(object sender, EventArgs e)
        {
			SelectComponent(this.WizardControl.SelectedPage);
        }
    }
}