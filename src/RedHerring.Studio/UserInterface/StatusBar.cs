using System.Numerics;
using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class StatusBar
{
	public enum Color
	{
		Info    = 0,
		Warning = 1,
		Error   = 2,
	}

	private readonly Vector4[] _messageColors =
	{
		new(0.0f, 0.8f, 0.0f, 1.0f), // Info
		new(1.0f, 0.5f, 0.0f, 1.0f), // Warning
		new(1.0f, 0.0f, 0.0f, 1.0f), // Error
	};

	public string Message      { get; set; } = "";
	public Color  MessageColor { get; set; } = Color.Info;

	public void Update()
	{
		ImGuiStylePtr style        = Gui.GetStyle();
		Vector2       viewportSize = Gui.GetMainViewport().Size;
		
		Gui.SetNextWindowPos(new Vector2(0, viewportSize.Y - style.WindowMinSize.Y));
		Gui.SetNextWindowSize(style.WindowMinSize with {X = viewportSize.X});
		
		Gui.Begin("StatusBar",
			ImGuiWindowFlags.NoCollapse            |
			ImGuiWindowFlags.NoDecoration          |
			ImGuiWindowFlags.NoInputs              |
			ImGuiWindowFlags.NoDocking             |
			ImGuiWindowFlags.NoMove                |
			ImGuiWindowFlags.NoNav                 |
			ImGuiWindowFlags.NoResize              |
			ImGuiWindowFlags.NoScrollbar           |
			ImGuiWindowFlags.NoTitleBar            |
			ImGuiWindowFlags.NoBringToFrontOnFocus |
			ImGuiWindowFlags.NoFocusOnAppearing
		);
		{
			Gui.TextColored(_messageColors[(int)MessageColor], Message);
		}
		Gui.End();
	}
}