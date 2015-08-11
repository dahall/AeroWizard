using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	internal class SafeGDIHandle : SafeHandle
	{
		private IDeviceContext idc;

		[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
		public SafeGDIHandle(IDeviceContext dc)
			: base(IntPtr.Zero, true)
		{
			if (dc != null)
			{
				idc = dc;
				base.SetHandle(idc.GetHdc());
			}
		}

		public override bool IsInvalid => base.handle == IntPtr.Zero;

		[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
		public static implicit operator IntPtr(SafeGDIHandle h) => h.DangerousGetHandle();

		protected override bool ReleaseHandle()
		{
			if (idc != null)
				idc.ReleaseHdc();
			return true;
		}
	}
}