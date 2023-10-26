using System.Numerics;
using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class DockSpace
{
	public void Update()
	{
		ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoDocking /*| ImGuiWindowFlags.MenuBar*/;

		ImGuiViewportPtr viewport = Gui.GetMainViewport();
		Gui.SetNextWindowPos(viewport.WorkPos);
		Gui.SetNextWindowSize(new Vector2(viewport.WorkSize.X, viewport.WorkSize.Y - Gui.GetStyle().WindowMinSize.Y));
		Gui.SetNextWindowViewport(viewport.ID);
		Gui.PushStyleVar(ImGuiStyleVar.WindowRounding,   0.0f);
		Gui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
		window_flags |= ImGuiWindowFlags.NoTitleBar            | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove;
		window_flags |= ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus | ImGuiWindowFlags.NoBackground;
	
		Gui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0.0f, 0.0f));

		bool unusedOpen = true;
		Gui.Begin("DockSpace", ref unusedOpen, window_flags);
		Gui.PopStyleVar(3);

		ImGuiIOPtr io = Gui.GetIO();
		if ((io.ConfigFlags & ImGuiConfigFlags.DockingEnable) != 0)
		{
			ImGuiDockNodeFlags dockspace_flags = ImGuiDockNodeFlags.PassthruCentralNode;
			uint               dockspace_id    = Gui.GetID("MyDockSpace");
			Gui.DockSpace(dockspace_id, new Vector2(0.0f, 0.0f), dockspace_flags);
		}
	}
}