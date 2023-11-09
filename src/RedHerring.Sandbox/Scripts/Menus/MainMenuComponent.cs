using System.Numerics;
using ImGuiNET;
using RedHerring.Alexandria;
using RedHerring.Game;
using static ImGuiNET.ImGui;

namespace RedHerring.Sandbox.Menus;

public class MainMenuComponent : SessionComponent, Drawable
{
    public bool IsVisible => true;
    public int DrawOrder => 0;

    #region Lifecycle

    public bool BeginDraw() => true;

    public void Draw(GameTime gameTime)
    {
        if (BeginChild("main_menu"))
        {
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
                Quit();
            }
            
            EndChild();
        }
    }

    public void EndDraw()
    {
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

    private void Quit()
    {
        bool isOpen = false;
        
        Vector2 center = GetMainViewport().GetCenter();
        SetNextWindowPos(center, ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));
        
        if (BeginPopupModal("Quit", ref isOpen, ImGuiWindowFlags.AlwaysAutoResize))
        {
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
    }

    #endregion Private
}