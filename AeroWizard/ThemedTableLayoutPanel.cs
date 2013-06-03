using Microsoft.Win32.DesktopWindowManager;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace AeroWizard
{
	internal class ThemedTableLayoutPanel : TableLayoutPanel
	{
		private VisualStyleRenderer rnd;

		public ThemedTableLayoutPanel()
		{
			SetTheme(VisualStyleElement.Window.Dialog.Normal);
		}

		public void SetTheme(VisualStyleElement element)
		{
			if (VisualStyleRenderer.IsSupported && VisualStyleRenderer.IsElementDefined(element))
				rnd = new VisualStyleRenderer(element);
			else
				rnd = null;
		}

		public void SetTheme(string className, int part, int state)
		{
			if (VisualStyleRenderer.IsSupported)
			{
				try
				{
					rnd = new VisualStyleRenderer(className, part, state);
					return;
				}
				catch { }
			}
			rnd = null;
		}

		[DefaultValue(false), Category("Behavior")]
		public bool WatchFocus { get; set; }

		[DefaultValue(false), Category("Appearance")]
		public bool SupportGlass { get; set; }

		protected override void OnHandleCreated(System.EventArgs e)
		{
			base.OnHandleCreated(e);
			AttachToFormEvents();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (!this.IsDesignMode() && SupportGlass && DesktopWindowManager.IsCompositionEnabled())
				try { e.Graphics.Clear(System.Drawing.Color.Black); } catch { }
			else
			{
				if (this.IsDesignMode() || rnd == null)
					try { e.Graphics.Clear(this.BackColor); } catch { }
				else
					rnd.DrawBackground(e.Graphics, this.ClientRectangle, e.ClipRectangle);
			}
			base.OnPaint(e);
		}

		private void AttachToFormEvents()
		{
			Form pForm = this.FindForm();
			if (pForm != null && WatchFocus)
			{
				pForm.Activated += new System.EventHandler(Form_GotFocus);
				pForm.Deactivate += new System.EventHandler(Form_LostFocus);
			}
		}

		private void Form_GotFocus(object sender, System.EventArgs e)
		{
			OnGotFocus(e);
			if (rnd != null)
				rnd.SetParameters(rnd.Class, rnd.Part, 1);
			Refresh();
		}

		private void Form_LostFocus(object sender, System.EventArgs e)
		{
			OnLostFocus(e);
			if (rnd != null)
				rnd.SetParameters(rnd.Class, rnd.Part, 2);
			Refresh();
		}
	}
}
