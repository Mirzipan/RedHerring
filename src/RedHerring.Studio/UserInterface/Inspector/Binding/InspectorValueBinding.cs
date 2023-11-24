using System.Reflection;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.UserInterface;

public class InspectorValueBinding : InspectorBinding
{
	protected readonly object?    _sourceOwner;
	protected readonly object     _source;
	protected readonly FieldInfo? _sourceField;
	protected readonly Action?    _onCommitValue;

	public override bool       IsUnbound       => _sourceField == null;
	public override bool       IsReadOnly      => _sourceField == null || _sourceField.IsInitOnly || _sourceField.GetCustomAttribute<ReadOnlyInInspectorAttribute>() != null;
	public override object?    SourceOwner     => _sourceOwner;
	public override object     Source          => _source;
	public override FieldInfo? SourceFieldInfo => _sourceField!;
	public override int        Index           => -1;
	public override Type?      BoundType       => _sourceField?.FieldType ?? _source.GetType();
	
	public InspectorValueBinding(object? sourceOwner, object source, FieldInfo? sourceField, Action? onCommitValue)
	{
		_sourceOwner   = sourceOwner;
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
}
