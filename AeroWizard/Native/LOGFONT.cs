using System.Drawing;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	internal static partial class NativeMethods
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct LOGFONT
		{
			public int lfHeight;
			public int lfWidth;
			public int lfEscapement;
			public int lfOrientation;
			public int lfWeight;
			public byte lfItalic;
			public byte lfUnderline;
			public byte lfStrikeOut;
			public byte lfCharSet;
			public byte lfOutPrecision;
			public byte lfClipPrecision;
			public byte lfQuality;
			public byte lfPitchAndFamily;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string lfFaceName;

			public Font ToFont()
			{
				try { return Font.FromLogFont(this); }
				catch
				{
					return new Font(lfFaceName, lfHeight, FontStyle.Regular, GraphicsUnit.Display);
				}
			}

			public static LOGFONT FromFont(Font font)
			{
				if (font == null)
					throw new System.ArgumentNullException("font");

				LOGFONT lf = new LOGFONT();
				if (GetObject(font.ToHfont(), Marshal.SizeOf(typeof(LOGFONT)), lf) == 0)
					throw new System.ComponentModel.Win32Exception();

				return lf;
			}

			public override string ToString()
			{
				return string.Concat(new object[] { 
					"lfHeight=", this.lfHeight, ", lfWidth=", this.lfWidth, ", lfEscapement=", this.lfEscapement, ", lfOrientation=", this.lfOrientation,
					", lfWeight=", this.lfWeight, ", lfItalic=", this.lfItalic, ", lfUnderline=", this.lfUnderline, ", lfStrikeOut=", this.lfStrikeOut, 
					", lfCharSet=", this.lfCharSet, ", lfOutPrecision=", this.lfOutPrecision, ", lfClipPrecision=", this.lfClipPrecision, 
					", lfQuality=", this.lfQuality, ", lfPitchAndFamily=", this.lfPitchAndFamily, ", lfFaceName=", this.lfFaceName
				});
			}
		}

		[DllImport(GDI32, ExactSpelling = true, SetLastError = true)]
		public static extern int GetObject(System.IntPtr hFont, int nSize, [In, Out] LOGFONT logfont);
	}
}
