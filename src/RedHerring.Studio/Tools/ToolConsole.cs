using RedHerring.Studio.Models;
using RedHerring.Studio.Models.ViewModels.Console;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.Tools;

public sealed class ToolConsole : ATool
{
	public ToolConsole(StudioModel studioModel) : base(studioModel)
	{
	}

	public override void Update(out bool finished)
	{
		finished = UpdateUI();
	}
	
	private bool UpdateUI()
	{
		bool isOpen = true;
		if (Gui.Begin($"Console##{UniqueId}", ref isOpen))
		{
			foreach(ConsoleItem item in StudioModel.Console.Items)
			{
				Gui.TextColored(item.Type.ToColor(), item.Message);
			}
			
			Gui.End();
		}

		return !isOpen;
	}
}