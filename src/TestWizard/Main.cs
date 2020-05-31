using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using Vanara.Interop.DesktopWindowManager;

namespace TestWizard
{
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();

			// Add languages to combo
			langCombo.SelectedIndexChanged += langCombo_SelectedIndexChanged;
			langCombo.BeginUpdate();
			langCombo.SelectedIndex = -1;
			foreach (var culture in GetAsmCultures(typeof(AeroWizard.WizardControl).Assembly.GetTypes().First(t => t.Name == "Resources")))
				langCombo.Items.Add(culture);
			langCombo.EndUpdate();
			langCombo.SelectedItem = CultureInfo.CurrentUICulture;
		}

		private static IEnumerable<CultureInfo> GetAsmCultures(Type type)
		{
			var rm = new ResourceManager(type);
			return CultureInfo.GetCultures(CultureTypes.AllCultures).Where(c => !c.Equals(CultureInfo.InvariantCulture) &&
				rm.GetResourceSet(c, true, false) != null).Select(c => c.IsNeutralCulture ? CultureInfo.CreateSpecificCulture(c.TwoLetterISOLanguageName) : c);
		}

		private void compEnabledCheck_CheckedChanged(object sender, EventArgs e)
		{
			DesktopWindowManager.CompositionEnabled = compEnabledCheck.Checked;
		}

		private void customBtn_Click(object sender, EventArgs e)
		{
			new TestWizBase().ShowDialog(this);
		}

		private void DesktopWindowManager_CompositionChanged(object sender, EventArgs e)
		{
			UpdateChecks();
		}

		private void langCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			System.Threading.Thread.CurrentThread.CurrentCulture = langCombo.SelectedItem as CultureInfo;
			System.Threading.Thread.CurrentThread.CurrentUICulture = langCombo.SelectedItem as CultureInfo;
		}

		private void Main_Load(object sender, EventArgs e)
		{
			compEnabledCheck.AutoCheck = Environment.OSVersion.Version < new Version(6, 2) && Environment.OSVersion.Version >= new Version(6, 0);
			UpdateChecks();
			DesktopWindowManager.CompositionChanged += DesktopWindowManager_CompositionChanged;
			//new MyWizard().ShowDialog(this);
			//Close();
		}

		private void oldButton_Click(object sender, EventArgs e)
		{
			new OldStyleWizard().ShowDialog(this);
		}

		private void stepBtn_Click(object sender, EventArgs e)
		{
			new MyStepWizard().ShowDialog(this);
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
	}
}