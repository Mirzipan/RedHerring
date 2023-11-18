using System.Reflection;

namespace RedHerring.Studio.UserInterface;

public abstract class InspectorBinding
{
	public abstract bool       IsUnbound       { get; }
	public abstract bool       IsReadOnly      { get; }
	public abstract object     Source          { get; }
	public abstract FieldInfo? SourceFieldInfo { get; }
	public abstract int        Index           { get; }

	public abstract object? GetValue();
	public abstract void SetValue(object? value);

	public static InspectorBinding Create(object source, FieldInfo? sourceField, int sourceIndex, Action? onCommitValue)
	{
		if (sourceIndex != -1)
		{
			return new InspectorListValueBinding(source, sourceField, onCommitValue, sourceIndex);
		}

		return new InspectorValueBinding(source, sourceField, onCommitValue);
	}
}