using RedHerring.Studio.Commands;

namespace RedHerring.Studio.UserInterface;

// TODO - refactor, copy pasted
public class InspectorDeleteClassCommand : Command
{
	private readonly List<InspectorBinding> _bindings;
	private readonly object?[]              _previousValues;
	
	public InspectorDeleteClassCommand(List<InspectorBinding> bindings)
	{
		_bindings = new List<InspectorBinding>();
		_bindings.AddRange(bindings);

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

			binding.SetValue(null);
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