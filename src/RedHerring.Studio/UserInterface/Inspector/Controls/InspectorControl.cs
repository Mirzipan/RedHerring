using System.Reflection;
using RedHerring.Alexandria.Extensions;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.UserInterface;

public abstract class InspectorControl
{
	protected static readonly object UnboundValue   = new(); // mark that there is no value to update
	protected static readonly object MultipleValues = new(); // mark that there are multiple values

	protected readonly IInspectorCommandTarget _commandTarget;
	public readonly    string                  Id;
	public             string?                 Label    = null;
	public             string                  LabelId  = null!;
	public readonly    List<InspectorBinding>  Bindings = new();

	protected InspectorControl(IInspectorCommandTarget commandTarget, string id)
	{
		_commandTarget = commandTarget;
		Id         = id;
		LabelId    = $"##{id}";
	}

	public Type? BoundValueType => Bindings.Count == 0 ? null : Bindings[0].SourceFieldInfo?.FieldType; // todo - remove/move to binding?

	public virtual void InitFromSource(object? sourceOwner, object source, FieldInfo? sourceField = null, int sourceIndex = -1)
	{
		if (sourceField != null)
		{
			Label   = sourceField.Name.PrettyCamelCase();
			LabelId = $"{Label}##{Id}";
		}

		Bindings.Add(InspectorBinding.Create(sourceOwner, source, sourceField, sourceIndex, GetOnCommitValueAction(sourceOwner, sourceField)));
	}

	public virtual void AdaptToSource(object? sourceOwner, object source, FieldInfo? sourceField = null)
	{
		Bindings.Add(InspectorBinding.Create(sourceOwner, source, sourceField, -1, GetOnCommitValueAction(sourceOwner, sourceField)));
	}
	
	public abstract void Update();

	public void SetCustomLabel(string? label)
	{
		Label   = label;
		LabelId = label == null ? $"##{Id}" : $"{label}##{Id}";
	}

	private Action? GetOnCommitValueAction(object? sourceOwner, FieldInfo? sourceField) // TODO - move to binding?
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
		if (Bindings.Count == 0 || Bindings[0].IsUnbound)
		{
			return UnboundValue;
		}

		object? value = Bindings[0].GetValue();
		for (int i = 1; i < Bindings.Count; ++i)
		{
			if (!Bindings[i].IsUnbound)
			{
				return UnboundValue;
			}
 
			object? otherValue = Bindings[i].GetValue();
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
		object? currentValue = GetValue();
		if (currentValue == null)
		{
			if (value == null)
			{
				return;
			}
		}
		else if (currentValue.Equals(value))
		{
			return;
		}

		Console.WriteLine($"Submitted value {value} from control {Id}");
		_commandTarget.Commit(new InspectorModifyValueCommand(Bindings, value));
	}
	#endregion
}