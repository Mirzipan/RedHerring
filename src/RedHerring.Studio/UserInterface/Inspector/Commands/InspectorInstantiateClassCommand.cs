using RedHerring.Studio.Commands;

namespace RedHerring.Studio.UserInterface;

// TODO - refactor, too many similar parts
public sealed class InspectorInstantiateClassCommand : ACommand
{
	private readonly List<InspectorBinding> _bindings;
	private readonly Type                   _type;
	private readonly object?[]              _previousValues;
	
	public InspectorInstantiateClassCommand(List<InspectorBinding> bindings, Type type)
	{
		_bindings = new List<InspectorBinding>();
		_bindings.AddRange(bindings);

		_type           = type;

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

			object? instance = Activator.CreateInstance(_type);
			binding.SetValue(instance);
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