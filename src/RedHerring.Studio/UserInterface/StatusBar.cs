using System.Numerics;
using ImGuiNET;
using RedHerring.Numbers;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class StatusBar
{
	// TODO - this is very similar to console - refactor
	public enum Color
	{
		Info    = 0,
		Warning = 1,
		Error   = 2,
	}

	private readonly Color4[] _messageColors =
	{
		Color4.LightGreen, // Info
		Color4.Gold, // Warning
		Color4.Crimson, // Error
	};

	public string Message      { get; set; } = "";
	public Color  MessageColor { get; set; } = Color.Info;

	public void Update()
	{
		ImGuiStylePtr style          = Gui.GetStyle();
		Vector2       viewportSize   = Gui.GetMainViewport().Size;
		float         statusBarSizeY = Gui.CalcTextSize("|j").Y * 2; // style.WindowMinSize.Y;
		
		Gui.SetNextWindowPos(new Vector2(0, viewportSize.Y - statusBarSizeY));
		Gui.SetNextWindowSize(new Vector2(viewportSize.X, statusBarSizeY));
		
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