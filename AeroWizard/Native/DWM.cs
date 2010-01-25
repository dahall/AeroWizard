using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AeroWizard.Native
{
    /// <summary>Main DWM class, provides glass sheet effect and blur behind.</summary>
    internal static class DesktopWindowManager
    {
        enum BlurBehindFlags : int
        {
            Enable = 0x00000001,
            BlurRegion = 0x00000002,
            TransitionOnMaximized = 0x00000004
        }

        /// <summary>Enable the Aero "Blur Behind" effect on the whole client area. Background must be black.</summary>
        public static void EnableBlurBehind(IWin32Window window, bool enabled)
        {
            EnableBlurBehind(window, null, null, enabled, false);
        }

        /// <summary>Enable the Aero "Blur Behind" effect on the whole client area. Background must be black.</summary>
        public static void EnableBlurBehind(IWin32Window window, System.Drawing.Graphics graphics, System.Drawing.Region region, bool enabled, bool transitionOnMaximized)
        {
            BlurBehind bb = new BlurBehind(enabled);
            if (graphics != null && region != null)
                bb.SetRegion(graphics, region);
            if (transitionOnMaximized)
                bb.TransitionOnMaximized = true;
            DwmEnableBlurBehindWindow(window.Handle, ref bb);
        }

        public static void ExtendFrameIntoClientArea(IWin32Window window, Margins margins)
        {
            DwmExtendFrameIntoClientArea(window.Handle, ref margins);
        }

        public static bool IsCompositionEnabled()
        {
            int res = 0;
            DwmIsCompositionEnabled(ref res);
            return res != 0;
        }

        [DllImport("dwmapi", ExactSpelling = true, PreserveSig = false)]
        static extern void DwmEnableBlurBehindWindow(IntPtr hWnd, ref BlurBehind pBlurBehind);

        [DllImport("dwmapi", ExactSpelling = true, PreserveSig = false)]
        static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMarInset);

		[DllImport("dwmapi", ExactSpelling = true, PreserveSig = false)]
        static extern void DwmIsCompositionEnabled(ref int pfEnabled);

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

        /*
        /// <summary>Registers a thumbnail to be drawn on a Windows Form.</summary>
        /// <remarks>The thumbnail will not be drawn until you update the thumbnail's properties calling Update().</remarks>
        /// <param name="destination">The Windows Form instance on which to draw the thumbnail.</param>
        /// <param name="source">The handle (HWND) of the window that has to be drawn.</param>
        /// <returns>A Thumbnail instance, needed to unregister and to update properties.</returns>
        public static Thumbnail Register(Form destination, IntPtr source) {
            return Register(destination.Handle, source);
        }

        /// <summary>Registers a thumbnail to be drawn on a window.</summary>
        /// <remarks>The thumbnail will not be drawn until you update the thumbnail's properties calling Update().</remarks>
        /// <param name="destination">The handle (HWND) of the window on which the thumbnail will be drawn.</param>
        /// <param name="source">The handle (HWND) of the window that has to be drawn.</param>
        /// <returns>A Thumbnail instance, needed to unregister and to update properties.</returns>
        public static Thumbnail Register(IntPtr destination, IntPtr source) {
            if (!OsSupport.IsVistaOrBetter)
                throw new DwmCompositionException(Resources.ExceptionMessages.DWMOsNotSupported);

            if (!OsSupport.IsCompositionEnabled)
                throw new DwmCompositionException(Resources.ExceptionMessages.DWMNotEnabled);

            if (destination == source)
                throw new DwmCompositionException(Resources.ExceptionMessages.DWMWindowMatch);

            Thumbnail ret = new Thumbnail();

            if (DwmRegisterThumbnail(destination, source, out ret) == 0) {
                return ret;
            }
            else {
                throw new DwmCompositionException(String.Format(Resources.ExceptionMessages.NativeCallFailure, "DwmRegisterThumbnail"));
            }
        }

        /// <summary>Unregisters the thumbnail handle.</summary>
        /// <remarks>The handle is unvalid after the call and should not be used again.</remarks>
        /// <param name="handle">A handle to a registered thumbnail.</param>
        public static void Unregister(Thumbnail handle) {
            if (handle != null && !handle.IsInvalid) {
                handle.Close();
            }
        }
        */
    }
}