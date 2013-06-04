using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	internal static partial class NativeMethods
	{
		internal const string DWMAPI = "dwmapi.dll";

		public enum BlurBehindFlags : int
		{
			Enable = 0x00000001,
			BlurRegion = 0x00000002,
			TransitionOnMaximized = 0x00000004
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct BlurBehind
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

		[StructLayout(LayoutKind.Sequential)]
		public struct ColorizationParams
		{
			public uint Color1, Color2, Intensity, Unk1, Unk2, Unk3, Opaque;
		}

		/// <summary>Margins structure for theme related functions.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct Margins
		{
			public int Left;
			public int Right;
			public int Top;
			public int Bottom;

			public static readonly Margins Empty = new Margins(0);
			public static readonly Margins Infinite = new Margins(-1);

			public Margins(int left, int right, int top, int bottom)
			{
				Left = left;
				Right = right;
				Top = top;
				Bottom = bottom;
			}

			public Margins(int allMargins)
			{
				Left = Right = Top = Bottom = allMargins;
			}

			public Margins(System.Windows.Forms.Padding padding)
				: this(padding.Left, padding.Right, padding.Top, padding.Bottom)
			{
			}

			public static bool operator !=(Margins m1, Margins m2)
			{
				return !m1.Equals(m2);
			}

			public static bool operator ==(Margins m1, Margins m2)
			{
				return m1.Equals(m2);
			}

			public override bool Equals(object obj)
			{
				if (obj is Margins)
				{
					Margins m2 = (Margins)obj;
					return Left == m2.Left && Right == m2.Right && Top == m2.Top && Bottom == m2.Bottom;
				}
				return base.Equals(obj);
			}

			public override int GetHashCode()
			{
				return (((this.Left ^ RotateLeft(this.Top, 8)) ^ RotateLeft(this.Right, 0x10)) ^ RotateLeft(this.Bottom, 0x18));
			}

			public override string ToString()
			{
				return string.Format("{{Left={0},Right={1},Top={2},Bottom={3}}}", Left, Right, Top, Bottom);
			}

			internal static int RotateLeft(int value, int nBits)
			{
				nBits = nBits % 0x20;
				return ((value << nBits) | (value >> (0x20 - nBits)));
			}
		}

		[DllImport(DWMAPI, EntryPoint = "#127", PreserveSig = false)]
		public static extern void DwmGetColorizationParameters(ref ColorizationParams parameters);

		[DllImport(DWMAPI, EntryPoint = "#131", PreserveSig = false)]
		public static extern void DwmSetColorizationParameters(ref ColorizationParams parameters, uint unk);

		[DllImport(DWMAPI, ExactSpelling = true, PreserveSig = false)]
		public static extern void DwmEnableBlurBehindWindow(IntPtr hWnd, ref BlurBehind pBlurBehind);

		[DllImport(DWMAPI, ExactSpelling = true, PreserveSig = false)]
		public static extern void DwmEnableComposition(int compositionAction);

		[DllImport(DWMAPI, ExactSpelling = true, PreserveSig = false)]
		public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins pMarInset);

		//[DllImport(DWMAPI, ExactSpelling = true, PreserveSig = false)]
		//public static extern void DwmGetColorizationColor(out uint ColorizationColor, [MarshalAs(UnmanagedType.Bool)]out bool ColorizationOpaqueBlend);

		[DllImport(DWMAPI, ExactSpelling = true, PreserveSig = false)]
		public static extern void DwmIsCompositionEnabled(ref int pfEnabled);
	}
}
