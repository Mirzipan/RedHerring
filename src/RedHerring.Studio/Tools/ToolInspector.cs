using RedHerring.Studio.Models;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.Tools;

public sealed class ToolInspector : ATool
{
	public ToolInspector(StudioModel studioModel) : base(studioModel)
	{
	}

	public override void Update(out bool finished)
	{
		finished = UpdateUI();
	}

	private bool UpdateUI()
	{
		bool isOpen = true;
		if (Gui.Begin($"Inspector##{UniqueId}", ref isOpen))
		{
			Gui.End();
		}

		return !isOpen;
	}
}