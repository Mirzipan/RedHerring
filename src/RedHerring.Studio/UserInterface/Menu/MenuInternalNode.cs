using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class MenuInternalNode : MenuNode
{
	protected readonly List<MenuNode> _children = new();

	public MenuInternalNode(string name) : base(name)
	{
	}
	
	public override void Update()
	{
		if (Gui.BeginMenu(Name))
		{
			foreach (MenuNode child in _children)
			{
				child.Update();
			}

			Gui.EndMenu();
		}
	}
	
	public MenuInternalNode GetOrCreateInternalNode(string name)
	{
		if (_children.FirstOrDefault(child => child.Name == name) is MenuInternalNode node)
		{
			return node;
		}

		node = new MenuInternalNode(name);
		_children.Add(node);
		return node;
	}

	public void AddLeafNode(MenuLeafNode node)
	{
		_children.Add(node);
	}
}