using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AeroWizard
{
	/// <summary>
	/// Provides data for the <see cref="E:StepList.DrawItem"/> event.
	/// </summary>
	public class DrawStepListItemEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DrawStepListItemEventArgs"/> class.
		/// </summary>
		/// <param name="graphics">The graphics surface on which to draw.</param>
		/// <param name="itemRect">The rectangle within which to draw.</param>
		/// <param name="page">The page to which this step refers.</param>
		/// <param name="isSelected">if set to <c>true</c> item is selected.</param>
		/// <param name="isCompleted">if set to <c>true</c> item has been completed.</param>
		public DrawStepListItemEventArgs(Graphics graphics, Rectangle itemRect, WizardPage page, bool isSelected, bool isCompleted)
		{
			this.Graphics = graphics;
			this.Bounds = itemRect;
			this.Item = page;
			this.Selected = isSelected;
			this.Completed = isCompleted;
		}

		/// <summary>
		/// Gets the size and location of the item to draw.
		/// </summary>
		/// <value>
		/// A rectangle that represents the bounds of the item to draw.
		/// </value>
		public Rectangle Bounds { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this step has already been completed.
		/// </summary>
		/// <value>
		///   <c>true</c> if completed; otherwise, <c>false</c>.
		/// </value>
		public bool Completed { get; private set; }

		/// <summary>
		/// Gets the <see cref="Graphics"/> used to draw the item.
		/// </summary>
		/// <value>
		/// The <see cref="Graphics"/> used to draw the item.
		/// </value>
		public Graphics Graphics { get; private set; }

		/// <summary>
		/// Gets the <see cref="WizardPage"/> to which this item refers.
		/// </summary>
		/// <value>
		/// The <see cref="WizardPage"/> to which this item refers.
		/// </value>
		public WizardPage Item { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this item is the one currently selected.
		/// </summary>
		/// <value>
		///   <c>true</c> if selected; otherwise, <c>false</c>.
		/// </value>
		public bool Selected { get; private set; }
	}

	/// <summary>
	/// Provides data for the <see cref="E:StepList.MeasureItem"/> event.
	/// </summary>
	public class MeasureStepListItemEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MeasureStepListItemEventArgs"/> class.
		/// </summary>
		/// <param name="graphics">The graphics surface on which to draw.</param>
		/// <param name="page">The page to which this step refers.</param>
		/// <param name="itemSize">The size of the item.</param>
		public MeasureStepListItemEventArgs(Graphics graphics, WizardPage page, Size itemSize)
		{
			this.Graphics = graphics;
			this.Item = page;
			this.ItemSize = itemSize;
		}

		/// <summary>
		/// Gets the <see cref="Graphics"/> used to draw the item.
		/// </summary>
		/// <value>
		/// The <see cref="Graphics"/> used to draw the item.
		/// </value>
		public Graphics Graphics { get; private set; }

		/// <summary>
		/// Gets the <see cref="WizardPage"/> to which this item refers.
		/// </summary>
		/// <value>
		/// The <see cref="WizardPage"/> to which this item refers.
		/// </value>
		public WizardPage Item { get; private set; }

		/// <summary>
		/// Gets or sets the size of the item.
		/// </summary>
		/// <value>
		/// The size of the item.
		/// </value>
		public Size ItemSize { get; set; }
	}

	/// <summary>
	/// Wizard control that shows a step summary on the left of the wizard page area.
	/// </summary>
	[ProvideProperty("StepText", typeof(WizardPage))]
	[ProvideProperty("StepTextIndentLevel", typeof(WizardPage))]
	public class StepWizardControl : WizardControl, IExtenderProvider
	{
		private StepList list;
		private Splitter splitter;

		/// <summary>
		/// Initializes a new instance of the <see cref="StepWizardControl"/> class.
		/// </summary>
		public StepWizardControl()
		{
			var ds = this.RightToLeft == System.Windows.Forms.RightToLeft.Yes ? DockStyle.Right : DockStyle.Left;
			this.pageContainer.Controls.Add(splitter = new Splitter() { Dock = ds, BorderStyle = BorderStyle.FixedSingle, Width = 1, Name = "splitter" });
			this.pageContainer.Controls.Add(list = new StepList() { Dock = ds, Name = "stepList" });
			list.DrawItem += list_DrawItem;
			list.MeasureItem += list_MeasureItem;
			this.Pages.Reset += Pages_Reset;
		}

		/// <summary>
		/// Occurs when a visual aspect of an owner-drawn StepList changes.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler<DrawStepListItemEventArgs> DrawStepListItem;

		/// <summary>
		/// Occurs when an owner-drawn StepList is created and the sizes of the list items are determined.
		/// </summary>
		[Category("Appearance")]
		public event EventHandler<MeasureStepListItemEventArgs> MeasureStepListItem;

		/// <summary>
		/// Gets or sets a value indicating whether the StepWizardControl step list is drawn by the operating system or by code that you provide.
		/// </summary>
		/// <value>
		///   <c>true</c> if the StepWizardControl step list is drawn by code that you provide; otherwise, <c>false</c>.
		/// </value>
		[DefaultValue(false), Category("Appearance"), Description("Indicates if step list items are drawn by owner.")]
		public bool OwnerDrawStepList
		{
			get { return list.OwnerDraw; }
			set { list.OwnerDraw = value; }
		}

		/// <summary>
		/// Gets or sets the width of the step list.
		/// </summary>
		/// <value>
		/// The width of the step list.
		/// </value>
		[DefaultValue(150), Category("Appearance"), Description("Determines width of step list on left.")]
		public int StepListWidth
		{
			get { return list.Width; }
			set { list.Width = value; }
		}

		/// <summary>
		/// Gets the step text.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <returns>Step text for the specified wizard page.</returns>
		[DefaultValue((string)null), Category("Appearance"), Description("Alternate text to provide to the StepList. Default value comes the Text property of the WizardPage.")]
		public string GetStepText(WizardPage page)
		{
			return list.GetStepText(page);
		}

		/// <summary>
		/// Gets the step text indent level.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <returns>Step text indent level for the specified wizard page.</returns>
		[DefaultValue(0), Category("Appearance"), Description("Indentation level for text provided to the StepList.")]
		public int GetStepTextIndentLevel(WizardPage page)
		{
			return list.GetStepTextIndentLevel(page);
		}

		/// <summary>
		/// Specifies whether this object can provide its extender properties to the specified object.
		/// </summary>
		/// <param name="extendee">The <see cref="T:System.Object" /> to receive the extender properties.</param>
		/// <returns>
		/// true if this object can provide extender properties to the specified object; otherwise, false.
		/// </returns>
		bool IExtenderProvider.CanExtend(object extendee)
		{
			return (extendee is WizardPage);
		}

		/// <summary>
		/// Sets the step text.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="value">The value.</param>
		public void SetStepText(WizardPage page, string value)
		{
			list.SetStepText(page, value);
		}

		/// <summary>
		/// Sets the step text indent level.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="value">The indent level.</param>
		public void SetStepTextIndentLevel(WizardPage page, int value)
		{
			list.SetStepTextIndentLevel(page, value);
		}

		/// <summary>
		/// Raises the <see cref="E:DrawStepListItem" /> event.
		/// </summary>
		/// <param name="e">The <see cref="DrawStepListItemEventArgs"/> instance containing the event data.</param>
		protected virtual void OnDrawStepListItem(DrawStepListItemEventArgs e)
		{
			var h = this.DrawStepListItem;
			if (h != null)
				h(this, e);
		}

		/// <summary>
		/// Raises the <see cref="E:MeasureStepListItem" /> event.
		/// </summary>
		/// <param name="e">The <see cref="MeasureStepListItemEventArgs"/> instance containing the event data.</param>
		protected virtual void OnMeasureStepListItem(MeasureStepListItemEventArgs e)
		{
			var h = this.MeasureStepListItem;
			if (h != null)
				h(this, e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.RightToLeftChanged" /> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected override void OnRightToLeftChanged(System.EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			var ds = this.RightToLeft == System.Windows.Forms.RightToLeft.Yes ? DockStyle.Right : DockStyle.Left;
			if (this.pageContainer.Controls.Count > 1)
			{
				this.pageContainer.Controls["splitter"].Dock = ds;
				this.pageContainer.Controls["stepList"].Dock = ds;
			}
		}

		private void list_DrawItem(object sender, DrawStepListItemEventArgs e)
		{
			OnDrawStepListItem(e);
		}

		private void list_MeasureItem(object sender, MeasureStepListItemEventArgs e)
		{
			OnMeasureStepListItem(e);
		}

		void Pages_Reset(object sender, System.Collections.Generic.EventedList<WizardPage>.ListChangedEventArgs<WizardPage> e)
		{
			this.pageContainer.Controls.Add(splitter);
			this.pageContainer.Controls.Add(list);
		}

		private void ResetStepText(WizardPage page)
		{
			SetStepText(page, null);
		}

		private bool ShouldSerializeStepText(WizardPage page)
		{
			return (GetStepText(page) != page.Text);
		}
	}
}