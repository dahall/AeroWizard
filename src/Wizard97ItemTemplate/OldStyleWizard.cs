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

		private void wizardPageContainer1_Finished(object sender, EventArgs e)
		{
			Close();
		}

		private void wizardPageContainer1_SelectedPageChanged(object sender, EventArgs e)
		{
			string[] headers = new string[] { "" };
			if (wizardPageContainer1.SelectedPage.Text != null)
				headers = wizardPageContainer1.SelectedPage.Text.Split('|');
			headerLabel.Text = headers[0];
			if (headers.Length == 2)
				subHeaderLabel.Text = headers[1];
		}

		private void wizardPage1_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
		{
			headerPanel.Visible = topDivider.Visible = false;
			startEndPicture.Visible = true;
		}

		private void wizardPage2_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
		{
			headerPanel.Visible = topDivider.Visible = true;
			startEndPicture.Visible = false;
		}
	}
}
