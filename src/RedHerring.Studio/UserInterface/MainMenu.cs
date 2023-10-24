using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class MainMenu
{
	public Action? OnUndo;
	public Action? OnRedo;
	public Action? OnExit;
	
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
				ExitItem();
				Gui.EndMenu();
			}

			if (Gui.BeginMenu("Edit"))
			{
				UndoRedoItems();
				Gui.EndMenu();
			}

			Gui.EndMainMenuBar();
		}
	}

	private void ExitItem()
	{
		if (Gui.MenuItem("Exit"))
		{
			OnExit?.Invoke();
		}
	}

	private void UndoRedoItems()
	{
		if (Gui.MenuItem("Undo"))
		{
			OnUndo?.Invoke();
		}

		if (Gui.MenuItem("Redo"))
		{
			OnRedo?.Invoke();
		}
	}
}