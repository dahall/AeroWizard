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

        [DefaultValue("BUTTON")]
        public string StyleClass
        {
            get; set;
        }

        [DefaultValue(1)]
        public int StylePart
        {
            get; set;
        }

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
        }
    }
}