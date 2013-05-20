using System;
using System.Windows.Forms;

namespace TestWizard
{
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();
		}

		private void Main_Load(object sender, EventArgs e)
		{
			//UpdateChecks();
			//Microsoft.Win32.DesktopWindowManager.DesktopWindowManager.CompositionChanged += DesktopWindowManager_CompositionChanged;
			new MyWizard().ShowDialog(this);
			Close();
		}

		private void DesktopWindowManager_CompositionChanged(object sender, EventArgs e)
		{
			UpdateChecks();
		}

		private void UpdateChecks()
		{
			checkBox1.Checked = Application.RenderWithVisualStyles;
			checkBox2.Checked = Microsoft.Win32.DesktopWindowManager.DesktopWindowManager.CompositionEnabled;
			checkBox3.Checked = System.Windows.Forms.VisualStyles.VisualStyleInformation.IsEnabledByUser;
			checkBox4.Checked = System.Windows.Forms.VisualStyles.VisualStyleInformation.IsSupportedByOS;
		}
	}
}
