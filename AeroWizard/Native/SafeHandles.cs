using System;
using System.Drawing;
using System.Runtime.InteropServices;

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

	internal class GenericSafeHandle : SafeHandle
	{
		private HandleCloser closeMethod;

		public delegate bool HandleCloser(IntPtr ptr);

		public GenericSafeHandle(IntPtr ptr, HandleCloser closeMethod, bool ownsHandle = true)
			: base(ptr, ownsHandle)
		{
			if (closeMethod == null)
				throw new ArgumentNullException("closeMethod");
			this.closeMethod = closeMethod;
		}

		public override bool IsInvalid
		{
			get { return base.handle == IntPtr.Zero; }
		}

		public static implicit operator IntPtr(GenericSafeHandle h)
		{
			return h.DangerousGetHandle();
		}

		protected override bool ReleaseHandle()
		{
			if (!IsInvalid)
				return closeMethod(base.handle);
			return true;
		}
	}

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