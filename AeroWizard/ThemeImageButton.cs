using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using AeroWizard.Native;

namespace AeroWizard
{
    /// <summary>
    /// ImageButton
    /// </summary>
    [ToolboxBitmap(typeof(Button))]
    internal class ThemeImageButton : ButtonBase
    {
		private const string defaultText = "<";
		private const string defaultToolTip = "Returns to a previous page";
		private PushButtonState state = PushButtonState.Normal;
		private PushButtonState stateOnEnter;
		private static bool inDesigner;
		private ToolTip toolTip;

		static ThemeImageButton()
		{
			inDesigner = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
		}

        /// <summary>
        /// ImageButton
        /// </summary>
        public ThemeImageButton()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			StyleClass = "BUTTON";
			StylePart = 1;

			Text = defaultText;
			toolTip = new ToolTip();
			toolTip.SetToolTip(this, defaultToolTip);
        }

		[DefaultValue("BUTTON")]
		public string StyleClass { get; set; }

		[DefaultValue(1)]
		public int StylePart { get; set; }

		[DefaultValue(defaultToolTip)]
		public string ToolTipText { get { return toolTip.GetToolTip(this); } set { toolTip.SetToolTip(this, value); } }

		/// <summary>
		/// </summary>
		/// <value></value>
		/// <returns>
		/// The default <see cref="T:System.Drawing.Size"/> of the control.
		/// </returns>
        protected override Size DefaultSize
        {
            get
            {
				return new Size(30, 30);
			}
        }

		/// <summary>
		/// Retrieves the size of a rectangular area into which a control can be fitted.
		/// </summary>
		/// <param name="proposedSize">The custom-sized area for a control.</param>
		/// <returns>
		/// An ordered pair of type <see cref="T:System.Drawing.Size"/> representing the width and height of a rectangle.
		/// </returns>
		public override Size GetPreferredSize(Size proposedSize)
		{
			return DefaultSize;
		}

        /// <summary>
        /// For button user use to simulate a click operate.
        /// </summary>
        public void PerformClicked()
        {
            base.OnClick(EventArgs.Empty);
        }

        /// <summary>
        /// Process Enabled property changed 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnabledChanged(EventArgs e)
        {
			System.Diagnostics.Debug.WriteLine(string.Format("ImgBtnEnabled:{0}", Enabled));
			state = Enabled ? PushButtonState.Normal : PushButtonState.Disabled;
			Invalidate();
            base.OnEnabledChanged(e);
        }

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
			if ((e.Button & MouseButtons.Left) != MouseButtons.Left) return;
			System.Diagnostics.Debug.WriteLine("ImgBtnMsDn:");
			state = PushButtonState.Pressed;
            Invalidate();
			base.OnMouseDown(e);
		}

		/// <summary>
		/// Raises the <see cref="M:System.Windows.Forms.Control.OnMouseEnter(System.EventArgs)"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnMouseEnter(EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("ImgBtnMsEn:");
			stateOnEnter = state;
			state = PushButtonState.Hot;
			Invalidate();
			base.OnMouseEnter(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
			System.Diagnostics.Debug.WriteLine("ImgBtnMsLv:");
			state = Enabled ? PushButtonState.Normal : PushButtonState.Disabled;
			Invalidate();
			base.OnMouseLeave(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
			if ((e.Button & MouseButtons.Left) != MouseButtons.Left) return;
			System.Diagnostics.Debug.WriteLine("ImgBtnMsUp:");
			state = this.Enabled ? PushButtonState.Hot : PushButtonState.Disabled;
			Invalidate();
            base.OnMouseUp(e);
        }

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			VisualStyleRenderer rnd = new VisualStyleRenderer(StyleClass, StylePart, (int)state);
			if (inDesigner)
			{
				g.Clear(this.BackColor);
				rnd.DrawBackground(e.Graphics, this.Bounds, e.ClipRectangle);
			}
			else
			{
				rnd.DrawGlassBackground(e.Graphics, this.Bounds, e.ClipRectangle);
			}
			System.Diagnostics.Debug.WriteLine(string.Format("ImgBtnPaint: bnds={0},clip={1},st={2},bg={3}", this.Bounds, e.ClipRectangle, state, this.BackColor));
		}

		/// <summary>
		/// Paints the background of the control.
		/// </summary>
		/// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains information about the control to paint.</param>
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}

		[DefaultValue(defaultText), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public override string Text { get { return base.Text; } set { base.Text = value; } }
    }
}