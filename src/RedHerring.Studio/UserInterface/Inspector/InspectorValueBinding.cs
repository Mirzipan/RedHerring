using System.Reflection;

namespace RedHerring.Studio.UserInterface;

public readonly struct InspectorValueBinding
{
	public readonly object Source;
	public readonly FieldInfo? SourceField;

	public InspectorValueBinding(object source, FieldInfo? sourceField = null)
	{
		Source      = source;
		SourceField = sourceField;
	}
}