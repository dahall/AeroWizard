using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Microsoft.Win32.DesktopWindowManager;

namespace AeroWizard
{
    /// <summary>
    /// A Label containing some text that will be drawn with glowing border on top of the Glass Sheet effect.
    /// </summary>
    //[Designer("AeroWizard.Design.ThemedLabelDesigner")]
    [DefaultProperty("Text")]
    internal class ThemedLabel : Label
    {
        private static bool inDesigner;

        static ThemedLabel()
        {
            inDesigner = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }

        public ThemedLabel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        public new Image Image
        {
            get { return base.Image; }
            set
            {
                base.Image = value;
                base.ImageIndex = -1;
                base.ImageList = null;
            }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size sz = base.GetPreferredSize(proposedSize);
            if (Text.Length > 0)
                sz.Width += 10;
            return sz;
        }

        internal static Rectangle DeflateRect(Rectangle rect, Padding padding)
        {
            rect.X += padding.Left;
            rect.Y += padding.Top;
            rect.Width -= padding.Horizontal;
            rect.Height -= padding.Vertical;
            return rect;
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);

            //Invalidate parent
            this.Parent.Invalidate(this.ClientRectangle, false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Visible)
            {
                //e.Graphics.Clear(this.Parent.BackColor);
                VisualStyleRenderer vs = new VisualStyleRenderer(VisualStyleElement.Window.Caption.Active);
                Rectangle r = DeflateRect(base.ClientRectangle, base.Padding);
                if (this.Image != null)
                {
                    Rectangle ir = CalcImageRenderBounds(this.Image, r, base.RtlTranslateAlignment(this.ImageAlign));
                    if (this.ImageList != null && this.ImageIndex == 0)
                        vs.DrawImage(e.Graphics, r, this.ImageList, this.ImageIndex);
                    else
                        vs.DrawImage(e.Graphics, r, this.Image);
                }
                if (this.Text.Length > 0)
                {
                    TextFormatFlags tff = CreateTextFormatFlags(this.TextAlign, this.AutoEllipsis, this.UseMnemonic);
                    if (inDesigner || System.Environment.OSVersion.Version.Major < 6 || !DesktopWindowManager.IsCompositionEnabled())
                        e.Graphics.DrawString(Text, Font, SystemBrushes.ActiveCaptionText, e.ClipRectangle);
                    else
                        vs.DrawGlowingText(e.Graphics, base.ClientRectangle, Text, Font, ForeColor, tff);
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTTRANSPARENT = -1;

            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = new IntPtr(HTTRANSPARENT);
        }

        private TextFormatFlags CreateTextFormatFlags(System.Drawing.ContentAlignment textAlign, bool showEllipsis, bool useMnemonic)
        {
            TextFormatFlags flags = TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.SingleLine;
            if ((textAlign & (System.Drawing.ContentAlignment.BottomRight | System.Drawing.ContentAlignment.BottomCenter | System.Drawing.ContentAlignment.BottomLeft)) != ((System.Drawing.ContentAlignment)0))
                flags |= TextFormatFlags.Bottom;
            if ((textAlign & (System.Drawing.ContentAlignment.MiddleRight | System.Drawing.ContentAlignment.MiddleCenter | System.Drawing.ContentAlignment.MiddleLeft)) != ((System.Drawing.ContentAlignment)0))
                flags |= TextFormatFlags.VerticalCenter;
            if ((textAlign & (System.Drawing.ContentAlignment.BottomRight | System.Drawing.ContentAlignment.MiddleRight | System.Drawing.ContentAlignment.TopRight)) != ((System.Drawing.ContentAlignment)0))
                flags |= TextFormatFlags.Right;
            if ((textAlign & (System.Drawing.ContentAlignment.BottomCenter | System.Drawing.ContentAlignment.MiddleCenter | System.Drawing.ContentAlignment.TopCenter)) != ((System.Drawing.ContentAlignment)0))
                flags |= TextFormatFlags.HorizontalCenter;
            if (showEllipsis)
                flags |= TextFormatFlags.EndEllipsis;
            if (this.RightToLeft == RightToLeft.Yes)
                flags |= TextFormatFlags.RightToLeft;
            if (!useMnemonic)
                return (flags | TextFormatFlags.NoPrefix);
            if (!this.ShowKeyboardCues)
                flags |= TextFormatFlags.HidePrefix;
            return flags;
        }
    }
}