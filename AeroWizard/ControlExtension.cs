using System.Windows.Forms;

namespace AeroWizard
{
	static class ControlExtension
	{
		public static bool IsDesignMode(this Control ctrl)
		{
			Control p = ctrl.Parent;
			while (p != null)
			{
				var site = p.Site;
				if (site != null && site.DesignMode)
					return true;
				p = p.Parent;
			}
			return false;
		}
	}
}
