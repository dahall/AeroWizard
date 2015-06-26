using Microsoft.Win32;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.VisualStyles
{
	internal static partial class VisualStyleRendererExtension
	{
		public static System.Windows.Forms.Padding GetMargins2(this VisualStyleRenderer rnd, IDeviceContext dc, MarginProperty prop)
		{
			NativeMethods.RECT rc;
			using (SafeGDIHandle hdc = new SafeGDIHandle(dc))
				NativeMethods.GetThemeMargins(rnd.Handle, hdc, rnd.Part, rnd.State, (int)prop, IntPtr.Zero, out rc);
			return new System.Windows.Forms.Padding(rc.Left, rc.Top, rc.Right, rc.Bottom);
		}

		public static System.UInt32 GetTransitionDuration(this VisualStyleRenderer rnd, int toState, int fromState = 0)
		{
			System.UInt32 dwDuration = 0;
			NativeMethods.GetThemeTransitionDuration(rnd.Handle, rnd.Part, fromState == 0 ? rnd.State : fromState, toState, (int)Microsoft.Win32.NativeMethods.IntegerListProperty.TransitionDuration, ref dwDuration);
			return dwDuration;
		}

		/// <summary>
		/// Sets attributes to control how visual styles are applied to a specified window.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <param name="attr">The attributes to apply or disable.</param>
		/// <param name="enable">if set to <c>true</c> enable the attribute, otherwise disable it.</param>
		public static void SetWindowThemeAttribute(this IWin32Window window, NativeMethods.WindowThemeNonClientAttributes attr, bool enable = true)
		{
			NativeMethods.WTA_OPTIONS ops = new NativeMethods.WTA_OPTIONS() { Flags = attr, Mask = enable ? (uint)attr : 0 };
			try { NativeMethods.SetWindowThemeAttribute(window.Handle, NativeMethods.WindowThemeAttributeType.WTA_NONCLIENT, ref ops, Marshal.SizeOf(ops)); }
			catch (EntryPointNotFoundException) { }
			catch { throw; }
		}
	}
}