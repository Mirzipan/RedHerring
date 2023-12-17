using System.Numerics;
using ImGuiNET;
using RedHerring.Alexandria;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Layers;
using RedHerring.Game;
using RedHerring.Infusion.Attributes;
using RedHerring.Sandbox.MainMenu;
using static ImGuiNET.ImGui;

namespace RedHerring.Sandbox.Game.Session;

public sealed class GameMenuComponent : SessionComponent, Drawable
{
    private const ImGuiWindowFlags BackgroundWindowFlags = ImGuiWindowFlags.NoSavedSettings 
                                                           | ImGuiWindowFlags.NoCollapse 
                                                           | ImGuiWindowFlags.NoTitleBar 
                                                           | ImGuiWindowFlags.NoResize 
                                                           | ImGuiWindowFlags.NoScrollbar 
                                                           | ImGuiWindowFlags.NoMove 
                                                           | ImGuiWindowFlags.NoBackground;
    
    private const ImGuiWindowFlags ChildFlags = ImGuiWindowFlags.NoSavedSettings 
                                                | ImGuiWindowFlags.NoCollapse 
                                                | ImGuiWindowFlags.NoTitleBar 
                                                | ImGuiWindowFlags.AlwaysAutoResize 
                                                | ImGuiWindowFlags.NoScrollbar 
                                                | ImGuiWindowFlags.NoMove;
    
    [Infuse]
    private InputReceiver _input;
    
    private bool _isVisible;
    private bool _isOpen = true;

    public bool IsVisible => _isVisible;

    public int DrawOrder => 0;

    #region Lifecycle

    protected override void Init()
    {
        _input.Name = "game_menu";
        _input.ConsumesAllInput = true;
        _input.Bind("toggle_menu", InputState.Released, OnToggleMenu);
        
        _input.Push();
    }

    protected override void Close()
    {
        _input.Pop();
    }

    public bool BeginDraw()
    {
        if (!_isVisible)
        {
            return false;
        }
        
        SetNextWindowPos(Vector2.Zero);
        SetNextWindowSize(GetMainViewport().Size);
        Begin("Canvas", BackgroundWindowFlags);
        
        return true;
    }

    public void Draw(GameTime gameTime)
    {
        var center = GetMainViewport().GetCenter();
        SetNextWindowPos(center, ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));
        
        if (BeginChild("game_menu", Vector2.Zero, ImGuiChildFlags.None, ChildFlags))
        {
            var buttonSize = new Vector2(300, 20);
            
            if (Button("Resume", buttonSize))
            {
                _isVisible = false;
            }
            
            if (Button("Save Game", buttonSize))
            {
            }
            
            if (Button("Load Game", buttonSize))
            {
            }

            if (Button("Options", buttonSize))
            {
            }
            
            if (Button("Exit to Main Menu", buttonSize))
            {
                Exit();
            }
            
            EndChild();
        }
    }

    public void EndDraw()
    {
        End();
    }

    #endregion Lifecycle

    #region Private

    private void Exit()
    {
        var sessionContext = new SessionContext().WithInstaller(new MainSessionInstaller());
        Context.Engine?.Run(sessionContext);
    }

    #endregion Private

    #region Input

    private void OnToggleMenu(ref ActionEvent evt)
    {
        evt.Consumed = true;
        _isVisible = !_isVisible;
    }

    #endregion Input
}