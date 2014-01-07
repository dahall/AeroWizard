using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	internal enum HResult
	{
		AccessDenied = -2147287035,
		Canceled = -2147023673,
		ElementNotFound = -2147023728,
		Fail = -2147467259,
		False = 1,
		InvalidArguments = -2147024809,
		NoInterface = -2147467262,
		NoObject = -2147221019,
		Ok = 0,
		OutOfMemory = -2147024882,
		ResourceInUse = -2147024726,
		TypeElementNotFound = -2147319765,
		Win32ErrorCanceled = 0x4c7
	}

	internal enum SetTabPropertiesOption
	{
		None = 0,
		UseAppPeekAlways = 4,
		UseAppPeekWhenActive = 8,
		UseAppThumbnailAlways = 1,
		UseAppThumbnailWhenActive = 2
	}

	internal enum TaskbarProgressBarStatus
	{
		Error = 4,
		Indeterminate = 1,
		NoProgress = 0,
		Normal = 2,
		Paused = 8
	}

	internal enum ThumbButtonMask
	{
		Bitmap = 1,
		Icon = 2,
		THB_FLAGS = 8,
		Tooltip = 4
	}

	[Flags]
	internal enum ThumbButtonOptions
	{
		Disabled = 1,
		DismissOnClick = 2,
		Enabled = 0,
		Hidden = 8,
		NoBackground = 4,
		NonInteractive = 0x10
	} 

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct ThumbButton
	{
		internal const int Clicked = 0x1800;
		[MarshalAs(UnmanagedType.U4)]
		internal ThumbButtonMask Mask;
		internal uint Id;
		internal uint Bitmap;
		internal IntPtr Icon;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		internal string Tip;
		[MarshalAs(UnmanagedType.U4)]
		internal ThumbButtonOptions Flags;
	}

	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("c43dc798-95d1-4bea-9030-bb99e2983a1a")]
	internal interface ITaskbarList4
	{
		[PreserveSig]
		void HrInit();
		[PreserveSig]
		void AddTab(IntPtr hwnd);
		[PreserveSig]
		void DeleteTab(IntPtr hwnd);
		[PreserveSig]
		void ActivateTab(IntPtr hwnd);
		[PreserveSig]
		void SetActiveAlt(IntPtr hwnd);
		[PreserveSig]
		void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);
		[PreserveSig]
		void SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);
		[PreserveSig]
		void SetProgressState(IntPtr hwnd, TaskbarProgressBarStatus tbpFlags);
		[PreserveSig]
		void RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);
		[PreserveSig]
		void UnregisterTab(IntPtr hwndTab);
		[PreserveSig]
		void SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);
		[PreserveSig]
		void SetTabActive(IntPtr hwndTab, IntPtr hwndInsertBefore, uint dwReserved);
		[PreserveSig]
		HResult ThumbBarAddButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray)] ThumbButton[] pButtons);
		[PreserveSig]
		HResult ThumbBarUpdateButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray)] ThumbButton[] pButtons);
		[PreserveSig]
		void ThumbBarSetImageList(IntPtr hwnd, IntPtr himl);
		[PreserveSig]
		void SetOverlayIcon(IntPtr hwnd, IntPtr hIcon, [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);
		[PreserveSig]
		void SetThumbnailTooltip(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszTip);
		[PreserveSig]
		void SetThumbnailClip(IntPtr hwnd, IntPtr prcClip);
		void SetTabProperties(IntPtr hwndTab, SetTabPropertiesOption stpFlags);
	}

	[ComImport, Guid("56FDF344-FD6D-11d0-958A-006097C9A090"), ClassInterface(ClassInterfaceType.None)]
	internal class CTaskbarList
	{
	}
}
