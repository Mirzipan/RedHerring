using RedHerring.Studio.Commands;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorModifyValueCommand : ACommand
{
	private readonly object?                _value;
	private readonly List<InspectorBinding> _bindings;
	private readonly object?[]              _previousValues;
	
	public InspectorModifyValueCommand(List<InspectorBinding> bindings, object? value)
	{
		_value = value;
		
		_bindings = new List<InspectorBinding>();
		_bindings.AddRange(bindings); // hmm, it is safe to just keep reference to the list?
		
		_previousValues = new object[_bindings.Count];
		for(int i=0;i <_bindings.Count; ++i)
		{
			if (_bindings[i].IsUnbound)
			{
				continue;
			}

			object? previousValue = _bindings[i].GetValue();
			_previousValues[i] = previousValue;
		}
	}
	
	public override void Do()
	{
		foreach (InspectorBinding binding in _bindings)
		{
			if (binding.IsUnbound)
			{
				continue;
			}

			binding.SetValue(_value);
		}
	}

	public override void Undo()
	{
		for(int i=0;i <_bindings.Count; ++i)
		{
			if (_bindings[i].IsUnbound)
			{
				continue;
			}

			_bindings[i].SetValue(_previousValues[i]);
		}
	}
}