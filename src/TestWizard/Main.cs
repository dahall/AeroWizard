using System;
using System.Windows.Forms;
using Vanara.Interop.DesktopWindowManager;

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
			compEnabledCheck.AutoCheck = Environment.OSVersion.Version < new Version(6, 2) && Environment.OSVersion.Version >= new Version(6, 0);
			UpdateChecks();
			DesktopWindowManager.CompositionChanged += DesktopWindowManager_CompositionChanged;
			//new MyWizard().ShowDialog(this);
			//Close();
		}

		private void DesktopWindowManager_CompositionChanged(object sender, EventArgs e)
		{
			UpdateChecks();
		}

		private void UpdateChecks()
		{
			appRenderVS.Checked = Application.RenderWithVisualStyles;
			compEnabledCheck.Checked = DesktopWindowManager.CompositionEnabled;
			vsEnabledByUser.Checked = System.Windows.Forms.VisualStyles.VisualStyleInformation.IsEnabledByUser;
			vsOnOS.Checked = System.Windows.Forms.VisualStyles.VisualStyleInformation.IsSupportedByOS;
		}

		private void wizBtn_Click(object sender, EventArgs e)
		{
			new MyWizard().ShowDialog(this);
		}

		private void stepBtn_Click(object sender, EventArgs e)
		{
			new MyStepWizard().ShowDialog(this);
		}

		private void customBtn_Click(object sender, EventArgs e)
		{
			new TestWizBase().ShowDialog(this);
		}

		private void compEnabledCheck_CheckedChanged(object sender, EventArgs e)
		{
			DesktopWindowManager.CompositionEnabled = compEnabledCheck.Checked;
		}

		private void oldButton_Click(object sender, EventArgs e)
		{
			new OldStyleWizard().ShowDialog(this);
		}
	}
}
