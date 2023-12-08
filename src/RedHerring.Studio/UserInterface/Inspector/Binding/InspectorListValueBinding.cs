using System.Collections;
using System.Reflection;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.UserInterface;

// list item binding
public class InspectorListValueBinding : InspectorValueBinding
{
	private readonly int  _index;
	public override  int  Index                => _index;
	
	public override Type? BoundType => GetElementType() ?? base.BoundType;
	
	public InspectorListValueBinding(object? sourceOwner, object source, FieldInfo? sourceField, Action? onCommitValue, int index)
		: base(sourceOwner, source, sourceField, onCommitValue)
	{
		if (index == -1)
		{
			throw new ArgumentException("Index cannot be -1 for list value binding");
		}

		_index                = index;
		_allowDeleteReference = !_isReadOnly || (_sourceField != null && _sourceField.GetCustomAttribute<AllowDeleteReferenceAttribute>() != null);
	}

	public override object? GetValue()
	{
		IList? list = _sourceField?.GetValue(Source) as IList;
		return list?[_index];
	}

	public override void SetValue(object? value)
	{
		if (_sourceField?.GetValue(Source) is IList list)
		{
			list[_index] = value;
			_onCommitValue?.Invoke();
		}
	}
}