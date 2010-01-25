using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AeroWizard
{
    public class WizardPageDesigner : ParentControlDesigner
    {
        private static string[] propsToRemove = new string[] { "Anchor", "AutoScrollOffset", "AutoSize", "BackColor",
            "BackgroundImage", "BackgroundImageLayout", "ContextMenuStrip", "Cursor", "Dock", "Enabled", "Font",
			"ForeColor", "Location", "Margin", "MaximumSize", "MinimumSize", "Padding", "Size", "TabStop", "UseWaitCursor" };

        private DesignerVerbCollection verbs;

        public virtual WizardPage Page
        {
            get
            {
                return (this.Control as WizardPage);
            }
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                return (SelectionRules.Visible | SelectionRules.Locked);
            }
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (verbs == null)
                {
                    verbs = new DesignerVerbCollection();
                    verbs.Add(new DesignerVerb("Remove Page", new EventHandler(handleRemovePage)));
                }
                return verbs;
            }
        }

        public virtual WizardControl WizardControl
        {
            get
            {
                return (this.Page == null) ? null : this.Page.Owner;
            }
        }

		protected ISelectionService SelectionService
		{
			get { return (this.GetService(typeof(ISelectionService)) as ISelectionService); }
		}

		protected IDesignerHost DesignerHost
		{
			get { return (this.GetService(typeof(IDesignerHost)) as IDesignerHost); }
		}

		protected IComponentChangeService ComponentChangeService
		{
			get { return (this.GetService(typeof(IComponentChangeService)) as IComponentChangeService); }
		}

        public override void Initialize(System.ComponentModel.IComponent component)
        {
            base.Initialize(component);
            DesignerActionService service = this.GetService(typeof(DesignerActionService)) as DesignerActionService;
            if (service != null)
                service.Remove(component);
        }

        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
            Rectangle clientRectangle = this.Control.ClientRectangle;
            clientRectangle.Width--;
            clientRectangle.Height--;
			ControlPaint.DrawFocusRectangle(pe.Graphics, clientRectangle);
            base.OnPaintAdornments(pe);
        }

        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);
            foreach (string p in propsToRemove)
				if (properties.Contains(p))
					properties.Remove(p);
        }

        private void handleRemovePage(object sender, EventArgs e)
        {
			WizardControlDesigner.RemovePageFromWizard(this.DesignerHost, this.ComponentChangeService, this.Page);
        }
    }
}