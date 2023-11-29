using System.Collections;
using RedHerring.Studio.Commands;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorCreateListElementCommand : Command
{
	private readonly List<InspectorBinding> _bindings;
	
	public InspectorCreateListElementCommand(List<InspectorBinding> bindings)
	{
		_bindings = new List<InspectorBinding>();
		_bindings.AddRange(bindings);
	}

	public override void Do()
	{
		foreach(InspectorBinding binding in _bindings)
		{
			if (binding.IsUnbound)
			{
				continue;
			}

			{
				IList? list        = binding.GetValue() as IList;
				Type?  elementType = (binding as InspectorValueBinding)?.GetElementType();
				if (list == null || elementType == null)
				{
					continue;
				}

				if (elementType.IsClass)
				{
					// class
					list.Add(null);
				}
				else
				{
					// value object
					list.Add(Activator.CreateInstance(elementType));
				}
			}
		}
	}

	public override void Undo()
	{
		foreach(InspectorBinding binding in _bindings)
		{
			if (binding.IsUnbound)
			{
				continue;
			}

			if(binding.GetValue() is IList list)
			{
				list.RemoveAt(list.Count - 1);
			}
		}
	}
}