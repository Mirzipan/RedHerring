using System.Numerics;
using ImGuiNET;
using RedHerring.Infusion.Attributes;
using RedHerring.Render.ImGui;
using RedHerring.Studio.Models;
using RedHerring.Studio.Systems;
using static ImGuiNET.ImGui;

namespace RedHerring.Studio.Tools;

[Tool(ToolName)]
public sealed class ToolDebug : Tool
{
	[Infuse] private StudioSystem _studio = null!;
	
	public const       string ToolName = FontAwesome6.Bug + " Debug";
	protected override string Name => ToolName;

	private ToolDebug_StudioCamera? _cameraDebug = null;
	
	public ToolDebug(StudioModel studioModel, int uniqueId) : base(studioModel, uniqueId)
	{
	}

	public override void Update(out bool finished)
	{
		finished = UpdateUI();
	}
	
	private bool UpdateUI()
	{
		bool isOpen = true;
		SetNextWindowSize(new Vector2(300, 200), ImGuiCond.FirstUseEver);
		if (Begin(NameId, ref isOpen, ImGuiWindowFlags.HorizontalScrollbar))
		{
			if (CollapsingHeader("Studio camera"))
			{
				// TODO
				//_cameraDebug ??= new ToolDebug_StudioCamera(_studio.Camera);
				//_cameraDebug.Draw();
			}

			End();
		}

		return !isOpen;
	}
}