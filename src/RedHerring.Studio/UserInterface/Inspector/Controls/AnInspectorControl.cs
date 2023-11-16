using System.Collections;
using System.Reflection;
using RedHerring.Alexandria.Extensions;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.UserInterface;

public abstract class AnInspectorControl
{
	protected static readonly object UnboundValue   = new(); // mark that there is no value to update
	protected static readonly object MultipleValues = new(); // mark that there are multiple values

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

	public Type? ValueType => ValueBindings.Count == 0 ? null : ValueBindings[0].SourceField?.FieldType ?? typeof(object);

	public virtual void InitFromSource(object? sourceOwner, object source, FieldInfo? sourceField = null, int sourceIndex = -1)
	{
		if (sourceField != null)
		{
			Label   = sourceField.Name.PrettyCamelCase();
			LabelId = Label + Id;
		}

		ValueBindings.Add(new InspectorValueBinding(source, sourceField, sourceIndex, GetOnCommitValue(sourceOwner, sourceField)));
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

		object? value = ValueBindings[0].SourceField!.GetValue(ValueBindings[0].Source);
		if (ValueBindings[0].IsBoundToList)
		{
			IList? list = value as IList;
			value = list?[ValueBindings[0].Index];
		}
		
		for (int i = 1; i < ValueBindings.Count; ++i)
		{
			if (ValueBindings[i].SourceField == null)
			{
				return UnboundValue;
			}
 
			object? otherValue = ValueBindings[i].SourceField!.GetValue(ValueBindings[i].Source);
			if (ValueBindings[i].IsBoundToList)
			{
				IList? list = otherValue as IList;
				otherValue = list?[ValueBindings[i].Index];
			}

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