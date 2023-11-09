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
    [Infuse]
    private InputReceiver _input;
    
    private bool _isVisible;

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

    public bool BeginDraw() => _isVisible;

    public void Draw(GameTime gameTime)
    {
        if (BeginChild("game_menu"))
        {
            if (Button("Resume"))
            {
            }
            
            if (Button("Save Game"))
            {
            }
            
            if (Button("Load Game"))
            {
            }

            if (Button("Options"))
            {
            }
            
            if (Button("Exit to Main Menu"))
            {
                Exit();
            }
            
            EndChild();
        }
    }

    public void EndDraw()
    {
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