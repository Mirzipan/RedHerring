using RedHerring.Core;
using RedHerring.Worlds;

namespace RedHerring.Games;

public sealed class Game : AnEssence
{
    public GameComponentCollection Components { get; }
    public World? World { get; }

    public Game(string? name = null) : base(name)
    {
        Components = new GameComponentCollection();
    }

    private void Update(GameTime gameTime)
    {
        Components.Update(gameTime);
    }

    private bool BeginDraw()
    {
        return true;
    }

    private void Draw(GameTime gameTime)
    {
        Components.Draw(gameTime);
    }

    private void EndDraw()
    {
    }
}