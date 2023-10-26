using System.Numerics;
using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public class TestWindows
{
	private string? _save;
	
	public void Update()
	{
		ShowExampleAppDockSpace();
		
		// background dock area
		// ImGuiStylePtr style        = Gui.GetStyle();
		// Vector2       viewportSize = Gui.GetMainViewport().Size;
		//
		// Gui.SetNextWindowPos(new Vector2(0, style.WindowMinSize.Y));
		// Gui.SetNextWindowSize(new Vector2(viewportSize.X, viewportSize.Y - 2*style.WindowMinSize.Y));
		//
		// Gui.Begin("BackgroundDockableArea",
		// 	ImGuiWindowFlags.NoCollapse            |
		// 	ImGuiWindowFlags.NoDecoration          |
		// 	ImGuiWindowFlags.NoInputs              |
		// 	ImGuiWindowFlags.NoMove                |
		// 	ImGuiWindowFlags.NoNav                 |
		// 	ImGuiWindowFlags.NoResize              |
		// 	ImGuiWindowFlags.NoScrollbar           |
		// 	ImGuiWindowFlags.NoTitleBar            |
		// 	ImGuiWindowFlags.NoBringToFrontOnFocus |
		// 	ImGuiWindowFlags.NoFocusOnAppearing
		// 	//ImGuiWindowFlags.NoBackground
		// );
		// Gui.End();

		// test windows
		Gui.Begin("Dockable window");
		{
			Gui.Text("This is a dockable window");
			if (Gui.Button("Save"))
			{
				_save = Gui.SaveIniSettingsToMemory();
			}

			if (Gui.Button("Load"))
			{
				Gui.LoadIniSettingsFromMemory(_save);
			}
		}
		Gui.End();

		Gui.Begin("Another dockable window");
		{
			Gui.Text("This is also a dockable window");
		}
		Gui.End();
	}

	private bool               opt_fullscreen  = true;
	private bool               opt_padding     = false;
	private ImGuiDockNodeFlags dockspace_flags = ImGuiDockNodeFlags.None | ImGuiDockNodeFlags.PassthruCentralNode;
	private bool               p_open          = true;
	
	void ShowExampleAppDockSpace()
	{
		// If you strip some features of, this demo is pretty much equivalent to calling DockSpaceOverViewport()!
		// In most cases you should be able to just call DockSpaceOverViewport() and ignore all the code below!
		// In this specific demo, we are not using DockSpaceOverViewport() because:
		// - we allow the host window to be floating/moveable instead of filling the viewport (when opt_fullscreen == false)
		// - we allow the host window to have padding (when opt_padding == true)
		// - we have a local menu bar in the host window (vs. you could use BeginMainMenuBar() + DockSpaceOverViewport() in your code!)
		// TL;DR; this demo is more complicated than what you would normally use.
		// If we removed all the options we are showcasing, this demo would become:
		//     void ShowExampleAppDockSpace()
		//     {
		//         Gui.DockSpaceOverViewport(Gui.GetMainViewport());
		//     }

		// We are using the ImGuiWindowFlags.NoDocking flag to make the parent window not dockable into,
		// because it would be confusing to have two docking targets within each others.
		ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoDocking /*| ImGuiWindowFlags.MenuBar*/;
		if (opt_fullscreen)
		{
			ImGuiViewportPtr viewport = Gui.GetMainViewport();
			Gui.SetNextWindowPos(viewport.WorkPos);
			Gui.SetNextWindowSize(new Vector2(viewport.WorkSize.X, viewport.WorkSize.Y - Gui.GetStyle().WindowMinSize.Y));
			Gui.SetNextWindowViewport(viewport.ID);
			Gui.PushStyleVar(ImGuiStyleVar.WindowRounding,   0.0f);
			Gui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
			window_flags |= ImGuiWindowFlags.NoTitleBar            | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove;
			window_flags |= ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus | ImGuiWindowFlags.NoBackground;
		}
		else
		{
			dockspace_flags &= ~ImGuiDockNodeFlags.PassthruCentralNode;
		}

		// When using ImGuiDockNodeFlags.PassthruCentralNode, DockSpace() will render our background
		// and handle the pass-thru hole, so we ask Begin() to not render a background.
		if ((dockspace_flags & ImGuiDockNodeFlags.PassthruCentralNode) != 0)
			window_flags |= ImGuiWindowFlags.NoBackground;

		// Important: note that we proceed even if Begin() returns false (aka window is collapsed).
		// This is because we want to keep our DockSpace() active. If a DockSpace() is inactive,
		// all active windows docked into it will lose their parent and become undocked.
		// We cannot preserve the docking relationship between an active window and an inactive docking, otherwise
		// any change of dockspace/settings would lead to windows being stuck in limbo and never being visible.
		if (!opt_padding)
			Gui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0.0f, 0.0f));
		Gui.Begin("DockSpace Demo",ref p_open, window_flags);
		if (!opt_padding)
			Gui.PopStyleVar();

		if (opt_fullscreen)
			Gui.PopStyleVar(2);

		// Submit the DockSpace
		ImGuiIOPtr io = Gui.GetIO();
		if ((io.ConfigFlags & ImGuiConfigFlags.DockingEnable) != 0)
		{
			uint dockspace_id = Gui.GetID("MyDockSpace");
			Gui.DockSpace(dockspace_id, new Vector2(0.0f, 0.0f), dockspace_flags);
		}
		else
		{
			//ShowDockingDisabledMessage();
		}

		/*
		if (Gui.BeginMenuBar())
		{
			if (Gui.BeginMenu("Options"))
			{
				// Disabling fullscreen would allow the window to be moved to the front of other windows,
				// which we can't undo at the moment without finer window depth/z control.
				Gui.MenuItem("Fullscreen", null, opt_fullscreen);
				Gui.MenuItem("Padding",    null, opt_padding);
				Gui.Separator();

				if (Gui.MenuItem("Flag: NoSplit",                "", (dockspace_flags & ImGuiDockNodeFlags.NoSplit) != 0))                 { dockspace_flags ^= ImGuiDockNodeFlags.NoSplit; }
				if (Gui.MenuItem("Flag: NoResize",               "", (dockspace_flags & ImGuiDockNodeFlags.NoResize) != 0))                { dockspace_flags ^= ImGuiDockNodeFlags.NoResize; }
				if (Gui.MenuItem("Flag: NoDockingInCentralNode", "", (dockspace_flags & ImGuiDockNodeFlags.NoDockingInCentralNode) != 0))  { dockspace_flags ^= ImGuiDockNodeFlags.NoDockingInCentralNode; }
				if (Gui.MenuItem("Flag: AutoHideTabBar",         "", (dockspace_flags & ImGuiDockNodeFlags.AutoHideTabBar) != 0))          { dockspace_flags ^= ImGuiDockNodeFlags.AutoHideTabBar; }
				if (Gui.MenuItem("Flag: PassthruCentralNode",    "", (dockspace_flags & ImGuiDockNodeFlags.PassthruCentralNode) != 0, opt_fullscreen)) { dockspace_flags ^= ImGuiDockNodeFlags.PassthruCentralNode; }
				Gui.Separator();

				if (Gui.MenuItem("Close", null, false, p_open))
					p_open = false;
				Gui.EndMenu();
			}
			
			Gui.Text(
				"When docking is enabled, you can ALWAYS dock MOST window into another! Try it now!\n"+
			"- Drag from window title bar or their tab to dock/undock.\n"+
			"- Drag from window menu button (upper-left button) to undock an entire node (all windows).\n"+
			"- Hold SHIFT to disable docking (if io.ConfigDockingWithShift == false, default)\n"+
			"- Hold SHIFT to enable docking (if io.ConfigDockingWithShift == true)\n"+
			"This demo app has nothing to do with enabling docking!\n\n"+
			"This demo app only demonstrate the use of Gui.DockSpace() which allows you to manually create a docking node _within_ another window.\n\n"+
			"Read comments in ShowExampleAppDockSpace() for more details."
				);

			Gui.EndMenuBar();
		}
		*/

		Gui.End();
	}	
	
}