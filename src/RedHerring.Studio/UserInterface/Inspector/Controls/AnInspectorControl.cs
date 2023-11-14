using System.Reflection;
using RedHerring.Alexandria.Extensions;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.UserInterface;

public abstract class AnInspectorControl
{
	#region Control map
	private static Dictionary<Type, Type> _fieldToControlMap // TODO - move to attribute attached to each control class
		= new()
		  {
			  {typeof(int), typeof(InspectorIntControl)},
			  {typeof(float), typeof(InspectorFloatControl)},
			  {typeof(string), typeof(InspectorStringControl)},
			  {typeof(bool), typeof(InspectorBoolControl)},
		  };

	private static readonly Type _classControl            = typeof(InspectorClassControl);
	private static readonly Type _enumControl             = typeof(InspectorEnumControl);
	private static readonly Type _valueDropdownIntControl = typeof(InspectorValueDropdownIntControl);
	private static readonly Type _valueDropdownStringControl = typeof(InspectorValueDropdownStringControl);
	#endregion

	public static readonly object UnboundValue   = new();
	public static readonly object MultipleValues = new();

	protected readonly Inspector                   _inspector;
	public readonly    string                      Id;
	public             string?                     Label         = null;
	public             string                      LabelId       = null!;
	public readonly    List<InspectorValueBinding> ValueBindings = new();

	protected AnInspectorControl(Inspector inspector, string id)
	{
		_inspector = inspector;
		Id         = id;
		LabelId    = id;
	}

	public virtual void InitFromSource(object? sourceOwner, object source, FieldInfo? sourceField = null)
	{
		if (sourceField != null)
		{
			Label   = sourceField.Name.PrettyCamelCase();
			LabelId = Label + Id;
		}

		ValueBindings.Add(new InspectorValueBinding(source, sourceField, GetOnCommitValue(sourceOwner, sourceField)));
	}

	public virtual void AdaptToSource(object? sourceOwner, object source, FieldInfo? sourceField = null)
	{
		ValueBindings.Add(new InspectorValueBinding(source, sourceField, GetOnCommitValue(sourceOwner, sourceField)));
	}
	
	public abstract void Update();

	public void SetCustomLabel(string? label)
	{
		Label   = label;
		LabelId = label == null ? Id : label + Id;
	}

	protected Type? FieldToControl(FieldInfo fieldInfo)
	{
		Type fieldType = fieldInfo.FieldType;
		
		// by attribute
		if(fieldInfo.GetCustomAttribute<ValueDropdownAttribute>() != null)
		{
			if (fieldType == typeof(int))
			{
				return _valueDropdownIntControl;
			}

			if (fieldType == typeof(string))
			{
				return _valueDropdownStringControl;
			}

			return null;
		}
		
		// by type
		if (_fieldToControlMap.TryGetValue(fieldType, out Type? type))
		{
			return type;
		}

		if (fieldType.IsEnum)
		{
			return _enumControl;
		}

		if (fieldType.IsClass)
		{
			return _classControl;
		}

		return null;
	}

	private Action? GetOnCommitValue(object? sourceOwner, FieldInfo? sourceField)
	{
		if (sourceOwner == null || sourceField == null)
		{
			return null;
		}

		OnCommitValueAttribute? onCommitValueAttribute = sourceField.GetCustomAttribute<OnCommitValueAttribute>();
		if (onCommitValueAttribute == null)
		{
			return null;
		}

		MethodInfo? onCommitMethod = sourceOwner.GetType().GetMethod(onCommitValueAttribute.MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		if (onCommitMethod == null)
		{
			return null;
		}

		return () => onCommitMethod.Invoke(sourceOwner, null);
	}

	#region Value manipulation
	protected object? GetValue()
	{
		if (ValueBindings.Count == 0 || ValueBindings[0].SourceField == null)
		{
			return UnboundValue;
		}

		object? value = ValueBindings[0].SourceField.GetValue(ValueBindings[0].Source);
		for (int i = 1; i < ValueBindings.Count; ++i)
		{
			if (ValueBindings[1].SourceField == null)
			{
				return UnboundValue;
			}
 
			object? otherValue = ValueBindings[1].SourceField.GetValue(ValueBindings[1].Source);
			if (otherValue == null)
			{
				if (value == null)
				{
					continue;
				}
				return MultipleValues;
			}

			if (!otherValue.Equals(value))
			{
				return MultipleValues;
			}
		}

		return value;
	}

	protected void SetValue(object? value)
	{
		Console.WriteLine($"Submitted value {value}");
		_inspector.Commit(new InspectorModifyValueCommand(value, ValueBindings));

		foreach (InspectorValueBinding valueBinding in ValueBindings)
		{
			valueBinding.OnCommitValue?.Invoke();
		}
	}
	#endregion
}