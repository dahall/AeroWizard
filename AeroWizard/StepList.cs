using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AeroWizard
{
	/// <summary>
	/// Shows a list of all the pages in the WizardControl
	/// </summary>
	[ProvideProperty("StepText", typeof(WizardPage))]
	[ProvideProperty("StepTextIndentLevel", typeof(WizardPage))]
	internal class StepList : ScrollableControl, IExtenderProvider
	{
		private WizardControl myParent;
		private Dictionary<WizardPage, string> stepTexts = new Dictionary<WizardPage, string>();
		private Dictionary<WizardPage, int> indentLevels = new Dictionary<WizardPage, int>();

		/// <summary>
		/// Initializes a new instance of the <see cref="StepList"/> class.
		/// </summary>
		public StepList()
		{
		}

		private void SetupControl(WizardControl p)
		{
			if (myParent != null)
			{
				WizardPageCollection pages = myParent.Pages;
				pages.ItemAdded -= pages_Changed;
				pages.ItemChanged -= pages_Changed;
				pages.ItemDeleted -= pages_Changed;
			}
			myParent = p;
			if (myParent != null)
			{
				WizardPageCollection pages = myParent.Pages;
				pages.ItemAdded += pages_Changed;
				pages.ItemChanged += pages_Changed;
				pages.ItemDeleted += pages_Changed;
			}
			Refresh();
		}

		private void pages_Changed(object sender, EventedList<WizardPage>.ListChangedEventArgs<WizardPage> e)
		{
			Refresh();
		}

		/// <summary>
		/// Gets the default size of the control.
		/// </summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		protected override Size DefaultSize { get { return new Size(150, 200); } }

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged" /> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			SetupControl(this.GetParent<WizardControl>());
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (myParent == null) return;
			bool isRTL = this.RightToLeft == System.Windows.Forms.RightToLeft.Yes;
			using (Font ptrFont = new Font("Marlett", this.Font.Size), boldFont = new Font(this.Font, FontStyle.Bold))
			{
				int itemHeight = (int)Math.Ceiling(TextRenderer.MeasureText(e.Graphics, "Wg", this.Font).Height * 1.2);
				var tffrtl = isRTL ? TextFormatFlags.Right : 0;
				var tff = TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | tffrtl;
				int lPad = TextRenderer.MeasureText(e.Graphics, "4", ptrFont, new Size(0, itemHeight), tff).Width;
				const int rPad = 4;
				Rectangle rect = new Rectangle(0, 0, this.Width - lPad - rPad, itemHeight);
				Rectangle prect = new Rectangle(0, 0, lPad, itemHeight);
				System.Diagnostics.Debug.WriteLine(string.Format("r1:{0}; r2:{1}", prect, rect));
				WizardPageCollection pages = myParent.Pages;
				bool hit = false;
				for (int i = 0; i < pages.Count && rect.Y < (this.Height - itemHeight); i++)
				{
					if (!pages[i].Suppress)
					{
						Color fc = this.ForeColor, bc = this.BackColor;
						bool isSelected = myParent.SelectedPage == pages[i];
						int level = GetStepTextIndentLevel(pages[i]);
						prect.X = isRTL ? this.Width - (lPad * (level + 1)) : lPad * level;
						if (isRTL)
							rect.Width = this.Width - (lPad * (level + 1));
						else
							rect.X = lPad * (level + 1);
						if (isSelected)
						{
							hit = true;
							//fc = SystemColors.HighlightText;
							//bc = SystemColors.Highlight;
						}
						else if (!hit)
						{
							fc = SystemColors.GrayText;
						}
						using (Brush br = new SolidBrush(bc))
							e.Graphics.FillRectangle(br, Rectangle.Union(rect, prect));
						TextRenderer.DrawText(e.Graphics, hit ? (isRTL ? "3" : "4") : "a", ptrFont, prect, fc, tff);
						TextRenderer.DrawText(e.Graphics, GetStepText(pages[i]), isSelected ? boldFont : this.Font, rect, fc, TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | tffrtl);
						prect.Y = rect.Y += itemHeight;
					}
				}
			}
		}

		bool IExtenderProvider.CanExtend(object extendee)
		{
			return (extendee is WizardPage);
		}

		/// <summary>
		/// Gets the step text.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <returns>Step text for the specified wizard page.</returns>
		[DefaultValue((string)null), Category("Appearance"), Description("Alternate text to provide to the StepList. Default value comes the Text property of the WizardPage.")]
		public string GetStepText(WizardPage page)
		{
			string value;
			if (stepTexts.TryGetValue(page, out value))
				return value;
			return page.Text;
		}

		/// <summary>
		/// Sets the step text.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="value">The value.</param>
		public void SetStepText(WizardPage page, string value)
		{
			if (string.IsNullOrEmpty(value) || value == page.Text)
				stepTexts.Remove(page);
			else
				stepTexts[page] = value;
			Refresh();
		}

		private bool ShouldSerializeStepText(WizardPage page)
		{
			return (GetStepText(page) != page.Text);
		}

		private void ResetStepText(WizardPage page)
		{
			SetStepText(page, null);
		}

		/// <summary>
		/// Gets the step text indent level.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <returns>Step text indent level for the specified wizard page.</returns>
		[DefaultValue(0), Category("Appearance"), Description("Indentation level for text provided to the StepList.")]
		public int GetStepTextIndentLevel(WizardPage page)
		{
			int value;
			if (indentLevels.TryGetValue(page, out value))
				return value;
			return 0;
		}

		/// <summary>
		/// Sets the step text indent level.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="value">The indent level.</param>
		public void SetStepTextIndentLevel(WizardPage page, int value)
		{
			if (value < 0) value = 0;
			if (value == 0)
				indentLevels.Remove(page);
			else
				indentLevels[page] = value;
			Refresh();
		}
	}
}
