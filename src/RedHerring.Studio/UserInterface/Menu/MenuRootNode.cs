using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class MenuRootNode : MenuInternalNode
{
	private readonly MenuStyle _style;
	
	public MenuRootNode(MenuStyle style) : base("")
	{
		_style = style;
	}
	
	public override void Update()
	{
		if (_style == MenuStyle.MainMenu)
		{
			if (Gui.BeginMainMenuBar())
			{
				foreach (MenuNode child in _children)
				{
					child.Update();
				}

				Gui.EndMainMenuBar();
			}

			return;
		}

		foreach (MenuNode child in _children)
		{
			child.Update();
		}
	}
}