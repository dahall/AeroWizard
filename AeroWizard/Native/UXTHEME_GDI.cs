// Requires UXTHEME\UXTHEME.cs
// Requires Gdi\LOGFONT.cs
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	internal static partial class NativeMethods
	{
		[DllImport(UXTHEME, ExactSpelling = true, CharSet = CharSet.Unicode)]
		public static extern Int32 GetThemeFont(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, int iPropId, out LOGFONT pFont);
	}
}