using System.Collections;
using RedHerring.Studio.Commands;

namespace RedHerring.Studio.UserInterface;

public class InspectorSwapListElementsCommand : Command
{
	private readonly List<InspectorBinding> _bindings;
	private readonly int _indexA;
	private readonly int _indexB;
	
	public InspectorSwapListElementsCommand(List<InspectorBinding> bindings, int indexA, int indexB)
	{
		_bindings = new List<InspectorBinding>();
		_bindings.AddRange(bindings);

		_indexA = indexA;
		_indexB = indexB;
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
				
				if(_indexA < 0 || _indexA >= list.Count || _indexB < 0 || _indexB >= list.Count)
				{
					continue;
				}

				(list[_indexA], list[_indexB]) = (list[_indexB], list[_indexA]);
			}
		}
	}

	public override void Undo()
	{
		Do(); // Do and Undo do the same thing 
	}
}