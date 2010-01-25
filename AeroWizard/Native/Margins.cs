using System.Runtime.InteropServices;

namespace AeroWizard
{
    /// <summary>Margins structure for theme related functions.</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Margins
    {
        public int Left;
		public int Right;
		public int Top;
		public int Bottom;

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

		public static readonly Margins Empty;
		public static readonly Margins Infinite;

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		/// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj is Margins)
			{
				Margins m2 = (Margins)obj;
				return Left == m2.Left && Right == m2.Right && Top == m2.Top && Bottom == m2.Bottom;
			}
			return base.Equals(obj);
		}

		public static bool operator ==(Margins m1, Margins m2)
		{
			return m1.Equals(m2);
		}

		public static bool operator !=(Margins m1, Margins m2)
		{
			return !m1.Equals(m2);
		}

		internal static int RotateLeft(int value, int nBits)
		{
			nBits = nBits % 0x20;
			return ((value << nBits) | (value >> (0x20 - nBits)));
		}

		public override int GetHashCode()
		{
			return (((this.Left ^ RotateLeft(this.Top, 8)) ^ RotateLeft(this.Right, 0x10)) ^ RotateLeft(this.Bottom, 0x18));
		}

		public override string ToString()
		{
			return string.Format("{Left={0},Right={1},Top={2},Bottom={3}}", Left, Right, Top, Bottom);
		}
	}
}