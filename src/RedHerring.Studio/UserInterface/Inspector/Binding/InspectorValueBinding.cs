using System.Reflection;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.UserInterface;

public class InspectorValueBinding : InspectorBinding
{
	protected readonly object     _source;
	protected readonly FieldInfo? _sourceField;
	protected readonly Action?    _onCommitValue;

	public override bool       IsUnbound       => _sourceField == null;
	public override bool       IsReadOnly      => _sourceField == null || _sourceField.IsInitOnly || _sourceField.GetCustomAttribute<ReadOnlyInInspectorAttribute>() != null;
	public override object     Source          => _source;
	public override FieldInfo? SourceFieldInfo => _sourceField!;
	public override int        Index           => -1;
	
	public InspectorValueBinding(object source, FieldInfo? sourceField, Action? onCommitValue)
	{
		_source        = source;
		_sourceField   = sourceField;
		_onCommitValue = onCommitValue;
	}

	public override object? GetValue()
	{
		return _sourceField?.GetValue(_source);
	}

	public override void SetValue(object? value)
	{
		_sourceField?.SetValue(_source, value);
		_onCommitValue?.Invoke();
	}

	public Type? GetElementType()
	{
		Type? sourceFieldType = SourceFieldInfo?.FieldType;
		if (sourceFieldType == null)
		{
			return null;
		}

		if (sourceFieldType.IsArray)
		{
			return sourceFieldType.GetElementType()!;
		}
		
		if (sourceFieldType.IsGenericType && sourceFieldType.GetGenericTypeDefinition() == typeof(List<>))
		{
			return sourceFieldType.GetGenericArguments()[0];
		}

		return null;
	}
}
