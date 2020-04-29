using AeroWizard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestWizard
{
	public partial class EmbeddedStepList : Form
	{
		public EmbeddedStepList()
		{
			InitializeComponent();
		}

		private void EmbeddedStepList_Load(object sender, EventArgs e)
		{
			// Setup the list
			foreach (var wizPage in wizardPageContainer1.Pages)
				listBox1.Items.Add(wizPage.Text);
			listBox1.SelectedIndex = 0;
		}

		private void wizardPageContainer1_SelectedPageChanged(object sender, EventArgs e)
		{
			// When a wizard page changes, set the label text and the active item in the list
			wizardPageLabel.Text = wizardPageContainer1.SelectedPage.Text;
			listBox1.SelectedIndex = listBox1.Items.IndexOf(wizardPageContainer1.SelectedPage.Text);
		}

		private void wizardPage3_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
		{
			// Do final work here for all pages
		}
	}
}
