using System.Reflection;

namespace RedHerring.Studio.UserInterface;

public readonly struct InspectorValueBinding
{
	public readonly object     Source;
	public readonly FieldInfo? SourceField;
	public readonly Action?    OnCommitValue;

	public InspectorValueBinding(object source, FieldInfo? sourceField, Action? onCommitValue)
	{
		Source        = source;
		SourceField   = sourceField;
		OnCommitValue = onCommitValue;
	}
}