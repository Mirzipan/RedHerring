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

			_previousValues[i] = _valueBindings[i].SourceField?.GetValue(_valueBindings[i].Source);
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

			binding.SourceField.SetValue(binding.Source, _value);
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

			_valueBindings[i].SourceField?.SetValue(_valueBindings[i].Source, _previousValues[i]);
		}
	}
}