using RedHerring.Studio.Models;
using RedHerring.Studio.Models.ViewModels.Console;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.Tools;

[Tool(ToolName)]
public sealed class ToolConsole : ATool
{
	public const       string ToolName = "Console";
	protected override string Name => ToolName;

	public ToolConsole(StudioModel studioModel) : base(studioModel)
	{
	}

	public ToolConsole(StudioModel studioModel, int uniqueId) : base(studioModel, uniqueId)
	{
	}

	public override void Update(out bool finished)
	{
		finished = UpdateUI();
	}
	
	private bool UpdateUI()
	{
		bool isOpen = true;
		if (Gui.Begin(NameWithSalt, ref isOpen))
		{
			for(int i=0; i<StudioModel.Console.Count; ++i)
			{
				ConsoleItem item = StudioModel.Console[i];
				Gui.TextColored(item.Type.ToColor(), item.Message);
			}
			
			Gui.End();
		}

		return !isOpen;
	}
}