using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Microsoft.Win32.DesktopWindowManager;

namespace AeroWizard
{
	/// <summary>
	/// Image button that can be displayed on glass.
	/// </summary>
	[ToolboxBitmap(typeof(Button))]
	internal class XThemeImageButton : ThemeImageButton
	{
		private const string defaultText = "";

		private Image imageStrip;

		/// <summary>
		/// Initializes a new instance of the <see cref="ThemeImageButton"/> class.
		/// </summary>
		public XThemeImageButton()
		{
			StyleClass = "BUTTON";
			StylePart = 1;
			Text = defaultText;
		}

		/// <summary>
		/// Gets or sets the compatible image strip used when visual style rendering is not available.
		/// </summary>
		/// <value>The compatible image strip.</value>
		[DefaultValue(null), Category("Appearance")]
		public Image CompatibleImageStrip
		{
			get { return imageStrip; }
			set { base.SetImageListImageStrip(imageStrip = value, Orientation.Vertical); }
		}

		/// <summary>
		/// Gets or sets the style class.
		/// </summary>
		/// <value>The style class.</value>
		[DefaultValue("BUTTON"), Category("Appearance")]
		public string StyleClass { get; set; }

		/// <summary>
		/// Gets or sets the style part.
		/// </summary>
		/// <value>The style part.</value>
		[DefaultValue(1), Category("Appearance")]
		public int StylePart { get; set; }

		/// <summary>
		/// Gets or sets the text associated with this control.
		/// </summary>
		/// <returns>
		/// The text associated with this control.
		///   </returns>
		[DefaultValue(defaultText), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public override string Text
		{
			get { return base.Text; }
			set { base.Text = value; }
		}

		/// <summary>
		/// Primary function for painting the button. This method should be overridden instead of OnPaint.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="bounds">The bounds.</param>
		protected override void PaintButton(Graphics graphics, Rectangle bounds)
		{
			if (Application.RenderWithVisualStyles)
			{
				try
				{
					VisualStyleRenderer rnd = new VisualStyleRenderer(StyleClass, StylePart, (int)ButtonState);
					if (this.IsDesignMode() || !DesktopWindowManager.IsCompositionEnabled())
					{
						rnd.DrawParentBackground(graphics, bounds, this);
						rnd.DrawBackground(graphics, bounds);
					}
					else
					{
						rnd.DrawGlassBackground(graphics, bounds, bounds);
					}
					return;
				}
				catch { }
			}
			else
			{
				base.PaintButton(graphics, bounds);
				/*Rectangle sr = this.ClientRectangle;
				sr.Offset(0, sr.Height * ((int)ButtonState - 1));
				graphics.Clear(this.Parent.BackColor);
				if (imageStrip != null)
				{
					Bitmap bmp = imageStrip.Clone(sr, imageStrip.PixelFormat);
					if (this.IsDesignMode() || !DesktopWindowManager.IsCompositionEnabled())
					{
						base.ImageList.Draw(graphics, bounds.X, bounds.Y, bounds.Width, bounds.Height, ((int)ButtonState - 1));
					}
					else
					{
						VisualStyleRendererExtender.DrawGlassImage(null, graphics, bounds, bmp);
					}
				}
				else
					using (Brush br = new SolidBrush(this.BackColor))
						graphics.FillRectangle(br, sr);*/
			}
		}
	}
}