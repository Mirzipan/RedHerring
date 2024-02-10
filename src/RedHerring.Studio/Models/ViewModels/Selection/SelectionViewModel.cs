namespace RedHerring.Studio.Models.ViewModels;

public sealed class SelectionViewModel
{
	private readonly Dictionary<string, WeakReference<ISelectable>> _selection = new();
	public event Action?                                            SelectionChanged;

	public void Select(string path, ISelectable target)
	{
		_selection[path] = new WeakReference<ISelectable>(target);
		SelectionChanged?.Invoke();
	}
	
	public void Deselect(string path)
	{
		_selection.Remove(path);
		SelectionChanged?.Invoke();
	}

	public void Flip(string path, ISelectable target)
	{
		if (_selection.ContainsKey(path))
		{
			Deselect(path);
		}
		else
		{
			Select(path, target);
		}
	}

	public void DeselectAll()
	{
		_selection.Clear();
		SelectionChanged?.Invoke();
	}

	public bool IsSelected(string item)
	{
		return _selection.ContainsKey(item);
	}

	public ISelectable? SelectedTarget(string item)
	{
		return _selection.TryGetValue(item, out WeakReference<ISelectable>? targetRef) ? (targetRef.TryGetTarget(out ISelectable? target) ? target : null) : null;
	}
	
	public IReadOnlyList<ISelectable> GetAllSelectedTargets()
	{
		return _selection.Values.Select(reference => reference.TryGetTarget(out ISelectable? target) ? target : null).Where(target => target != null).ToList()!;
	}
}