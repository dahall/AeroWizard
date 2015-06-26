using System;

namespace Microsoft.Win32
{
	internal static partial class NativeMethods
	{
		public class SafeDCObjectHandle : GenericSafeHandle
		{
			public SafeDCObjectHandle(IntPtr hdc, IntPtr hObj): base(IntPtr.Zero, NativeMethods.DeleteObject, true)
			{
				if (hdc != null)
				{
					NativeMethods.SelectObject(hdc, hObj);
					base.SetHandle(hObj);
				}
			}
		}

		public class SafeCompatibleDCHandle : GenericSafeHandle
		{
			public SafeCompatibleDCHandle(IntPtr hdc) : base(IntPtr.Zero, NativeMethods.DeleteDC, true)
			{
				if (hdc != null)
				{
					base.SetHandle(NativeMethods.CreateCompatibleDC(hdc));
				}
			}
		}
	}
}