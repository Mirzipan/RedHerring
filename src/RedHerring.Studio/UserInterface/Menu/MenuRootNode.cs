using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class MenuRootNode : MenuInternalNode
{
	public MenuRootNode() : base("")
	{
	}
	
	public override void Update()
	{
		if (Gui.BeginMainMenuBar())
		{
			foreach (AMenuNode child in _children)
			{
				child.Update();
			}

			Gui.EndMainMenuBar();
		}
	}
}