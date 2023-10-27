namespace RedHerring.Studio.Models.ViewModels;

public sealed class SelectionViewModel
{
	private HashSet<string> _selection = new();

	public void Select(string item)
	{
		_selection.Add(item);
	}
	
	public void Deselect(string item)
	{
		_selection.Remove(item);
	}

	public void Flip(string item)
	{
		if (_selection.Contains(item))
		{
			Deselect(item);
		}
		else
		{
			Select(item);
		}
	}

	public void DeselectAll()
	{
		_selection.Clear();
	}

	public bool IsSelected(string item)
	{
		return _selection.Contains(item);
	}
}