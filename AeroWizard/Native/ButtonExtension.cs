
namespace System.Windows.Forms
{
	internal static class ButtonExtension
	{
		public static void SetElevationRequiredState(this ButtonBase btn, bool required = true)
		{
			if (System.Environment.OSVersion.Version.Major >= 6)
			{
				const uint BCM_SETSHIELD = 0x160C;    //Elevated button
				btn.FlatStyle = required ? FlatStyle.System : FlatStyle.Standard;
				Microsoft.Win32.NativeMethods.SendMessage(btn.Handle, BCM_SETSHIELD, IntPtr.Zero, required ? new IntPtr(0xFFFFFFFF) : IntPtr.Zero);
				btn.Invalidate();
			}
			else
				throw new PlatformNotSupportedException();
		}

		public static void SetCueBanner(this ButtonBase btn, string cueBannerText, bool retainOnFocus = false)
		{
			if (System.Environment.OSVersion.Version.Major >= 6)
			{
				const uint EM_SETCUEBANNER = 0x1501;
				Microsoft.Win32.NativeMethods.SendMessage(btn.Handle, EM_SETCUEBANNER, new IntPtr(retainOnFocus ? 1 :0), cueBannerText);
				btn.Invalidate();
			}
			else
				throw new PlatformNotSupportedException();
		}
	}
}