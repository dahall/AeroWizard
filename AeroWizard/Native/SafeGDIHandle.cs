using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	internal class SafeGDIHandle : SafeHandle
	{
		private IDeviceContext idc;

		public SafeGDIHandle(IDeviceContext dc)
			: base(IntPtr.Zero, true)
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
}