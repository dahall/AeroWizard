using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestWizard
{
	public partial class TestWizBase : Form
	{
		public TestWizBase()
		{
			InitializeComponent();
		}

		private void wizardPageContainer1_Finished(object sender, EventArgs e)
		{
			this.Close();
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			wizardPage2.AllowNext = checkBox1.Checked;
		}

		private void wizardPageContainer1_SelectedPageChanged(object sender, EventArgs e)
		{
			label4.Text = wizardPageContainer1.SelectedPage.Text;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
