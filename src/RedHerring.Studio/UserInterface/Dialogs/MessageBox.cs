using System.Numerics;
using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Dialogs;

public class MessageBox
{
	private bool _isOpen = true;
	
	public void Update()
	{
		// if (Gui.Button("Open popup"))
		// {
		// 	Gui.OpenPopup("MessageBox");
		// }

		Vector2 center = Gui.GetMainViewport().GetCenter();
		
		Gui.SetNextWindowPos(center, ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));

		if (Gui.BeginPopupModal("MessageBox", ref _isOpen, ImGuiWindowFlags.AlwaysAutoResize))
		{
			Gui.Text("All those beautiful files will be deleted.\nThis operation cannot be undone!");
			Gui.Separator();

			// static bool dont_ask_me_next_time = false;
			// ImGui::PushStyleVar(ImGuiStyleVar_FramePadding, ImVec2(0, 0));
			// ImGui::Checkbox("Don't ask me next time", &dont_ask_me_next_time);
			// ImGui::PopStyleVar();

			if (Gui.Button("OK", new Vector2(120, 0))) 
			{
				Gui.CloseCurrentPopup();
			}
			
			Gui.SetItemDefaultFocus();
			Gui.SameLine();
			
			if (Gui.Button("Cancel", new Vector2(120, 0)))
			{
				Gui.CloseCurrentPopup();
			}
			
			Gui.EndPopup();
		}
	}
}