using System.Windows.Forms;

namespace TestWizard
{
	public partial class MyStepWizard : Form
	{
		System.Drawing.Font myFont = new System.Drawing.Font("Impact", 20);

		public MyStepWizard()
		{
			InitializeComponent();
		}

		private void wizardControl1_DrawStepListItem(object sender, AeroWizard.DrawStepListItemEventArgs e)
		{
			string pre = e.Selected ? "> " : e.Completed ? "- " : "+ ";
			TextRenderer.DrawText(e.Graphics, pre + wizardControl1.GetStepText(e.Item), myFont, e.Bounds, ForeColor);
		}

		private void wizardControl1_MeasureStepListItem(object sender, AeroWizard.MeasureStepListItemEventArgs e)
		{
			e.ItemSize = new System.Drawing.Size(e.ItemSize.Width, (int)(TextRenderer.MeasureText("Wg", myFont).Height * 1.2));
		}
	}
}
