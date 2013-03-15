using System.ComponentModel;
using System.Windows.Forms;

namespace AeroWizard
{
	/// <summary>
	/// Wizard control that shows a step summary on the left of the wizard page area.
	/// </summary>
	public class StepWizardControl : WizardControl
	{
        private StepList list;
        private Splitter splitter;

		/// <summary>
		/// Initializes a new instance of the <see cref="StepWizardControl"/> class.
		/// </summary>
		public StepWizardControl()
		{
            this.pageContainer.Controls.Add(splitter = new Splitter() { Dock = DockStyle.Left, BorderStyle = BorderStyle.FixedSingle, Width = 1 });
            this.pageContainer.Controls.Add(list = new StepList() { Dock = DockStyle.Left });
            this.Pages.Reset += Pages_Reset;
		}

        /// <summary>
        /// Gets or sets the width of the step list.
        /// </summary>
        /// <value>
        /// The width of the step list.
        /// </value>
        [DefaultValue(150), Category("Appearance"), Description("Determines width of step list on left.")]
        public int StepListWidth
        {
            get { return list.Width; }
            set { list.Width = value; }
        }

        void Pages_Reset(object sender, System.Collections.Generic.EventedList<WizardPage>.ListChangedEventArgs<WizardPage> e)
        {
            this.pageContainer.Controls.Add(splitter);
            this.pageContainer.Controls.Add(list);
        }
	}
}
