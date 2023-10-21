using RedHerring.Alexandria;
using RedHerring.Motive.Worlds;

namespace RedHerring.Game.Components;

public sealed class WorldComponent : ASessionComponent, IDrawable
{
    // TODO: any special tracking if necessary, if not, consider removing this component
    private World _world;

    public bool IsVisible => true;
    public int DrawOrder => 0;
    public World World => _world;

    #region Lifecycle

    public WorldComponent()
    {
        _world = new World();
    }

    public bool BeginDraw()
    {
        return true;
    }

    public void Draw(GameTime gameTime)
    {
    }

    public void EndDraw()
    {
    }

    #endregion Lifecycle
}