using System;
using System.Windows.Forms;

namespace $rootnamespace$
{
	public partial class $safeitemname$ : Form
	{
		public $safeitemname$()
		{
			InitializeComponent();
		}

		private void wizardPageContainer1_Cancelling(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Close();
		}

		private void wizardPageContainer1_Finished(object sender, EventArgs e)
		{
			this.Close();
		}

		private void wizardPageContainer1_SelectedPageChanged(object sender, EventArgs e)
		{
			headingControl.Text = wizardPageContainer1.SelectedPage.Text;
		}
	}
}
