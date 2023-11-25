﻿using System.Reflection;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.UserInterface;

// any value/class binding
public class InspectorValueBinding : InspectorBinding
{
	protected readonly object?    _sourceOwner;
	protected readonly FieldInfo? _sourceField;
	protected readonly Action?    _onCommitValue;

	public override bool       IsUnbound       => _sourceField == null;
	public override bool       IsReadOnly      => _sourceField == null || _sourceField.IsInitOnly || _sourceField.GetCustomAttribute<ReadOnlyInInspectorAttribute>() != null;
	public override object?    SourceOwner     => _sourceOwner;
	public override FieldInfo? SourceFieldInfo => _sourceField!;
	public override Type?      BoundType       => _sourceField?.FieldType ?? Source.GetType();
	
	public InspectorValueBinding(object? sourceOwner, object source, FieldInfo? sourceField, Action? onCommitValue)
		: base(source)
	{
		_sourceOwner   = sourceOwner;
		_sourceField   = sourceField;
		_onCommitValue = onCommitValue;
	}

	public override object? GetValue()
	{
		return _sourceField?.GetValue(Source);
	}

	public override void SetValue(object? value)
	{
		_sourceField?.SetValue(Source, value);
		_onCommitValue?.Invoke();
	}
}
