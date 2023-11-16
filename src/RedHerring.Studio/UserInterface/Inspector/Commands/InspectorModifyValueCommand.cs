using System.Collections;
using RedHerring.Studio.Commands;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorModifyValueCommand : ACommand
{
	private readonly object?                     _value;
	private readonly List<InspectorValueBinding> _valueBindings;
	private readonly object?[]                   _previousValues;
	
	public InspectorModifyValueCommand(object? value, List<InspectorValueBinding> valueBindings)
	{
		_value = value;
		
		_valueBindings = new List<InspectorValueBinding>();
		_valueBindings.AddRange(valueBindings); // hmm, it is safe to just keep reference to the list?
		
		_previousValues = new object[_valueBindings.Count];
		for(int i=0;i <_valueBindings.Count; ++i)
		{
			if (_valueBindings[i].SourceField == null)
			{
				continue;
			}

			object? previousValue = _valueBindings[i].SourceField?.GetValue(_valueBindings[i].Source);
			if (_valueBindings[i].IsBoundToList)
			{
				IList? list = previousValue as IList;
				previousValue = list?[_valueBindings[i].Index];
			}

			_previousValues[i] = previousValue;
		}
	}
	
	public override void Do()
	{
		foreach (InspectorValueBinding binding in _valueBindings)
		{
			if (binding.SourceField == null)
			{
				continue;
			}

			if (binding.IsBoundToList)
			{
				if (binding.SourceField?.GetValue(binding.Source) is IList list)
				{
					list[binding.Index] = _value;
				}
			}
			else
			{
				binding.SourceField.SetValue(binding.Source, _value);
			}
		}
	}

	public override void Undo()
	{
		for(int i=0;i <_valueBindings.Count; ++i)
		{
			if (_valueBindings[i].SourceField == null)
			{
				continue;
			}

			if (_valueBindings[i].IsBoundToList)
			{
				if (_valueBindings[i].SourceField?.GetValue(_valueBindings[i].Source) is IList list)
				{
					list[_valueBindings[i].Index] = _previousValues[i];
				}
			}
			else
			{
				_valueBindings[i].SourceField?.SetValue(_valueBindings[i].Source, _previousValues[i]);
			}
		}
	}
}