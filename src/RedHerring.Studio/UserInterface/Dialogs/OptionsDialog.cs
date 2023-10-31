using System.Numerics;
using ImGuiNET;
using Gui = ImGuiNET.ImGui;


namespace RedHerring.Studio.UserInterface.Dialogs;

public sealed class OptionsDialog
{
	private bool _isOpen = true;

	public void Update()
	{
		Vector2 center = Gui.GetMainViewport().GetCenter();
		Gui.SetNextWindowPos(center, ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));
		Gui.SetNextWindowSizeConstraints(new Vector2(400, 200), new Vector2(2000, 2000));

		if (Gui.BeginPopupModal("Options", ref _isOpen))
		{
			
			Gui.EndPopup();
		}
	}
}