using Microsoft.Win32;
using System.Drawing;

namespace System.Windows.Forms.VisualStyles
{
	internal static partial class VisualStyleRendererExtension
	{
		private delegate void DrawWrapperMethod(IntPtr hdc);

		public static void DrawGlassBackground(this VisualStyleRenderer rnd, IDeviceContext dc, Rectangle bounds, Rectangle clipRectangle, bool rightToLeft = false)
		{
			DrawWrapper(rnd, dc, bounds,
				delegate(IntPtr memoryHdc)
				{
					NativeMethods.RECT rBounds = new NativeMethods.RECT(bounds);
					NativeMethods.RECT rClip = new NativeMethods.RECT(clipRectangle);
					// Draw background
					if (rightToLeft) NativeMethods.SetLayout(memoryHdc, 1);
					NativeMethods.DrawThemeBackground(rnd.Handle, memoryHdc, rnd.Part, rnd.State, ref rBounds, ref rClip);
					NativeMethods.SetLayout(memoryHdc, 0);
				}
			);
		}

		public static void DrawGlassIcon(this VisualStyleRenderer rnd, Graphics g, Rectangle bounds, ImageList imgList, int imgIndex)
		{
			DrawWrapper(rnd, g, bounds,
				delegate(IntPtr memoryHdc)
				{
					NativeMethods.RECT rBounds = new NativeMethods.RECT(bounds);
					NativeMethods.DrawThemeIcon(rnd.Handle, memoryHdc, rnd.Part, rnd.State, ref rBounds, imgList.Handle, imgIndex);
				}
			);
		}

		public static void DrawGlassImage(this VisualStyleRenderer rnd, Graphics g, Rectangle bounds, Image img, bool disabled = false)
		{
			DrawWrapper(rnd, g, bounds,
				delegate(IntPtr memoryHdc)
				{
					using (Graphics mg = Graphics.FromHdc(memoryHdc))
					{
						if (disabled)
							ControlPaint.DrawImageDisabled(mg, img, bounds.X, bounds.Y, Color.Transparent);
						else
							mg.DrawImage(img, bounds);
					}
				}
			);
		}

		public static void DrawGlowingText(this VisualStyleRenderer rnd, IDeviceContext dc, Rectangle bounds, string text, Font font, Color color, System.Windows.Forms.TextFormatFlags flags)
		{
			DrawWrapper(rnd, dc, bounds,
				delegate(IntPtr memoryHdc) {
					// Create and select font
					using (NativeMethods.SafeDCObjectHandle fontHandle = new NativeMethods.SafeDCObjectHandle(memoryHdc, font.ToHfont()))
					{
						// Draw glowing text
						NativeMethods.DrawThemeTextOptions dttOpts = new NativeMethods.DrawThemeTextOptions(true);
						dttOpts.TextColor = color;
						dttOpts.GlowSize = 10;
						dttOpts.AntiAliasedAlpha = true;
						NativeMethods.RECT textBounds = new NativeMethods.RECT(4, 0, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);
						NativeMethods.DrawThemeTextEx(rnd.Handle, memoryHdc, rnd.Part, rnd.State, text, text.Length, (int)flags, ref textBounds, ref dttOpts);
					}
				}
			);
		}

		/*public static void DrawGlowingText(this VisualStyleRenderer rnd, IDeviceContext dc, Rectangle bounds, string text, Font font, Color color, System.Windows.Forms.TextFormatFlags flags)
		{
			using (SafeGDIHandle primaryHdc = new SafeGDIHandle(dc))
			{
				// Create a memory DC so we can work offscreen
				using (SafeCompatibleDCHandle memoryHdc = new SafeCompatibleDCHandle(primaryHdc))
				{
					// Create a device-independent bitmap and select it into our DC
					BITMAPINFO info = new BITMAPINFO(bounds.Width, -bounds.Height);
					using (SafeDCObjectHandle dib = new SafeDCObjectHandle(memoryHdc, GDI.CreateDIBSection(primaryHdc, ref info, 0, 0, IntPtr.Zero, 0)))
					{
						// Create and select font
						using (SafeDCObjectHandle fontHandle = new SafeDCObjectHandle(memoryHdc, font.ToHfont()))
						{
							// Draw glowing text
							DrawThemeTextOptions dttOpts = new DrawThemeTextOptions(true);
							dttOpts.TextColor = color;
							dttOpts.GlowSize = 10;
							dttOpts.AntiAliasedAlpha = true;
							NativeMethods.RECT textBounds = new NativeMethods.RECT(4, 0, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);
							DrawThemeTextEx(rnd.Handle, memoryHdc, rnd.Part, rnd.State, text, text.Length, (int)flags, ref textBounds, ref dttOpts);

							// Copy to foreground
							const int SRCCOPY = 0x00CC0020;
							GDI.BitBlt(primaryHdc, bounds.Left, bounds.Top, bounds.Width, bounds.Height, memoryHdc, 0, 0, SRCCOPY);
						}
					}
				}
			}
		}*/

		public static void DrawText(this VisualStyleRenderer rnd, IDeviceContext dc, ref Rectangle bounds, string text, System.Windows.Forms.TextFormatFlags flags, NativeMethods.DrawThemeTextOptions options)
		{
			NativeMethods.RECT rc = new NativeMethods.RECT(bounds);
			using (SafeGDIHandle hdc = new SafeGDIHandle(dc))
				NativeMethods.DrawThemeTextEx(rnd.Handle, hdc, rnd.Part, rnd.State, text, text.Length, (int)flags, ref rc, ref options);
			bounds = rc;
		}

		public static System.Drawing.Font GetFont2(this VisualStyleRenderer rnd, IDeviceContext dc = null)
		{
			using (SafeGDIHandle hdc = new SafeGDIHandle(dc))
			{
				Microsoft.Win32.NativeMethods.LOGFONT f;
				int hres = NativeMethods.GetThemeFont(rnd.Handle, hdc, rnd.Part, rnd.State, 210, out f);
				if (hres != 0)
					throw new System.ComponentModel.Win32Exception(hres);
				return f.ToFont();
			}
		}

		private static void DrawWrapper(VisualStyleRenderer rnd, IDeviceContext dc, Rectangle bounds, DrawWrapperMethod func)
		{
			using (SafeGDIHandle primaryHdc = new SafeGDIHandle(dc))
			{
				// Create a memory DC so we can work offscreen
				using (NativeMethods.SafeCompatibleDCHandle memoryHdc = new NativeMethods.SafeCompatibleDCHandle(primaryHdc))
				{
					// Create a device-independent bitmap and select it into our DC
					NativeMethods.BITMAPINFO info = new NativeMethods.BITMAPINFO(bounds.Width, -bounds.Height);
					using (NativeMethods.SafeDCObjectHandle dib = new NativeMethods.SafeDCObjectHandle(memoryHdc, NativeMethods.CreateDIBSection(primaryHdc, ref info, 0, IntPtr.Zero, IntPtr.Zero, 0)))
					{
						// Call method
						func(memoryHdc);

						// Copy to foreground
						const int SRCCOPY = 0x00CC0020;
						NativeMethods.BitBlt(primaryHdc, bounds.Left, bounds.Top, bounds.Width, bounds.Height, memoryHdc, 0, 0, SRCCOPY);
					}
				}
			}
		}
	}
}