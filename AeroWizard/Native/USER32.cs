using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	internal static partial class NativeMethods
	{
		internal const string USER32 = "user32.dll";

		[DllImport(USER32, ExactSpelling = true)]
		[System.Security.SecurityCritical]
		public static extern IntPtr GetActiveWindow();

		[DllImport(USER32, CharSet = CharSet.Auto, ExactSpelling = true)]
		[System.Security.SecurityCritical]
		public static extern IntPtr ChildWindowFromPointEx(IntPtr hwndParent, ref System.Drawing.Point pt, System.Windows.Forms.GetChildAtPointSkip uFlags);

		[DllImport(USER32, CharSet = CharSet.Auto, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[System.Security.SecurityCritical]
		public static extern bool GetClientRect(IntPtr hWnd, [In, Out] ref NativeMethods.RECT rect);

		public static IntPtr GetWindowLong(IntPtr hWnd, int nIndex)
		{
			IntPtr ret = IntPtr.Zero;
			if (IntPtr.Size == 4)
				ret = (IntPtr)GetWindowLong32(hWnd, nIndex);
			else
				ret = GetWindowLongPtr(hWnd, nIndex);
			if (ret == IntPtr.Zero)
				throw new System.ComponentModel.Win32Exception();
			return ret;
		}

		[DllImport(USER32, EntryPoint = "GetWindowLong", SetLastError = true)]
		[SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return", Justification = "This declaration is not used on 64-bit Windows.")]
		[SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "2", Justification = "This declaration is not used on 64-bit Windows.")]
		[System.Security.SecurityCritical]
		public static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

		[DllImport(USER32, EntryPoint = "GetWindowLongPtr", SetLastError = true)]
		[SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist", Justification = "Entry point does exist on 64-bit Windows.")]
		[System.Security.SecurityCritical]
		public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

		[DllImport(USER32, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[System.Security.SecurityCritical]
		public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[DllImport(USER32, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[System.Security.SecurityCritical]
		public static extern bool InvalidateRect(IntPtr hWnd, [In] ref NativeMethods.RECT rect, [MarshalAs(UnmanagedType.Bool)] bool bErase);

		[DllImport(USER32, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[System.Security.SecurityCritical]
		public static extern bool InvalidateRect(IntPtr hWnd, IntPtr rect, [MarshalAs(UnmanagedType.Bool)] bool bErase);

		[DllImport(USER32, ExactSpelling = true, CharSet = CharSet.Auto)]
		[System.Security.SecurityCritical]
		public static extern int LoadString(IntPtr hInstance, int uID, out IntPtr lpBuffer, int nBufferMax);

		[DllImport(USER32, CharSet = CharSet.Auto, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[System.Security.SecurityCritical]
		public static extern bool ScreenToClient(IntPtr hWnd, [In, Out] ref System.Drawing.Point lpPoint);

		[DllImport(USER32, CharSet = CharSet.Unicode, SetLastError = false)]
		[System.Security.SecurityCritical]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport(USER32, CharSet = CharSet.Unicode, SetLastError = false)]
		[System.Security.SecurityCritical]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, [MarshalAs(UnmanagedType.SysInt)] int wParam, IntPtr lParam);

		[DllImport(USER32, CharSet = CharSet.Unicode, SetLastError = false)]
		[System.Security.SecurityCritical]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, [MarshalAs(UnmanagedType.SysInt)] int wParam, [MarshalAs(UnmanagedType.SysInt)] int lParam);

		public static IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam = 0) => SendMessage(hWnd, Msg, wParam, 0);

		[DllImport(USER32, CharSet = CharSet.Unicode, SetLastError = false)]
		[System.Security.SecurityCritical]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, ref RECT rect);

		[DllImport(USER32, CharSet = CharSet.Unicode, SetLastError = false)]
		[System.Security.SecurityCritical]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, [In, MarshalAs(UnmanagedType.LPWStr)] string lParam);

		[DllImport(USER32, CharSet = CharSet.Unicode, SetLastError = false)]
		[System.Security.SecurityCritical]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, ref int wParam, [In, Out, MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder lParam);

		public static IntPtr SetWindowLong(IntPtr hWnd, Int32 nIndex, IntPtr dwNewLong)
		{
			IntPtr ret = IntPtr.Zero;
			if (IntPtr.Size == 4)
				ret = SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
			else
				ret = SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
			if (ret == IntPtr.Zero)
				throw new System.ComponentModel.Win32Exception();
			return ret;
		}

		[DllImport(USER32, SetLastError = true, EntryPoint = "SetWindowLong")]
		[SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return", Justification = "This declaration is not used on 64-bit Windows.")]
		[SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "2", Justification = "This declaration is not used on 64-bit Windows.")]
		private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, Int32 nIndex, IntPtr dwNewLong);

		[DllImport(USER32, SetLastError = true, EntryPoint = "SetWindowLongPtr")]
		[SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist", Justification = "Entry point does exist on 64-bit Windows.")]
		private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, Int32 nIndex, IntPtr dwNewLong);

		/// <summary>
		/// Special window handles
		/// </summary>
		public static class SpecialWindowHandles
		{
			/// <summary>Places the window at the top of the Z order.</summary>
			/// <remarks>HWND_TOP</remarks>
			public static IntPtr HwndTop = new IntPtr(0);
			/// <summary>Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost window, the window loses its topmost status and is placed at the bottom of all other windows.</summary>
			/// <remarks>HWND_BOTTOM</remarks>
			public static IntPtr HwndBottom = new IntPtr(1);
			/// <summary>Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.</summary>
			/// <remarks>HWND_TOPMOST</remarks>
			public static IntPtr HwndTopMost = new IntPtr(-1);
			/// <summary>Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.</summary>
			/// <remarks>HWND_NOTOPMOST</remarks>
			public static IntPtr HwndNoTopMost = new IntPtr(-2);
		}

		/// <summary>
		/// Window sizing and positioning flags.
		/// </summary>
		[Flags]
		public enum SetWindowPosFlags : uint
		{
			/// <summary>If the calling thread and the thread that owns the window are attached to different input queues, 
			/// the system posts the request to the thread that owns the window. This prevents the calling thread from 
			/// blocking its execution while other threads process the request.</summary>
			/// <remarks>SWP_ASYNCWINDOWPOS</remarks>
			AsynchronousWindowPosition = 0x4000,
			/// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
			/// <remarks>SWP_DEFERERASE</remarks>
			DeferErase = 0x2000,
			/// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
			/// <remarks>SWP_DRAWFRAME</remarks>
			DrawFrame = 0x0020,
			/// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to 
			/// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE 
			/// is sent only when the window's size is being changed.</summary>
			/// <remarks>SWP_FRAMECHANGED</remarks>
			FrameChanged = 0x0020,
			/// <summary>Hides the window.</summary>
			/// <remarks>SWP_HIDEWINDOW</remarks>
			HideWindow = 0x0080,
			/// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the 
			/// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter 
			/// parameter).</summary>
			/// <remarks>SWP_NOACTIVATE</remarks>
			DoNotActivate = 0x0010,
			/// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid 
			/// contents of the client area are saved and copied back into the client area after the window is sized or 
			/// repositioned.</summary>
			/// <remarks>SWP_NOCOPYBITS</remarks>
			DoNotCopyBits = 0x0100,
			/// <summary>Retains the current position (ignores X and Y parameters).</summary>
			/// <remarks>SWP_NOMOVE</remarks>
			IgnoreMove = 0x0002,
			/// <summary>Does not change the owner window's position in the Z order.</summary>
			/// <remarks>SWP_NOOWNERZORDER</remarks>
			DoNotChangeOwnerZOrder = 0x0200,
			/// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to 
			/// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent 
			/// window uncovered as a result of the window being moved. When this flag is set, the application must 
			/// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
			/// <remarks>SWP_NOREDRAW</remarks>
			DoNotRedraw = 0x0008,
			/// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
			/// <remarks>SWP_NOREPOSITION</remarks>
			DoNotReposition = 0x0200,
			/// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
			/// <remarks>SWP_NOSENDCHANGING</remarks>
			DoNotSendChangingEvent = 0x0400,
			/// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
			/// <remarks>SWP_NOSIZE</remarks>
			IgnoreResize = 0x0001,
			/// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
			/// <remarks>SWP_NOZORDER</remarks>
			IgnoreZOrder = 0x0004,
			/// <summary>Displays the window.</summary>
			/// <remarks>SWP_SHOWWINDOW</remarks>
			ShowWindow = 0x0040,
		}

		[DllImport(USER32, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[System.Security.SecurityCritical]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

		[DllImport(USER32, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		[System.Security.SecurityCritical]
		public static extern bool SetWindowText(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string lpString);

		[StructLayout(LayoutKind.Sequential)]
		public struct WINDOWPOS
		{
			public IntPtr hwnd;
			public IntPtr hwndInsertAfter;
			public int x;
			public int y;
			public int cx;
			public int cy;
			public int flags;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct NMHDR
		{
			public IntPtr hwndFrom;
			public IntPtr idFrom;
			public int code;
		}
	}
}