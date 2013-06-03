using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	internal static class GraphicsExtension
	{
		/// <summary>
		/// Draws image with specified paramters. 
		/// </summary>
		/// <param name="graphics">Graphics on which to draw image</param>
		/// <param name="image">Image to be drawn</param>
		/// <param name="destination">Bounding rectangle for the image </param>
		/// <param name="source">Source rectangle of the image</param>
		/// <param name="alignment">Alignment specifying how image will be aligned against the bounding rectangle </param>
		/// <param name="transparency">Transparency for the image </param>
		/// <param name="grayscale">Value indicating if the image should be gray scaled</param>
		public static void DrawImage(this Graphics graphics, Image image, Rectangle destination, Rectangle source, ContentAlignment alignment = ContentAlignment.TopLeft, float transparency = 1.0f, bool grayscale = false)
		{
			if (graphics == null)
				throw new ArgumentNullException("graphics");
			if (image == null)
				throw new ArgumentNullException("image");
			if (destination.IsEmpty)
				throw new ArgumentNullException("destination");
			if (source.IsEmpty)
				throw new ArgumentNullException("source");
			if (transparency < 0 || transparency > 1.0f)
				throw new ArgumentNullException("transparency");

			Rectangle imageRectangle = GetRectangleFromAlignment(alignment, destination, source.Size);

			if (image != null && !imageRectangle.IsEmpty)
			{
				System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix();
				if (grayscale)
				{
					colorMatrix.Matrix00 = 1 / 3f;
					colorMatrix.Matrix01 = 1 / 3f;
					colorMatrix.Matrix02 = 1 / 3f;
					colorMatrix.Matrix10 = 1 / 3f;
					colorMatrix.Matrix11 = 1 / 3f;
					colorMatrix.Matrix12 = 1 / 3f;
					colorMatrix.Matrix20 = 1 / 3f;
					colorMatrix.Matrix21 = 1 / 3f;
					colorMatrix.Matrix22 = 1 / 3f;
				}
				colorMatrix.Matrix33 = transparency; //Alpha factor 

				System.Drawing.Imaging.ImageAttributes imageAttributes = new System.Drawing.Imaging.ImageAttributes();
				imageAttributes.SetColorMatrix(colorMatrix);
				graphics.DrawImage(image, imageRectangle, source.X, source.Y, source.Width, source.Height, GraphicsUnit.Pixel, imageAttributes);
			}
		}

		internal static Rectangle GetRectangleFromAlignment(ContentAlignment alignment, Rectangle destination, Size size)
		{
			if ((alignment & (ContentAlignment.BottomLeft | ContentAlignment.MiddleLeft | ContentAlignment.TopLeft)) != 0)
				destination.Width = Math.Min(size.Width, destination.Width);
			else if ((alignment & (ContentAlignment.BottomRight | ContentAlignment.MiddleRight | ContentAlignment.TopRight)) != 0)
				destination.X = destination.Width - Math.Min(size.Width, destination.Width);
			else
				destination.X = (destination.Width - Math.Min(size.Width, destination.Width)) / 2;

			if ((alignment & (ContentAlignment.TopCenter | ContentAlignment.TopLeft | ContentAlignment.TopRight)) != 0)
				destination.Height = Math.Min(size.Height, destination.Height);
			else if ((alignment & (ContentAlignment.BottomCenter | ContentAlignment.BottomLeft | ContentAlignment.BottomRight)) != 0)
				destination.Y = destination.Height - Math.Min(size.Height, destination.Height);
			else
				destination.Y = (destination.Height - Math.Min(size.Height, destination.Height)) / 2;

			return destination;
		}
	}
}

namespace System.Windows.Forms.VisualStyles.Internal
{
	internal static class GDI
	{
		[DllImport("gdi32", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
		[DllImport("gdi32", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
		[DllImport("gdi32", ExactSpelling = true, SetLastError = true)]
		public static extern bool DeleteObject(IntPtr hObject);
		[DllImport("gdi32", ExactSpelling = true, SetLastError = true)]
		public static extern bool DeleteDC(IntPtr hdc);
		[DllImport("gdi32", ExactSpelling = true, SetLastError = true)]
		public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);
		[DllImport("gdi32", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO pbmi, uint iUsage, int ppvBits, IntPtr hSection, uint dwOffset);
		[DllImport("gdi32", ExactSpelling = true, SetLastError = true)]
		public static extern uint SetLayout(IntPtr hdc, uint dwLayout);
	}

	internal class SafeDCObjectHandle : SafeHandle
	{
		public SafeDCObjectHandle(IntPtr hdc, IntPtr hObj) : base(IntPtr.Zero, true)
		{
			if (hdc != null)
			{
				GDI.SelectObject(hdc, hObj);
				base.SetHandle(hObj);
			}
		}

		public override bool IsInvalid
		{
			get { return base.handle == IntPtr.Zero; }
		}

		public static implicit operator IntPtr(SafeDCObjectHandle h)
		{
			return h.DangerousGetHandle();
		}

		protected override bool ReleaseHandle()
		{
			if (!IsInvalid)
				GDI.DeleteObject(base.handle);
			return true;
		}
	}

	internal class SafeCompatibleDCHandle : SafeHandle
	{
		public SafeCompatibleDCHandle(IntPtr hdc) : base(IntPtr.Zero, true)
		{
			if (hdc != null)
			{
				base.SetHandle(GDI.CreateCompatibleDC(hdc));
			}
		}

		public override bool IsInvalid
		{
			get { return base.handle == IntPtr.Zero; }
		}

		public static implicit operator IntPtr(SafeCompatibleDCHandle h)
		{
			return h.DangerousGetHandle();
		}

		protected override bool ReleaseHandle()
		{
			if (!IsInvalid)
				GDI.DeleteDC(base.handle);
			return true;
		}
	}

	internal class SafeGDIHandle : SafeHandle
	{
		private IDeviceContext idc;

		public SafeGDIHandle(IDeviceContext dc) : base(IntPtr.Zero, true)
		{
			if (dc != null)
			{
				idc = dc;
				base.SetHandle(idc.GetHdc());
			}
		}

		public override bool IsInvalid
		{
			get { return base.handle == IntPtr.Zero; }
		}

		public static implicit operator IntPtr(SafeGDIHandle h)
		{
			return h.DangerousGetHandle();
		}

		protected override bool ReleaseHandle()
		{
			if (idc != null)
				idc.ReleaseHdc();
			return true;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BITMAPINFO
	{
		public int biSize;
		public int biWidth;
		public int biHeight;
		public short biPlanes;
		public short biBitCount;
		public int biCompression;
		public int biSizeImage;
		public int biXPelsPerMeter;
		public int biYPelsPerMeter;
		public int biClrUsed;
		public int biClrImportant;
		public byte bmiColors_rgbBlue;
		public byte bmiColors_rgbGreen;
		public byte bmiColors_rgbRed;
		public byte bmiColors_rgbReserved;

		public BITMAPINFO(int width, int height) : this()
		{
			biSize = Marshal.SizeOf(typeof(BITMAPINFO));
			biWidth = width;
			biHeight = height;
			biPlanes = 1;
			biBitCount = 32;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct POINT
	{
		public int X;
		public int Y;

		public POINT(int x, int y)
		{
			X = x;
			Y = y;
		}

		public POINT(System.Drawing.Point p)
			: this(p.X, p.Y)
		{
		}

		public POINT(System.Drawing.PointF p)
			: this((int)p.X, (int)p.Y)
		{
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct RECT
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;

		public RECT(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public RECT(System.Drawing.Rectangle rect)
			: this(rect.Left, rect.Top, rect.Right, rect.Bottom)
		{
		}

		public int Height
		{
			get { return Bottom - Top; }
			set { Bottom = Top + value; }
		}

		public int Width
		{
			get { return Right - Left; }
			set { Right = Left + value; }
		}

		public System.Drawing.Rectangle ToRectangle()
		{
			return new System.Drawing.Rectangle(Left, Top, Right - Left, Bottom - Top);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct SIZE
	{
		public int width;
		public int height;

		public Size ToSize()
		{
			return new Size(width, height);
		}
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal class LOGFONT
	{
		public int lfHeight = 0;
		public int lfWidth = 0;
		public int lfEscapement = 0;
		public int lfOrientation = 0;
		public int lfWeight = 0;
		public byte lfItalic = 0;
		public byte lfUnderline = 0;
		public byte lfStrikeOut = 0;
		public byte lfCharSet = 0;
		public byte lfOutPrecision = 0;
		public byte lfClipPrecision = 0;
		public byte lfQuality = 0;
		public byte lfPitchAndFamily = 0;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string lfFaceName = string.Empty;

		public Font ToFont()
		{
			return Font.FromLogFont(this);
		}
	}
}