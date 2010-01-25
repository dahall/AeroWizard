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
			new MyWizard().ShowDialog(this);
			Close();
		}
	}
}
