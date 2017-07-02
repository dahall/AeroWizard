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
	public abstract class VistaButtonBase : Button
	{
		private Icon icon;
		private bool showShield = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="CommandLink"/> class.
		/// </summary>
		public VistaButtonBase()
		{
			base.FlatStyle = Forms.FlatStyle.System;
		}

		/// <summary>
		/// Gets or sets the flat style.
		/// </summary>
		/// <value>
		/// The flat style.
		/// </value>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(typeof(Forms.FlatStyle), "System")]
		public new Forms.FlatStyle FlatStyle
		{
			get { return base.FlatStyle; }
			set { base.FlatStyle = value; }
		}

		/// <summary>
		/// Gets or sets the icon that is displayed on a button control.
		/// </summary>
		[Description("Gets or sets the icon that is displayed on a button control."), Category("Appearance"), DefaultValue(null)]
		public Icon Icon
		{
			get { return icon; }
			set
			{
				icon = value;
				if (value != null)
					Image = null;
				ShowShield = false;
				SetImage();
			}
		}

		/// <summary>
		/// Gets or sets the image that is displayed on a button control.
		/// </summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[Description("Gets or sets the image that is displayed on a button control."), Category("Appearance"), DefaultValue(null)]
		public new Image Image
		{
			get { return base.Image; }
			set
			{
				base.Image = value;
				if (value != null)
					Icon = null;
				ShowShield = false;
				SetImage();
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to display an elevated shield icon.
		/// </summary>
		/// <value>
		///   <c>true</c> if showing shield icon; otherwise, <c>false</c>.
		/// </value>
		[Description("Gets or sets whether if the control should use an elevated shield icon."), Category("Appearance"), DefaultValue(false)]
		public bool ShowShield
		{
			get { return showShield; }
			set
			{
				if (showShield != value && IsHandleCreated)
				{
					showShield = value;
					SetShield(value);
				}
			}
		}

		internal static bool IsPlatformSupported => Environment.OSVersion.Version.Major >= 6;

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			SetShield(showShield);
			SetImage();
		}

		/// <summary>
		/// Refreshes the image displayed on the button
		/// </summary>
		private void SetImage()
		{
			if (IsHandleCreated)
			{
				IntPtr iconhandle = IntPtr.Zero;
				if (Image != null)
					iconhandle = new Bitmap(Image).GetHicon();
				else if (icon != null)
					iconhandle = Icon.Handle;

				const int BM_SETIMAGE = 0xF7;
				NativeMethods.SendMessage(Handle, BM_SETIMAGE, 1, iconhandle);
			}
		}

		private void SetShield(bool value)
		{
			const uint BCM_SETSHIELD = 0x160C;    //Elevated button
			NativeMethods.SendMessage(Handle, BCM_SETSHIELD, IntPtr.Zero, value ? new IntPtr(1) : IntPtr.Zero);
			Invalidate();
		}
	}
}