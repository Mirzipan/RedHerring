using System.Collections;
using RedHerring.Studio.Commands;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorDeleteListElementCommand : Command
{
	private readonly List<InspectorBinding> _bindings;
	private readonly int                    _index;

	private readonly object?[] _previousValues;
	
	public InspectorDeleteListElementCommand(List<InspectorBinding> bindings, int index)
	{
		_bindings = new List<InspectorBinding>();
		_bindings.AddRange(bindings);

		_index = index;
		
		_previousValues = new object[_bindings.Count];
		for(int i=0;i <_bindings.Count; ++i)
		{
			if (_bindings[i].IsUnbound)
			{
				continue;
			}

			if(_bindings[i].GetValue() is IList list)
			{
				_previousValues[i] = list[_index];
			}
		}
	}
	
	public override void Do()
	{
		foreach(InspectorBinding binding in _bindings)
		{
			if (binding.IsUnbound)
			{
				continue;
			}

			if(binding.GetValue() is IList list)
			{
				list.RemoveAt(_index);
			}
		}
	}

	public override void Undo()
	{
		for(int i=0;i<_bindings.Count;++i)
		{
			if (_bindings[i].IsUnbound)
			{
				continue;
			}

			if(_bindings[i].GetValue() is IList list)
			{
				list.Insert(_index, _previousValues[i]);
			}
		}
	}
}