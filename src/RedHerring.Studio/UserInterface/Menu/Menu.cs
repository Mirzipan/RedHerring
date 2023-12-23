namespace RedHerring.Studio.UserInterface;

public sealed class Menu
{
	private readonly MenuNode _root;
	
	// on click cache - to avoid calling actions directly from menu items
	private readonly Dictionary<string, Action?> _onClickActions  = new();
	private          string?                     _menuItemClicked = null;

	public Menu(MenuStyle style)
	{
		_root = new MenuRootNode(style);
	}

	public void Update()
	{
		_menuItemClicked = null;
		_root.Update();
	}

	public void InvokeClickActions()
	{
		if (_menuItemClicked != null)
		{
			_onClickActions[_menuItemClicked]?.Invoke();
			_menuItemClicked = null;
		}
	}

	public void AddItem(
		string path,
		Action onClick,
		Func<bool>? isEnabled = null,
		Func<bool>? isChecked = null
	)
	{
		string[] pathItems = path.Split("/");

		MenuInternalNode node = (MenuInternalNode)_root;
		for (int i = 0; i < pathItems.Length - 1; ++i)
		{
			node = node.GetOrCreateInternalNode(pathItems[i])!;
		}

		_onClickActions.Add(path, onClick);
		
		node.AddLeafNode(
			new MenuLeafNode(pathItems[^1], "", () => _menuItemClicked = path , isEnabled, isChecked)
		);
	}
}