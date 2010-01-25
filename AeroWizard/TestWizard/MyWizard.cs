using System.Windows.Forms;

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
	}
}
