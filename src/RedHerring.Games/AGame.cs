using RedHerring.Core;
using RedHerring.Games.Components;

namespace RedHerring.Games;

public abstract class AGame : AnEssence
{
    public GameComponentCollection Components { get; }

    protected virtual void Update(GameTime gameTime)
    {
        Components.Update(gameTime);
    }
    
    protected virtual bool BeginDraw()
    {
        return true;
    }
    
    protected virtual void Draw(GameTime gameTime)
    {
        Components.Draw(gameTime);
    }

    protected virtual void EndDraw()
    {
    }
}