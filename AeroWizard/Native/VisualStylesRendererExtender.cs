using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles.Internal;

namespace System.Windows.Forms.VisualStyles
{
	internal static class VisualStyleRendererExtender
	{
		[Flags]
		enum DrawThemeTextOptionsFlags : int
		{
			TextColor = 1,
			BorderColor = 2,
			ShadowColor = 4,
			ShadowType = 8,
			ShadowOffset = 16,
			BorderSize = 32,
			FontProp = 64,
			ColorProp = 128,
			StateId = 256,
			CalcRect = 512,
			ApplyOverlay = 1024,
			GlowSize = 2048,
			Callback = 4096,
			Composited = 8192
		}

		public enum DrawThemeTextSystemFonts
		{
			Caption = 801,
			SmallCaption = 802,
			Menu = 803,
			Status = 804,
			MessageBox = 805,
			IconTitle = 806
		}

		public enum TextShadowType : int
		{
			None = 0,
			Single = 1,
			Continuous = 2
		}

		private delegate void DrawWrapperMethod(IntPtr hdc);

		private static void DrawWrapper(VisualStyleRenderer rnd, IDeviceContext dc, Rectangle bounds, DrawWrapperMethod func)
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
						// Call method
						func(memoryHdc);

						// Copy to foreground 
						const int SRCCOPY = 0x00CC0020;
						GDI.BitBlt(primaryHdc, bounds.Left, bounds.Top, bounds.Width, bounds.Height, memoryHdc, 0, 0, SRCCOPY);
					}
				}
			}
		}

		public static void DrawGlassBackground(this VisualStyleRenderer rnd, IDeviceContext dc, Rectangle bounds, Rectangle clipRectangle)
		{
			DrawWrapper(rnd, dc, bounds,
				delegate(IntPtr memoryHdc)
				{
					RECT rBounds = new RECT(bounds);
					RECT rClip = new RECT(clipRectangle);
					// Draw background
					DrawThemeBackground(rnd.Handle, memoryHdc, rnd.Part, rnd.State, ref rBounds, ref rClip);
				}
			);
		}

		public static void DrawGlassIcon(this VisualStyleRenderer rnd, Graphics g, Rectangle bounds, ImageList imgList, int imgIndex)
		{
			DrawWrapper(rnd, g, bounds,
				delegate(IntPtr memoryHdc)
				{
					RECT rBounds = new RECT(bounds);
					DrawThemeIcon(rnd.Handle, memoryHdc, rnd.Part, rnd.State, ref rBounds, imgList.Handle, imgIndex);
				}
			);
		}

		public static void DrawGlassImage(this VisualStyleRenderer rnd, Graphics g, Rectangle bounds, Image img)
		{
			DrawWrapper(rnd, g, bounds,
				delegate(IntPtr memoryHdc)
				{
					using (Graphics mg = Graphics.FromHdc(memoryHdc))
						mg.DrawImage(img, bounds);
				}
			);
		}

		public static void DrawGlowingText(this VisualStyleRenderer rnd, IDeviceContext dc, Rectangle bounds, string text, Font font, Color color, System.Windows.Forms.TextFormatFlags flags)
		{
			DrawWrapper(rnd, dc, bounds,
				delegate(IntPtr memoryHdc) {
					// Create and select font 
					using (SafeDCObjectHandle fontHandle = new SafeDCObjectHandle(memoryHdc, font.ToHfont()))
					{
						// Draw glowing text 
						DrawThemeTextOptions dttOpts = new DrawThemeTextOptions(true);
						dttOpts.TextColor = color;
						dttOpts.GlowSize = 10;
						dttOpts.AntiAliasedAlpha = true;
						RECT textBounds = new RECT(4, 0, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);
						DrawThemeTextEx(rnd.Handle, memoryHdc, rnd.Part, rnd.State, text, text.Length, (int)flags, ref textBounds, ref dttOpts);
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
							RECT textBounds = new RECT(4, 0, bounds.Right - bounds.Left, bounds.Bottom - bounds.Top);
							DrawThemeTextEx(rnd.Handle, memoryHdc, rnd.Part, rnd.State, text, text.Length, (int)flags, ref textBounds, ref dttOpts);

							// Copy to foreground 
							const int SRCCOPY = 0x00CC0020;
							GDI.BitBlt(primaryHdc, bounds.Left, bounds.Top, bounds.Width, bounds.Height, memoryHdc, 0, 0, SRCCOPY);
						}
					}
				}
			}
		}*/

		public static void DrawText(this VisualStyleRenderer rnd, IDeviceContext dc, ref Rectangle bounds, string text, System.Windows.Forms.TextFormatFlags flags, DrawThemeTextOptions options)
		{
			RECT rc = new RECT(bounds);
			using (SafeGDIHandle hdc = new SafeGDIHandle(dc))
				DrawThemeTextEx(rnd.Handle, hdc, rnd.Part, rnd.State, text, text.Length, (int)flags, ref rc, ref options);
			bounds = rc.ToRectangle();
		}

		public static System.Windows.Forms.Padding GetMargins2(this VisualStyleRenderer rnd, IDeviceContext dc, MarginProperty prop)
		{
			RECT rc;
			using (SafeGDIHandle hdc = new SafeGDIHandle(dc))
				GetThemeMargins(rnd.Handle, hdc, rnd.Part, rnd.State, (int)prop, IntPtr.Zero, out rc);
			return new System.Windows.Forms.Padding(rc.Left, rc.Top, rc.Right, rc.Bottom);
		}

		[DllImport("uxtheme")]
		static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, ref RECT pClipRect);

		[DllImport("uxtheme")]
		static extern int DrawThemeIcon(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, IntPtr himl, int iImageIndex);

		[DllImport("uxtheme", ExactSpelling = true, PreserveSig = false)]
		static extern void GetThemeMargins(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, int iPropId, IntPtr prc, out RECT pMargins);

		[DllImport("uxtheme", CharSet = CharSet.Unicode)]
		static extern int DrawThemeTextEx(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, string text, int iCharCount, int dwFlags, ref RECT pRect, ref DrawThemeTextOptions pOptions);

		[StructLayout(LayoutKind.Sequential)]
		public struct DrawThemeTextOptions
		{
			int dwSize;
			DrawThemeTextOptionsFlags dwFlags;
			int crText;
			int crBorder;
			int crShadow;
			TextShadowType iTextShadowType;
			POINT ptShadowOffset;
			int iBorderSize;
			int iFontPropId;
			int iColorPropId;
			int iStateId;
			bool fApplyOverlay;
			int iGlowSize;
			int pfnDrawTextCallback;
			IntPtr lParam;

			public DrawThemeTextOptions(bool init) : this()
			{
				dwSize = Marshal.SizeOf(typeof(DrawThemeTextOptions));
			}

			public Color TextColor
			{
				get { return ColorTranslator.FromWin32(crText); }
				set
				{
					crText = ColorTranslator.ToWin32(value);
					dwFlags |= DrawThemeTextOptionsFlags.TextColor;
				}
			}

			public Color BorderColor
			{
				get { return ColorTranslator.FromWin32(crBorder); }
				set
				{
					crBorder = ColorTranslator.ToWin32(value);
					dwFlags |= DrawThemeTextOptionsFlags.BorderColor;
				}
			}

			public Color ShadowColor
			{
				get { return ColorTranslator.FromWin32(crShadow); }
				set
				{
					crShadow = ColorTranslator.ToWin32(value);
					dwFlags |= DrawThemeTextOptionsFlags.ShadowColor;
				}
			}

			public TextShadowType ShadowType
			{
				get { return iTextShadowType; }
				set
				{
					iTextShadowType = value;
					dwFlags |= DrawThemeTextOptionsFlags.ShadowType;
				}
			}

			public Point ShadowOffset
			{
				get { return new Point(ptShadowOffset.X, ptShadowOffset.Y); }
				set
				{
					ptShadowOffset = new POINT(value);
					dwFlags |= DrawThemeTextOptionsFlags.ShadowOffset;
				}
			}

			public int BorderSize
			{
				get { return iBorderSize; }
				set
				{
					iBorderSize = value;
					dwFlags |= DrawThemeTextOptionsFlags.BorderSize;
				}
			}

			public DrawThemeTextSystemFonts AlternateFont
			{
				get { return (DrawThemeTextSystemFonts)iFontPropId; }
				set
				{
					iFontPropId = (int)value;
					dwFlags |= DrawThemeTextOptionsFlags.FontProp;
				}
			}

			public bool AntiAliasedAlpha
			{
				get { return (dwFlags & DrawThemeTextOptionsFlags.Composited) == DrawThemeTextOptionsFlags.Composited; }
				set
				{
					if (value)
						dwFlags |= DrawThemeTextOptionsFlags.Composited;
					else
						dwFlags &= ~DrawThemeTextOptionsFlags.Composited;
				}
			}

			public bool ReturnCalculatedRectangle
			{
				get { return (dwFlags & DrawThemeTextOptionsFlags.CalcRect) == DrawThemeTextOptionsFlags.CalcRect; }
				set
				{
					if (value)
						dwFlags |= DrawThemeTextOptionsFlags.CalcRect;
					else
						dwFlags &= ~DrawThemeTextOptionsFlags.CalcRect;
				}
			}

			public Color AlternateColor
			{
				get { return ColorTranslator.FromWin32(iColorPropId); }
				set
				{
					iColorPropId = ColorTranslator.ToWin32(value);
					dwFlags |= DrawThemeTextOptionsFlags.ColorProp;
				}
			}

			public bool ApplyOverlay
			{
				get { return fApplyOverlay; }
				set
				{
					fApplyOverlay = value;
					dwFlags |= DrawThemeTextOptionsFlags.ApplyOverlay;
				}
			}

			public int GlowSize
			{
				get { return iGlowSize; }
				set
				{
					iGlowSize = value;
					dwFlags |= DrawThemeTextOptionsFlags.GlowSize;
				}
			}
		}
	}
}
