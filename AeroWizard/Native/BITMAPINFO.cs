using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	internal static partial class NativeMethods
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct BITMAPINFO
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

		[DllImport(GDI32, ExactSpelling = true, SetLastError = true)]
		[System.Security.SecurityCritical]
		public static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO pbmi, uint iUsage, IntPtr ppvBits, IntPtr hSection, uint dwOffset);
	}
}