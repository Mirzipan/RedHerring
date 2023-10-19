using System.Collections;
using RedHerring.Alexandria;
using RedHerring.Engines;
using RedHerring.Games.Components;
using RedHerring.Infusion;
using RedHerring.Motive.Worlds;

namespace RedHerring.Games;

public sealed class Game : AThingamabob, IEnumerable<AGameComponent>
{
    private Engine _engine;
    private World? _world;
    
    public InjectionContainer InjectionContainer { get; private set; } = null!;
    public GameComponentCollection Components { get; }
    public AGameContext? Context { get; }
    public Engine Engine => _engine;
    public World? World => _world ??= Components.Get<WorldComponent>()?.World;

    public GamePhase Phase { get; private set; }
    public bool IsPlayable => Phase == GamePhase.Initialized;

    #region Lifecycle

    private Game(string? name = null) : base(name)
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

    public void Initialize(Engine engine)
    {
        _engine = engine;
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
        Components.Init();
        
        var description = new ContainerDescription($"[Game] {Name}", _engine.InjectionContainer);
        Components.InstallBindings(description);
        InjectionContainer = description.Build();
        
        Components.Load();
    }

    #endregion Private

    #region IEnumerable

    public IEnumerator<AGameComponent> GetEnumerator() => Components.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Components.GetEnumerator();

    #endregion IEnumerable
}