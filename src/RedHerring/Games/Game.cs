using System.Collections;
using RedHerring.Alexandria;
using RedHerring.Games.Components;
using RedHerring.Worlds;

namespace RedHerring.Games;

public sealed class Game : AThingamabob, IEnumerable<AGameComponent>
{
    private World? _world;
    
    public GameComponentCollection Components { get; }
    public AGameContext? Context { get; }
    public World? World => _world ??= Components.Get<WorldComponent>()?.World;

    public GamePhase Phase { get; private set; }
    public bool IsPlayable => Phase == GamePhase.Initialized;

    #region Lifecycle

    public Game(string? name = null) : base(name)
    {
        Components = new GameComponentCollection(this);
    }

    public Game(AGameContext? context): this(context?.Name)
    {
        Context = context;
    }

    public void Update(GameTime gameTime)
    {
        Components.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        Components.Draw(gameTime);
    }

    public void Initialize()
    {
        Phase = GamePhase.Initializing;
        
        InitFromContext();
        // TODO: loading magic

        Phase = GamePhase.Initialized;
    }

    public void Close()
    {
        Phase = GamePhase.Closing;
        
        // TODO: unload stuff
        // TODO: dispose of stuff

        Phase = GamePhase.Closed;
    }

    #endregion Lifecycle

    #region Private

    private void InitFromContext()
    {
        // TODO: create components based on context
    }

    #endregion Private

    #region IEnumerable

    public IEnumerator<AGameComponent> GetEnumerator() => Components.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Components.GetEnumerator();

    #endregion IEnumerable
}