using System.Numerics;
using ImGuiNET;
using RedHerring.Alexandria;
using RedHerring.Game;
using RedHerring.Sandbox.Game;
using static ImGuiNET.ImGui;

namespace RedHerring.Sandbox.MainMenu.Session;

public sealed class MainMenuComponent : SessionComponent, Drawable
{
    private const ImGuiWindowFlags BackgroundWindowFlags = ImGuiWindowFlags.NoSavedSettings 
                                                           | ImGuiWindowFlags.NoCollapse 
                                                           | ImGuiWindowFlags.NoTitleBar 
                                                           | ImGuiWindowFlags.NoResize 
                                                           | ImGuiWindowFlags.NoScrollbar 
                                                           | ImGuiWindowFlags.NoMove 
                                                           | ImGuiWindowFlags.NoBackground;

    private const string QuitModalId = "quit_modal";
    
    public bool IsVisible => true;
    public int DrawOrder => 0;

    #region Lifecycle

    public bool BeginDraw()
    {
        SetNextWindowPos(Vector2.Zero);
        SetNextWindowSize(GetMainViewport().Size);
        Begin("Canvas", BackgroundWindowFlags);
        
        return true;
    }

    public void Draw(GameTime gameTime)
    {
        Spacing();

        if (!BeginChild("main_menu", new Vector2(400, 600), true, BackgroundWindowFlags))
        {
            return;
        }

        if (Button("New Game"))
        {
            NewGame();
        }

        if (Button("Load Game"))
        {
            LoadGame();
        }

        if (Button("Options"))
        {
            ShowOptions();
        }

        if (Button("Quit"))
        {
            OpenPopup(QuitModalId);
        }
        
        DrawQuitModal();

        EndChild();

        Spacing();
    }

    public void EndDraw()
    {
        End();
    }

    #endregion Lifecycle

    #region Private

    private void NewGame()
    {
        var sessionContext = new SessionContext().WithInstaller(new GameSessionInstaller());
        Context.Engine?.Run(sessionContext);
    }

    private void LoadGame()
    {
        var sessionContext = new SessionContext().WithInstaller(new GameSessionInstaller());
        Context.Engine?.Run(sessionContext);
    }

    private void ShowOptions()
    {
        
    }

    private void DrawQuitModal()
    {
        bool isOpen = true;
        
        Vector2 center = GetMainViewport().GetCenter();
        SetNextWindowPos(center, ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));

        if (!BeginPopupModal(QuitModalId, ref isOpen, ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoMove))
        {
            return;
        }

        Text("Are you sure you want to quit the game?.");
        Separator();

        if (Button("Hell Yeah!", new Vector2(120, 0))) 
        {
            CloseCurrentPopup();
            Context.Engine?.Exit();
        }
			
        SetItemDefaultFocus();
        SameLine();
			
        if (Button("Hell No!", new Vector2(120, 0)))
        {
            CloseCurrentPopup();
        }
			
        EndPopup();
    }

    #endregion Private
}