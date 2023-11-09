using RedHerring.Alexandria;
using RedHerring.Fingerprint.Layers;
using RedHerring.Game;
using RedHerring.Infusion.Attributes;

namespace RedHerring.Sandbox.Game.Session;

public sealed class GameMenuComponent : SessionComponent, Drawable
{
    [Infuse]
    private InputReceiver _input;
    
    private bool _isVisible;

    public bool IsVisible => _isVisible;

    public int DrawOrder => 0;

    #region Lifecycle

    public bool BeginDraw() => _isVisible;

    public void Draw(GameTime gameTime)
    {
    }

    public void EndDraw()
    {
    }

    #endregion Lifecycle
}