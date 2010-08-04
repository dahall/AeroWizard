using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Microsoft.Win32.DesktopWindowManager;

namespace AeroWizard
{
    /// <summary>
    /// ImageButton
    /// </summary>
    [ToolboxBitmap(typeof(Button))]
    internal class ThemeImageButton : ImageButton
    {
        private const string defaultText = "";

        private static bool inDesigner;

		private Image imageStrip;

        static ThemeImageButton()
        {
            inDesigner = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }

        /// <summary>
        /// ImageButton
        /// </summary>
        public ThemeImageButton()
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
			get
			{
				return imageStrip;
			}
			set
			{
				imageStrip = value;
				/*if (imageStrip == null)
					base.ImageList = null;
				else
				{
					if (base.ImageList == null)
						base.ImageList = new ImageList() { ImageSize = new Size(29, 27) };
					else
						base.ImageList.Images.Clear();
					base.ImageList.Images.AddStrip(value);
				}*/
			}
		}

		[DefaultValue("BUTTON"), Category("Appearance")]
        public string StyleClass
        {
            get; set;
        }

		[DefaultValue(1), Category("Appearance")]
        public int StylePart
        {
            get; set;
        }

		[Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		EditorBrowsable(EditorBrowsableState.Never)]
		public new ImageList ImageList { get { return base.ImageList; } set { base.ImageList = value; } }

        [DefaultValue(defaultText),
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get { return base.Text; } set { base.Text = value; }
        }

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        /// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains information about the control to paint.</param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void PaintButton(Graphics graphics, Rectangle bounds)
        {
			if (Application.RenderWithVisualStyles)
			{
				try
				{
					VisualStyleRenderer rnd = new VisualStyleRenderer(StyleClass, StylePart, (int)ButtonState);
					if (inDesigner || System.Environment.OSVersion.Version.Major < 6 || !DesktopWindowManager.IsCompositionEnabled())
					{
						graphics.Clear(this.BackColor);
						rnd.DrawBackground(graphics, this.Bounds, bounds);
					}
					else
					{
						rnd.DrawGlassBackground(graphics, this.Bounds, bounds);
					}
					return;
				}
				catch { }
			}

			//base.PaintButton(graphics, bounds);
			Rectangle sr = this.ClientRectangle;
			sr.Offset(0, sr.Height * ((int)ButtonState - 1));
			graphics.Clear(this.Parent.BackColor);
			if (imageStrip != null)
				graphics.DrawImage(imageStrip, bounds, sr, GraphicsUnit.Pixel);
			else
				using (Brush br = new SolidBrush(this.BackColor))
					graphics.FillRectangle(br, sr);
        }
    }
}