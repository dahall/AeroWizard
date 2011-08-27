using System.Windows.Forms;
using Microsoft.Win32.DesktopWindowManager;

namespace TestWizard
{
	public partial class MyWizard : Form
	{
		public MyWizard()
		{
			InitializeComponent();
			System.Resources.ResourceManager rm = new System.Resources.ResourceManager("AeroWizard.Properties.Resources", typeof(AeroWizard.WizardControl).Assembly);
			this.Icon = rm.GetObject("WizardControlIcon") as System.Drawing.Icon;
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
			initMiddle = false;
		}

		private void checkBox2_CheckedChanged(object sender, System.EventArgs e)
		{
			wizardControl1.SelectedPage.AllowNext = checkBox2.Checked;
			wizardControl1.SelectedPage.AllowBack = !checkBox2.Checked;
		}
	}
}
