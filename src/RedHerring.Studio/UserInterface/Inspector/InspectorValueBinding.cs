using System.Reflection;

namespace RedHerring.Studio.UserInterface;

public readonly struct InspectorValueBinding
{
	public readonly object     Source;
	public readonly FieldInfo? SourceField;
	public readonly int        Index;	// if source field is an array, this is the index of the element
	public readonly Action?    OnCommitValue;

	public bool IsBoundToList => Index != -1;

	public InspectorValueBinding(object source, FieldInfo? sourceField, Action? onCommitValue)
	{
		Source        = source;
		SourceField   = sourceField;
		Index         = -1;
		OnCommitValue = onCommitValue;
	}
	
	public InspectorValueBinding(object source, FieldInfo? sourceField, int index, Action? onCommitValue)
	{
		Source        = source;
		SourceField   = sourceField;
		Index         = index;
		OnCommitValue = onCommitValue;
	}
}