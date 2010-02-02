using System.Runtime.InteropServices;

namespace Microsoft.Win32.DesktopWindowManager
{
    /// <summary>Margins structure for theme related functions.</summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct Margins
    {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;

        public static readonly Margins Empty;
        public static readonly Margins Infinite;

        static Margins()
        {
            Empty = new Margins(0);
            Infinite = new Margins(-1);
        }

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
            return string.Format("{Left={0},Right={1},Top={2},Bottom={3}}", Left, Right, Top, Bottom);
        }

        internal static int RotateLeft(int value, int nBits)
        {
            nBits = nBits % 0x20;
            return ((value << nBits) | (value >> (0x20 - nBits)));
        }
    }
}