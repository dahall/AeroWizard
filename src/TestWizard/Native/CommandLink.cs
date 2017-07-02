// Requires USER32.cs
using Microsoft.Win32;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>
	/// Implements a CommandLink button that can be used in WinForms user interfaces.
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(Button))]
	public class CommandLink : VistaButtonBase
	{
		private string note = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="CommandLink"/> class.
		/// </summary>
		public CommandLink()
		{
		}

		/// <summary>
		/// Gets or sets the note text for the button.
		/// </summary>
		/// <value>
		/// The note text.
		/// </value>
		[Category("Appearance"), Browsable(true), DefaultValue((string)null)]
		public string Note
		{
			get { return note; }
			set
			{
				const uint BCM_SETNOTE = 0x1609;
				note = value;
				NativeMethods.SendMessage(Handle, BCM_SETNOTE, IntPtr.Zero, note);
				Invalidate();
			}
		}

		/// <summary>
		/// Gets a System.Windows.Forms.CreateParams on the base class when creating a window.
		/// </summary>
		/// <value>
		/// The create parameters.
		/// </value>
		protected override CreateParams CreateParams
		{
			get
			{
				const int BS_COMMANDLINK = 0xE;
				var cp = base.CreateParams;
				if (IsPlatformSupported)
					cp.Style |= BS_COMMANDLINK;
				return cp;
			}
		}

		/// <summary>
		/// Gets the default size.
		/// </summary>
		/// <value>
		/// The default size.
		/// </value>
		protected override Drawing.Size DefaultSize => new Drawing.Size(135, 60);
	}
}