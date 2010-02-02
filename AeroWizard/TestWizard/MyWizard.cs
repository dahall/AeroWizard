using System.Windows.Forms;
using Microsoft.Win32.DesktopWindowManager;

namespace TestWizard
{
	public partial class MyWizard : Form
	{
		public MyWizard()
		{
			InitializeComponent();
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
			checkBox1.Checked = DesktopWindowManager.IsCompositionEnabled();
			initMiddle = false;
		}
	}
}
