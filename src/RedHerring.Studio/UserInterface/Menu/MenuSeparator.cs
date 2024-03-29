using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class MenuSeparator : MenuNode
{
	public MenuSeparator() : base("")
	{
	}

	public override void Update()
	{
		Gui.Separator();
	}
}