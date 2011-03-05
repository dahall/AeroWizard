using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Microsoft.Win32.DesktopWindowManager
{
	/// <summary>Main DWM class, provides glass sheet effect and blur behind.</summary>
	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	public static class DesktopWindowManager
	{
		static object ColorizationColorChangedKey = new object();
		static object CompositionChangedKey = new object();
		static EventHandlerList eventHandlerList;
		static object NonClientRenderingChangedKey = new object();
		//static object WindowMaximizedChangedKey = new object();
		static object[] keys = new object[] { CompositionChangedKey, NonClientRenderingChangedKey, ColorizationColorChangedKey/*, WindowMaximizedChangedKey*/ };
		static object _lock = new object();
		static MessageWindow _window;

		enum BlurBehindFlags : int
		{
			Enable = 0x00000001,
			BlurRegion = 0x00000002,
			TransitionOnMaximized = 0x00000004
		}

		/// <summary>
		/// Occurs when the colorization color has changed.
		/// </summary>
		public static event EventHandler ColorizationColorChanged
		{
			add { AddEventHandler(ColorizationColorChangedKey, value); }
			remove { RemoveEventHandler(ColorizationColorChangedKey, value); }
		}

		/// <summary>
		/// Occurs when the desktop window composition has been enabled or disabled.
		/// </summary>
		public static event EventHandler CompositionChanged
		{
			add { AddEventHandler(CompositionChangedKey, value); }
			remove { RemoveEventHandler(CompositionChangedKey, value); }
		}

		/// <summary>
		/// Occurs when the non-client area rendering policy has changed.
		/// </summary>
		public static event EventHandler NonClientRenderingChanged
		{
			add { AddEventHandler(NonClientRenderingChangedKey, value); }
			remove { RemoveEventHandler(NonClientRenderingChangedKey, value); }
		}

		/*/// <summary>
		/// Occurs when a Desktop Window Manager (DWM) composed window is maximized.
		/// </summary>
		public static event EventHandler WindowMaximizedChanged
		{
			add { AddEventHandler(WindowMaximizedChangedKey, value); }
			remove { RemoveEventHandler(WindowMaximizedChangedKey, value); }
		}*/

		/// <summary>
		/// Enable the Aero "Blur Behind" effect on the whole client area. Background must be black.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <param name="enabled"><c>true</c> to enable blur behind for this window, <c>false</c> to disable it.</param>
		public static void EnableBlurBehind(this IWin32Window window, bool enabled)
		{
			EnableBlurBehind(window, null, null, enabled, false);
		}

		/// <summary>
		/// Enable the Aero "Blur Behind" effect on a specific region of a drawing area. Background must be black.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <param name="graphics">The graphics area on which the region resides.</param>
		/// <param name="region">The region within the client area to apply the blur behind.</param>
		/// <param name="enabled"><c>true</c> to enable blur behind for this region, <c>false</c> to disable it.</param>
		/// <param name="transitionOnMaximized"><c>true</c> if the window's colorization should transition to match the maximized windows; otherwise, <c>false</c>.</param>
		public static void EnableBlurBehind(this IWin32Window window, System.Drawing.Graphics graphics, System.Drawing.Region region, bool enabled, bool transitionOnMaximized)
		{
			BlurBehind bb = new BlurBehind(enabled);
			if (graphics != null && region != null)
				bb.SetRegion(graphics, region);
			if (transitionOnMaximized)
				bb.TransitionOnMaximized = true;
			DwmEnableBlurBehindWindow(window.Handle, ref bb);
		}

		/// <summary>
		/// Enables or disables Desktop Window Manager (DWM) composition.
		/// </summary>
		/// <param name="value"><c>true</c> to enable DWM composition; <c>false</c> to disable composition.</param>
		public static void EnableComposition(bool value)
		{
			DwmEnableComposition(value ? 1 : 0);
		}

		/// <summary>
		/// Extends the window frame beyond the client area.
		/// </summary>
		/// <param name="window">The window.</param>
		/// <param name="padding">The padding to use as the area into which the frame is extended.</param>
		public static void ExtendFrameIntoClientArea(this IWin32Window window, Padding padding)
		{
			Margins m = new Margins(padding);
			DwmExtendFrameIntoClientArea(window.Handle, ref m);
		}

		/// <summary>
		/// Indicates whether Desktop Window Manager (DWM) composition is enabled.
		/// </summary>
		/// <returns><c>true</c> if is composition enabled; otherwise, <c>false</c>.</returns>
		public static bool IsCompositionEnabled()
		{
			if (!System.IO.File.Exists(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.System), "dwmapi.dll")))
				return false;
			int res = 0;
			DwmIsCompositionEnabled(ref res);
			return res != 0;
		}

		private static void AddEventHandler(object id, EventHandler value)
		{
			lock (_lock)
			{
				if (_window == null)
					_window = new MessageWindow();
				if (eventHandlerList == null)
					eventHandlerList = new EventHandlerList();
				eventHandlerList.AddHandler(id, value);
			}
		}

		[DllImport("dwmapi", ExactSpelling = true, PreserveSig = false)]
		static extern void DwmEnableBlurBehindWindow(IntPtr hWnd, ref BlurBehind pBlurBehind);

		[DllImport("dwmapi", ExactSpelling = true, PreserveSig = false)]
		static extern void DwmEnableComposition(int compositionAction);

		[DllImport("dwmapi", ExactSpelling = true, PreserveSig = false)]
		static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMarInset);

		[DllImport("dwmapi", ExactSpelling = true, PreserveSig = false)]
		static extern void DwmIsCompositionEnabled(ref int pfEnabled);

		private static void RemoveEventHandler(object id, EventHandler value)
		{
			lock (_lock)
			{
				if (eventHandlerList != null)
				{
					eventHandlerList.RemoveHandler(id, value);
				}
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		struct BlurBehind
		{
			BlurBehindFlags dwFlags;
			int fEnable;
			IntPtr hRgnBlur;
			int fTransitionOnMaximized;

			public BlurBehind(bool enabled)
			{
				fEnable = enabled ? 1 : 0;
				hRgnBlur = IntPtr.Zero;
				fTransitionOnMaximized = 0;
				dwFlags = BlurBehindFlags.Enable;
			}

			public System.Drawing.Region Region
			{
				get { return System.Drawing.Region.FromHrgn(hRgnBlur); }
			}

			public bool TransitionOnMaximized
			{
				get { return fTransitionOnMaximized > 0; }
				set
				{
					fTransitionOnMaximized = value ? 1 : 0;
					dwFlags |= BlurBehindFlags.TransitionOnMaximized;
				}
			}

			public void SetRegion(System.Drawing.Graphics graphics, System.Drawing.Region region)
			{
				hRgnBlur = region.GetHrgn(graphics);
				dwFlags |= BlurBehindFlags.BlurRegion;
			}
		}

		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
		private class MessageWindow : NativeWindow, IDisposable
		{
			const int WM_DWMCOLORIZATIONCOLORCHANGED = 0x0320;
			const int WM_DWMCOMPOSITIONCHANGED = 0x031E;
			const int WM_DWMNCRENDERINGCHANGED = 0x031F;
			//const int WM_DWMWINDOWMAXIMIZEDCHANGE = 0x0321;

			public MessageWindow()
			{
				CreateParams cp = new CreateParams() { Style = 0, ExStyle = 0, ClassStyle = 0, Parent = IntPtr.Zero };
				cp.Caption = base.GetType().Name;
				this.CreateHandle(cp);
			}

			public void Dispose()
			{
				this.DestroyHandle();
			}

			[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
			protected override void WndProc(ref Message m)
			{
				if (m.Msg >= WM_DWMCOMPOSITIONCHANGED && m.Msg <= WM_DWMCOLORIZATIONCOLORCHANGED)
					ExecuteEvents(m.Msg - WM_DWMCOMPOSITIONCHANGED);

				base.WndProc(ref m);
			}

			private void ExecuteEvents(int idx)
			{
				if (eventHandlerList != null)
				{
					lock (_lock)
					{
						try { ((EventHandler)eventHandlerList[keys[idx]]).Invoke(null, EventArgs.Empty); }
						catch { };
					}
				}
			}
		}
	}
}