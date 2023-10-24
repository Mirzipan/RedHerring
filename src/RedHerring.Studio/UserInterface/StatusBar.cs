using System.Numerics;
using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class StatusBar
{
	public void Update()
	{
		ImGuiStylePtr style = Gui.GetStyle();
		Vector2 viewportSize = Gui.GetMainViewport().Size;
		
		Gui.SetNextWindowPos(new Vector2(0, viewportSize.Y - style.WindowMinSize.Y));
		Gui.SetNextWindowSize(style.WindowMinSize with {X = viewportSize.X});
		
		Gui.Begin("StatusBar",
			ImGuiWindowFlags.NoCollapse |
			ImGuiWindowFlags.NoDecoration |
			ImGuiWindowFlags.NoInputs |
			ImGuiWindowFlags.NoDocking |
			ImGuiWindowFlags.NoMove |
			ImGuiWindowFlags.NoNav |
			ImGuiWindowFlags.NoResize |
			ImGuiWindowFlags.NoScrollbar |
			ImGuiWindowFlags.NoTitleBar |
			ImGuiWindowFlags.NoBringToFrontOnFocus |
			ImGuiWindowFlags.NoFocusOnAppearing
		);
		{
			Gui.Text("This is status bar");
		}
		Gui.End();
	}
}