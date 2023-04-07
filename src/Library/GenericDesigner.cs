#if !NETFRAMEWORK
using Microsoft.DotNet.DesignTools.Designers;
using Microsoft.DotNet.DesignTools.Designers.Actions;
using Microsoft.DotNet.DesignTools.Designers.Behaviors;
using Microsoft.DotNet.DesignTools.Editors;
#endif
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
#if NETFRAMEWORK
using System.Windows.Forms.Design.Behavior;
#endif

namespace System.ComponentModel.Design
{
	internal static class ComponentDesignerExtension
	{
		public const BindingFlags allInstBind = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

		public static object EditValue(this ComponentDesigner designer, object objectToChange, string propName)
		{
			var prop = TypeDescriptor.GetProperties(objectToChange)[propName];
			var context = new EditorServiceContext(designer, prop);
			var editor = prop.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
			if (editor == null) return null;
			var curVal = prop.GetValue(objectToChange);
			var newVal = editor.EditValue(context, context, curVal);
			if (newVal != curVal)
				try
				{
					prop.SetValue(objectToChange, newVal);
				}
				catch (CheckoutException) { }
			return newVal;
		}

		public static List<DesignerActionItem> GetAllAttributedActionItems(this DesignerActionList actionList)
		{
			var fullAIList = new List<DesignerActionItem>();
			foreach (var mbr in actionList.GetType().GetMethods(allInstBind))
			{
				foreach (IActionGetItem attr in mbr.GetCustomAttributes(typeof(DesignerActionMethodAttribute), false))
				{
					if (mbr.ReturnType == typeof(void) && mbr.GetParameters().Length == 0)
						fullAIList.Add(attr.GetItem(actionList, mbr));
					else
						throw new FormatException("DesignerActionMethodAttribute must be applied to a method returning void and having no parameters.");
				}
			}
			foreach (var mbr in actionList.GetType().GetProperties(allInstBind))
			{
				foreach (IActionGetItem attr in mbr.GetCustomAttributes(typeof(DesignerActionPropertyAttribute), false))
					fullAIList.Add(attr.GetItem(actionList, mbr));
			}
			fullAIList.Sort(CompareItems);
			return fullAIList;
		}

		public static DesignerVerbCollection GetAttributedVerbs(this ComponentDesigner designer)
		{
			var verbs = new DesignerVerbCollection();
			foreach (var m in designer.GetType().GetMethods(allInstBind))
			{
				foreach (DesignerVerbAttribute attr in m.GetCustomAttributes(typeof(DesignerVerbAttribute), true))
				{
					verbs.Add(attr.GetDesignerVerb(designer, m));
				}
			}
			return verbs;
		}

		public static DesignerActionItemCollection GetFilteredActionItems(this DesignerActionList actionList, List<DesignerActionItem> fullAIList)
		{
			var col = new DesignerActionItemCollection();
			fullAIList.ForEach(ai => { if (CheckCondition(actionList, ai)) { col.Add(ai); } });

			// Add header items for displayed items
			var i = 0; string cat = null;
			while (i < col.Count)
			{
				var curCat = col[i].Category;
				if (string.Compare(curCat, cat, true, CultureInfo.CurrentCulture) != 0)
				{
					col.Insert(i++, new DesignerActionHeaderItem(curCat));
					cat = curCat;
				}
				i++;
			}

			return col;
		}

		public static IDictionary<string, List<Attribute>> GetRedirectedProperties(this ComponentDesigner d)
		{
			var ret = new Dictionary<string, List<Attribute>>();
			foreach (var prop in d.GetType().GetProperties(allInstBind))
			{
				foreach (RedirectedDesignerPropertyAttribute attr in prop.GetCustomAttributes(typeof(RedirectedDesignerPropertyAttribute), false))
				{
					List<Attribute> attributes;
					if (attr.ApplyOtherAttributes)
					{
						attributes = new List<Attribute>(Array.ConvertAll(prop.GetCustomAttributes(false), o => o as Attribute));
						attributes.RemoveAll(a => a is RedirectedDesignerPropertyAttribute);
					}
					else
						attributes = new List<Attribute>();
					ret.Add(prop.Name, attributes);
				}
			}
			return ret;
		}

		public static void RedirectRegisteredProperties(this ComponentDesigner d, IDictionary properties, IDictionary<string, List<Attribute>> redirectedProps)
		{
			foreach (var propName in redirectedProps.Keys)
			{
				var oldPropertyDescriptor = (PropertyDescriptor)properties[propName];
				if (oldPropertyDescriptor != null)
				{
					var attributes = redirectedProps[propName];
					properties[propName] = TypeDescriptor.CreateProperty(d.GetType(), oldPropertyDescriptor, attributes.ToArray());
				}
			}
		}

		public static void RemoveProperties(this ComponentDesigner d, IDictionary properties, IEnumerable<string> propertiesToRemove)
		{
			foreach (var p in propertiesToRemove)
				if (properties.Contains(p))
					properties.Remove(p);
		}

		public static void SetComponentProperty<T>(this ComponentDesigner d, string propName, T value)
		{
			var propDesc = TypeDescriptor.GetProperties(d.Component)[propName];
			if (propDesc != null && propDesc.PropertyType == typeof(T) && !propDesc.IsReadOnly && propDesc.IsBrowsable)
				propDesc.SetValue(d.Component, value);
		}

		public static DialogResult ShowDialog(this ComponentDesigner designer, Form dialog)
		{
			var context = new EditorServiceContext(designer);
			return context.ShowDialog(dialog);
		}

		private static bool CheckCondition(DesignerActionList actionList, DesignerActionItem ai)
		{
			if (ai.Properties["Condition"] != null)
			{
				var p = actionList.GetType().GetProperty((string)ai.Properties["Condition"], allInstBind, null, typeof(bool), Type.EmptyTypes, null);
				if (p != null)
					return (bool)p.GetValue(actionList, null);
			}
			return true;
		}

		private static int CompareItems(DesignerActionItem a, DesignerActionItem b)
		{
			var c = string.Compare(a.Category ?? string.Empty, b.Category ?? string.Empty, true, CultureInfo.CurrentCulture);
			if (c != 0)
				return c;
			c = (int)a.Properties["Order"] - (int)b.Properties["Order"];
			return c != 0 ? c : string.Compare(a.DisplayName, b.DisplayName, true, CultureInfo.CurrentCulture);
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	internal sealed class RedirectedDesignerPropertyAttribute : Attribute
	{
		public RedirectedDesignerPropertyAttribute() => ApplyOtherAttributes = true;

		public bool ApplyOtherAttributes { get; set; }
	}
}

namespace System.Windows.Forms.Design
{
	internal interface IActionGetItem
	{
		string Category { get; }

		DesignerActionItem GetItem(DesignerActionList actions, MemberInfo mbr);
	}

	internal abstract class BaseDesignerActionList : DesignerActionList
	{
		private List<DesignerActionItem> fullAIList;

		protected BaseDesignerActionList(ComponentDesigner designer, IComponent component)
			: base(component)
		{
			AutoShow = true;
			ParentDesigner = designer;
		}

		public ComponentDesigner ParentDesigner { get; }

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			// Retrieve all attributed methods and properties
			fullAIList ??= this.GetAllAttributedActionItems();

			// Filter for conditions and load
			return this.GetFilteredActionItems(fullAIList);
		}

		protected T GetComponentProperty<T>(string propName)
		{
			var p = ComponentProp(propName, typeof(T));
			return p != null ? (T)p.GetValue(Component, null) : default(T);
		}

		protected void SetComponentProperty<T>(string propName, T value)
		{
			var p = ComponentProp(propName, typeof(T));
			p?.SetValue(Component, value, null);
		}

		private PropertyInfo ComponentProp(string propName, Type retType) => Component.GetType().GetProperty(propName, ComponentDesignerExtension.allInstBind, null, retType, Type.EmptyTypes, null);
	}

	[AttributeUsage(AttributeTargets.Method)]
	internal sealed class DesignerActionMethodAttribute : Attribute, IActionGetItem
	{
		public DesignerActionMethodAttribute(string displayName, int displayOrder = 0)
		{
			DisplayName = displayName;
			DisplayOrder = displayOrder;
		}

		public bool AllowAssociate { get; set; }

		public string Category { get; set; }

		public string Condition { get; set; }

		public string Description { get; set; }

		public string DisplayName { get; }

		public int DisplayOrder { get; }

		public bool IncludeAsDesignerVerb { get; set; }

		DesignerActionItem IActionGetItem.GetItem(DesignerActionList actions, MemberInfo mbr)
		{
			var ret = new DesignerActionMethodItem(actions, mbr.Name, DisplayName, Category, Description, IncludeAsDesignerVerb)
			{ AllowAssociate = AllowAssociate };
			if (!string.IsNullOrEmpty(Condition))
				ret.Properties.Add("Condition", Condition);
			ret.Properties.Add("Order", DisplayOrder);
			return ret;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	internal sealed class DesignerActionPropertyAttribute : Attribute, IActionGetItem
	{
		public DesignerActionPropertyAttribute(string displayName, int displayOrder = 0)
		{
			DisplayName = displayName;
			DisplayOrder = displayOrder;
		}

		public bool AllowAssociate { get; set; }

		public string Category { get; set; }

		public string Condition { get; set; }

		public string Description { get; set; }

		public string DisplayName { get; }

		public int DisplayOrder { get; }

		DesignerActionItem IActionGetItem.GetItem(DesignerActionList actions, MemberInfo mbr)
		{
			var ret = new DesignerActionPropertyItem(mbr.Name, DisplayName, Category, Description)
			{ AllowAssociate = AllowAssociate };
			if (!string.IsNullOrEmpty(Condition))
				ret.Properties.Add("Condition", Condition);
			ret.Properties.Add("Order", DisplayOrder);
			return ret;
		}
	}

	[AttributeUsage(AttributeTargets.Method)]
	internal sealed class DesignerVerbAttribute : Attribute
	{
		private readonly CommandID cmdId;
		private readonly string menuText;

		public DesignerVerbAttribute(string menuText) => this.menuText = menuText;

		public DesignerVerbAttribute(string menuText, Guid commandMenuGroup, int commandId)
		{
			this.menuText = menuText;
			cmdId = new CommandID(commandMenuGroup, commandId);
		}

		internal DesignerVerb GetDesignerVerb(object obj, MethodInfo mi)
		{
			var handler = (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), obj, mi);
			return cmdId != null ? new DesignerVerb(menuText, handler, cmdId) : new DesignerVerb(menuText, handler);
		}
	}

	internal class EditorServiceContext : IWindowsFormsEditorService, ITypeDescriptorContext
	{
		private readonly ComponentDesigner designer;
		private readonly PropertyDescriptor targetProperty;
		private IComponentChangeService componentChangeSvc;

		internal EditorServiceContext(ComponentDesigner designer) => this.designer = designer;

		internal EditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop)
		{
			this.designer = designer;
			targetProperty = prop;
			if (prop == null)
			{
				prop = TypeDescriptor.GetDefaultProperty(designer.Component);
				if (prop != null && typeof(ICollection).IsAssignableFrom(prop.PropertyType))
					targetProperty = prop;
			}
		}

		internal EditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop, string newVerbText)
			: this(designer, prop) => this.designer.Verbs.Add(new DesignerVerb(newVerbText, OnEditItems));

		IContainer ITypeDescriptorContext.Container => designer.Component.Site?.Container;

		object ITypeDescriptorContext.Instance => designer.Component;

		PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor => targetProperty;

		private IComponentChangeService ChangeService => componentChangeSvc ??= GetService<IComponentChangeService>();

		public DialogResult ShowDialog(Form dialog)
		{
			if (dialog == null)
				throw new ArgumentNullException(nameof(dialog));
			var service = GetService<IUIService>();
			return service != null ? service.ShowDialog(dialog) : dialog.ShowDialog(designer.Component as IWin32Window);
		}

		void IWindowsFormsEditorService.CloseDropDown()
		{
		}

		void IWindowsFormsEditorService.DropDownControl(Control control)
		{
		}

		object IServiceProvider.GetService(Type serviceType) => serviceType == typeof(ITypeDescriptorContext) || serviceType == typeof(IWindowsFormsEditorService)
				? this
				: (designer.Component?.Site?.GetService(serviceType));

		void ITypeDescriptorContext.OnComponentChanged() => ChangeService.OnComponentChanged(designer.Component, targetProperty, null, null);

		bool ITypeDescriptorContext.OnComponentChanging()
		{
			try
			{
				ChangeService.OnComponentChanging(designer.Component, targetProperty);
			}
			catch (CheckoutException exception)
			{
				if (exception != CheckoutException.Canceled)
					throw;
				return false;
			}
			return true;
		}

		private T GetService<T>() => (T)((IServiceProvider)this).GetService(typeof(T));

		private void OnEditItems(object sender, EventArgs e)
		{
			var component = targetProperty.GetValue(designer.Component);
			if (component != null)
			{
				var editor = (UITypeEditor)TypeDescriptor.GetEditor(component, typeof(UITypeEditor));
				editor?.EditValue(this, this, component);
			}
		}
	}

	internal abstract class RichBehavior<TDesigner> :
#if NETFRAMEWORK
		System.Windows.Forms.Design.Behavior.Behavior
#else
		Behavior
#endif
		where TDesigner : ControlDesigner
	{
		protected RichBehavior(TDesigner designer) => Designer = designer;

		public TDesigner Designer { get; }
	}

	internal class RichComponentDesigner<TComponent, TActions> : ComponentDesigner
		where TComponent : Component
		where TActions : BaseDesignerActionList
	{
		private TActions actions;
		private Adorner adorner;
		private IDictionary<string, List<Attribute>> redirectedProps;
		private DesignerVerbCollection verbs;

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				actions ??= Activator.CreateInstance(typeof(TActions), this, Component) as TActions;
				return new DesignerActionListCollection { actions };
			}
		}

		public BehaviorService BehaviorService { get; private set; }

#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
		public new IComponentChangeService ComponentChangeService { get; private set; }

		public new ISelectionService SelectionService { get; private set; }
#pragma warning restore CS0109 // Member does not hide an inherited member; new keyword is not required

		public override DesignerVerbCollection Verbs => verbs ??= this.GetAttributedVerbs();
		public new TComponent Component => (TComponent)base.Component;

		public virtual GlyphCollection Glyphs => Adorner.Glyphs;

		internal Adorner Adorner
		{
			get
			{
				if (adorner == null)
				{
					adorner = new Adorner();
					BehaviorService.Adorners.Add(adorner);
				}
				return adorner;
			}
		}

		protected virtual IEnumerable<string> PropertiesToRemove => new string[0];

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			BehaviorService = GetService<BehaviorService>();
			SelectionService = GetService<ISelectionService>();
			if (SelectionService != null)
				SelectionService.SelectionChanged += OnSelectionChanged;
			ComponentChangeService = GetService<IComponentChangeService>();
			if (ComponentChangeService != null)
				ComponentChangeService.ComponentChanged += OnComponentChanged;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (BehaviorService != null & adorner != null)
					BehaviorService.Adorners.Remove(adorner);
				var ss = SelectionService;
				if (ss != null)
					ss.SelectionChanged -= OnSelectionChanged;
				var cs = ComponentChangeService;
				if (cs != null)
					cs.ComponentChanged -= OnComponentChanged;
			}
			base.Dispose(disposing);
		}

#if NETFRAMEWORK
		protected virtual TSvc GetService<TSvc>() where TSvc : class => (TSvc)GetService(typeof(TSvc));
#endif

		protected virtual void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
		}

		protected virtual void OnSelectionChanged(object sender, EventArgs e)
		{
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);

			// RedirectRegisteredProperties
			redirectedProps ??= this.GetRedirectedProperties();
			this.RedirectRegisteredProperties(properties, redirectedProps);

			// Remove properties
			this.RemoveProperties(properties, PropertiesToRemove);
		}
	}

	internal class RichControlDesigner<TControl, TActions> : ControlDesigner
		where TControl : Control
		where TActions : BaseDesignerActionList
	{
		private TActions actions;
		private Adorner adorner;
		private IDictionary<string, List<Attribute>> redirectedProps;
		private DesignerVerbCollection verbs;

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				actions ??= Activator.CreateInstance(typeof(TActions), this, Component) as TActions;
				return new DesignerActionListCollection{ actions };
			}
		}

#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
		public new IComponentChangeService ComponentChangeService { get; private set; }
		public new ISelectionService SelectionService { get; private set; }
#pragma warning restore CS0109 // Member does not hide an inherited member; new keyword is not required
		public override DesignerVerbCollection Verbs => verbs ??= this.GetAttributedVerbs();
		public new BehaviorService BehaviorService => base.BehaviorService;
		public new TControl Control => (TControl)base.Control;

		public GlyphCollection Glyphs => Adorner.Glyphs;

		internal Adorner Adorner
		{
			get
			{
				if (adorner == null)
				{
					adorner = new Adorner();
					BehaviorService.Adorners.Add(adorner);
				}
				return adorner;
			}
		}

		protected virtual IEnumerable<string> PropertiesToRemove => new string[0];

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			SelectionService = GetService<ISelectionService>();
			if (SelectionService != null)
				SelectionService.SelectionChanged += OnSelectionChanged;
			ComponentChangeService = GetService<IComponentChangeService>();
			if (ComponentChangeService != null)
				ComponentChangeService.ComponentChanged += OnComponentChanged;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (BehaviorService != null && adorner != null)
					BehaviorService.Adorners.Remove(adorner);
				var ss = SelectionService;
				if (ss != null)
					ss.SelectionChanged -= OnSelectionChanged;
				var cs = ComponentChangeService;
				if (cs != null)
					cs.ComponentChanged -= OnComponentChanged;
			}
			base.Dispose(disposing);
		}

#if NETFRAMEWORK
		protected virtual TSvc GetService<TSvc>() where TSvc : class => (TSvc)GetService(typeof(TSvc));
#endif

		protected virtual void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
		}

		protected virtual void OnSelectionChanged(object sender, EventArgs e)
		{
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);

			// RedirectRegisteredProperties
			redirectedProps ??= this.GetRedirectedProperties();
			this.RedirectRegisteredProperties(properties, redirectedProps);

			// Remove properties
			this.RemoveProperties(properties, PropertiesToRemove);
		}
	}

	internal abstract class RichDesignerActionList<TDesigner, TComponent> : BaseDesignerActionList where TDesigner : ComponentDesigner where TComponent : Component
	{
		protected RichDesignerActionList(TDesigner designer, TComponent component) : base(designer, component) => ParentDesigner = designer;

		public new TDesigner ParentDesigner { get; }
		public new TComponent Component => (TComponent)base.Component;
	}

	internal abstract class RichGlyph<TDesigner> : Glyph, IDisposable where TDesigner : ControlDesigner
	{
		protected RichGlyph(TDesigner designer,
#if NETFRAMEWORK
		System.Windows.Forms.Design.Behavior.Behavior
#else
		Behavior
#endif
			behavior)
			: base(behavior) => Designer = designer;

		public TDesigner Designer { get; }

		public virtual void Dispose()
		{
		}

		public void SetBehavior(RichBehavior<TDesigner> b) => base.SetBehavior(b);
	}

	internal class RichParentControlDesigner<TControl, TActions> : ParentControlDesigner
		where TControl : Control
		where TActions : BaseDesignerActionList
	{
		private TActions actions;
		private Adorner adorner;
		private IDictionary<string, List<Attribute>> redirectedProps;
		private DesignerVerbCollection verbs;

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				actions ??= Activator.CreateInstance(typeof(TActions), this, Component) as TActions;
				return new DesignerActionListCollection { actions };
			}
		}

#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
		public new IComponentChangeService ComponentChangeService { get; private set; }
		public new ISelectionService SelectionService { get; private set; }
#pragma warning restore CS0109 // Member does not hide an inherited member; new keyword is not required
		public override DesignerVerbCollection Verbs => verbs ??= this.GetAttributedVerbs();
		public new BehaviorService BehaviorService => base.BehaviorService;
		public new TControl Control => (TControl)base.Control;

		public virtual GlyphCollection Glyphs => Adorner.Glyphs;

		internal Adorner Adorner
		{
			get
			{
				if (adorner != null) return adorner;
				adorner = new Adorner();
				BehaviorService.Adorners.Add(adorner);
				return adorner;
			}
		}

		protected virtual IEnumerable<string> PropertiesToRemove => new string[0];

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			SelectionService = GetService<ISelectionService>();
			if (SelectionService != null)
				SelectionService.SelectionChanged += OnSelectionChanged;
			ComponentChangeService = GetService<IComponentChangeService>();
			if (ComponentChangeService != null)
				ComponentChangeService.ComponentChanged += OnComponentChanged;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (BehaviorService != null & adorner != null)
					BehaviorService.Adorners.Remove(adorner);
				var ss = SelectionService;
				if (ss != null)
					ss.SelectionChanged -= OnSelectionChanged;
				var cs = ComponentChangeService;
				if (cs != null)
					cs.ComponentChanged -= OnComponentChanged;
			}
			base.Dispose(disposing);
		}

#if NETFRAMEWORK
		protected virtual TSvc GetService<TSvc>() where TSvc : class => (TSvc)GetService(typeof(TSvc));
#endif

		protected virtual void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
		}

		protected virtual void OnSelectionChanged(object sender, EventArgs e)
		{
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);

			// RedirectRegisteredProperties
			redirectedProps ??= this.GetRedirectedProperties();
			this.RedirectRegisteredProperties(properties, redirectedProps);

			// Remove properties
			this.RemoveProperties(properties, PropertiesToRemove);
		}
	}
}