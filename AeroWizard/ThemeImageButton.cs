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
	internal class ThemeImageButton : ImageButton
	{
		private const string defaultText = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeImageButton"/> class.
        /// </summary>
		public ThemeImageButton()
		{
			StyleClass = "BUTTON";
			StylePart = 1;
			Text = defaultText;
		}

        /// <summary>
        /// Gets or sets the <see cref="T:System.Windows.Forms.ImageList" /> that contains the <see cref="T:System.Drawing.Image" /> displayed on a button control.
        /// </summary>
        /// <returns>An <see cref="T:System.Windows.Forms.ImageList" />. The default value is null.</returns>
        ///   <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   </PermissionSet>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new ImageList ImageList
        {
            get { return base.ImageList; }
            set { base.ImageList = value; }
        }

        /// <summary>
        /// Gets or sets the style class.
        /// </summary>
        /// <value>
        /// The style class.
        /// </value>
        [DefaultValue("BUTTON"), Category("Appearance")]
        public string StyleClass { get; set; }

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
		/// Paints the background of the control.
		/// </summary>
		/// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains information about the control to paint.</param>
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}

        /// <summary>
        /// Primary function for painting the button. This method should be overridden instead of OnPaint.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="bounds">The bounds.</param>
		protected override void PaintButton(Graphics graphics, Rectangle bounds)
		{
			if (Application.RenderWithVisualStyles || DesktopWindowManager.IsCompositionEnabled())
			{
				try
				{
					VisualStyleRenderer rnd = new VisualStyleRenderer(StyleClass, StylePart, (int)ButtonState);
					if (this.IsDesignMode() || !DesktopWindowManager.IsCompositionEnabled())
					{
						rnd.DrawParentBackground(graphics, this.Bounds, this);
						rnd.DrawBackground(graphics, this.Bounds, bounds);
					}
					else
					{
                        if (this.GetRightToLeftProperty() == System.Windows.Forms.RightToLeft.Yes)
                        {
                            int idx = (int)ButtonState - 1;
                            if (this.ImageList.Images.Count == 2)
                                idx = ButtonState == PushButtonState.Disabled ? 1 : 0;
                            if (this.ImageList.Images.Count == 3)
                                idx = ButtonState == PushButtonState.Normal ? 0 : idx - 1;
                            //rnd.DrawGlassIcon(graphics, this.ClientRectangle, this.ImageList, idx);
                            this.ImageList.Draw(graphics, this.ClientRectangle.Location, idx);
                        }
                        else
                            rnd.DrawGlassBackground(graphics, this.ClientRectangle, bounds, this.GetRightToLeftProperty() == System.Windows.Forms.RightToLeft.Yes);
					}
					return;
				}
				catch { }
			}

			//base.PaintButton(graphics, bounds);
            /*Rectangle sr = this.ClientRectangle;
            sr.Offset(0, sr.Height * ((int)ButtonState - 1));
            if (this.GetRightToLeftProperty() == System.Windows.Forms.RightToLeft.Yes)
                sr.X = sr.Width;
            graphics.Clear(this.Parent.BackColor);
            if (imageStrip != null)
                graphics.DrawImage(imageStrip, this.Bounds, sr, GraphicsUnit.Pixel);
            else
                using (Brush br = new SolidBrush(this.BackColor))
                    graphics.FillRectangle(br, sr);*/ 
		}
	}
}