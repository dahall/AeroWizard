using Microsoft.Win32.DesktopWindowManager;
using System.Windows.Forms;

namespace TestWizard
{
	public partial class MyWizard : Form
	{
		public MyWizard()
		{
			InitializeComponent();
			//this.wizardControl1.TitleIcon = null;
			foreach (var i in this.wizardControl1.Pages)
				i.Commit += new System.EventHandler<AeroWizard.WizardPageConfirmEventArgs>(i_Commit);
			this.wizardControl1.Finished += new System.EventHandler(wizardControl1_Finished);
		}

		void wizardControl1_Finished(object sender, System.EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("--> Wizard finished.");
		}

		void i_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine(string.Format("--> Page {0} committed.", e.Page.Name));
		}

		private void commandLink1_Click(object sender, System.EventArgs e)
		{
			wizardControl1.NextPage();
		}

		private void commandLink2_Click(object sender, System.EventArgs e)
		{
			wizardControl1.NextPage(endPage);
		}

		private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!initMiddle)
				DesktopWindowManager.EnableComposition(checkBox1.Checked);
		}

		private bool initMiddle = false;

		private void middlePage_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
		{
			initMiddle = true;
			if (!System.IO.File.Exists(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.System), "dwmapi.dll")))
				checkBox1.Enabled = false;
			else
				checkBox1.Checked = DesktopWindowManager.IsCompositionEnabled();
			wizardControl1.FinishButtonText = "Finish";
			initMiddle = false;
		}

		private void checkBox2_CheckedChanged(object sender, System.EventArgs e)
		{
			wizardControl1.SelectedPage.AllowNext = checkBox2.Checked;
			wizardControl1.SelectedPage.AllowBack = !checkBox2.Checked;
		}

		private void endPage_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
		{
			wizardControl1.FinishButtonText = "Sorry, but you are hosed.";
		}

		private void introPage_HelpClicked(object sender, System.EventArgs e)
		{
			MessageBox.Show("Clicked help");
		}
	}
}
