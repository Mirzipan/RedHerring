using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class MainMenu
{
	public void Update()
	{
		if (Gui.BeginMainMenuBar())
		{
			if (Gui.BeginMenu("File"))
			{
				if (Gui.MenuItem("Open..", "Ctrl+O"))
				{
					//Gui.OpenPopup("MessageBox"); <-- not working!
				}

				Gui.MenuItem("Save",   "Ctrl+S");
				Gui.MenuItem("Save As..");
				Gui.Separator();
				Gui.MenuItem("Exit");
				Gui.EndMenu();
			}

			if (Gui.BeginMenu("Edit"))
			{
				Gui.MenuItem("Undo");
				Gui.MenuItem("Redo");
				Gui.EndMenu();
			}

			Gui.EndMainMenuBar();
		}
	}
}