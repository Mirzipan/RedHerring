using System.Reflection;

namespace RedHerring.Studio.UserInterface;

public abstract class AnInspectorControl
{
	#region Control map
	private static Dictionary<Type, Type> _fieldToControlMap
		= new()
		  {
			  {typeof(int), typeof(InspectorIntControl)}
		  };

	private static readonly Type _classControl = typeof(InspectorFoldoutControl);
	#endregion

	public static readonly object UnboundValue   = new();
	public static readonly object MultipleValues = new();

	protected readonly Inspector                   _inspector;
	public readonly    string                      Id;
	public             string                      Label         = "";
	public readonly    List<InspectorValueBinding> ValueBindings = new();

	protected AnInspectorControl(Inspector inspector, string id)
	{
		_inspector = inspector;
		Id         = id;
	}

	public virtual void InitFromSource(object source, FieldInfo? sourceField = null)
	{
		if (sourceField != null)
		{
			Label = sourceField.Name;
		}

		ValueBindings.Add(new InspectorValueBinding(source, sourceField));
	}

	public virtual void AdaptToSource(object source, FieldInfo? sourceField = null)
	{
		ValueBindings.Add(new InspectorValueBinding(source, sourceField));
	}

	public abstract void Update();

	public void SetCustomLabel(string label)
	{
		Label = label;
	}

	protected Type? FieldToControl(Type fieldType)
	{
		if (_fieldToControlMap.TryGetValue(fieldType, out Type? type))
		{
			return type;
		}

		if (fieldType.IsClass)
		{
			return _classControl;
		}

		return null;
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

	protected void SetValue(object value)
	{
		// create do action
		Action @do = () =>
		{
			foreach (InspectorValueBinding binding in ValueBindings)
			{
				if (binding.SourceField == null)
				{
					continue;
				}

				binding.SourceField.SetValue(binding.Source, value);
			}
		};

		// store old values
		object[] oldValues = new object[ValueBindings.Count];
		for(int i=0;i<ValueBindings.Count; ++i)
		{
			if (ValueBindings[i].SourceField == null)
			{
				continue;
			}

			oldValues[i] = ValueBindings[i].SourceField.GetValue(ValueBindings[i].Source);
		}
		
		// create undo action
		Action undo = () =>
		{
			for(int i=0;i <ValueBindings.Count; ++i)
			{
				if (ValueBindings[i].SourceField == null)
				{
					continue;
				}

				ValueBindings[i].SourceField.SetValue(ValueBindings[i].Source, oldValues[i]);
			}
		};
		
		// create commands
		_inspector.Commit(@do, undo);
	}
	#endregion
}