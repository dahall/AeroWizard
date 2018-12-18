namespace System.Windows.Forms
{
	internal static class TextBoxExtension
	{
		public static void SetElevationRequiredState(this ButtonBase btn, bool required = true)
		{
			if (System.Environment.OSVersion.Version.Major >= 6)
			{
				const uint BCM_SETSHIELD = 0x160C;    //Elevated button
				btn.FlatStyle = required ? FlatStyle.System : FlatStyle.Standard;
				Vanara.Interop.NativeMethods.SendMessage(btn.Handle, BCM_SETSHIELD, IntPtr.Zero, required ? new IntPtr(1) : IntPtr.Zero);
				btn.Invalidate();
			}
			else
				throw new PlatformNotSupportedException();
		}
	}
}