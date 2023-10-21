using System.Collections;
using RedHerring.Alexandria;
using RedHerring.Alexandria.Disposables;
using RedHerring.Core;
using RedHerring.Game.Components;
using RedHerring.Infusion;
using RedHerring.Motive.Worlds;

namespace RedHerring.Game;

public sealed class Session : AThingamabob, IEnumerable<ASessionComponent>
{
    private Engine _engine = null!;
    private World? _world;
    
    public InjectionContainer InjectionContainer { get; private set; } = null!;
    public SessionComponentCollection Components { get; }
    public ASessionContext? Context { get; }
    public Engine Engine => _engine;
    public World? World => _world ??= Components.Get<WorldComponent>()?.World;

    public SessionPhase Phase { get; private set; }
    public bool IsPlayable => Phase == SessionPhase.Initialized;

    #region Lifecycle

    public Session(Engine engine, ASessionContext? context): base(context?.Name)
    {
        _engine = engine;
        Context = context;
        Components = new SessionComponentCollection(this);
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
        Phase = SessionPhase.Initializing;
        
        InitFromContext();
        // TODO: loading magic

        Phase = SessionPhase.Initialized;
    }

    public void Close()
    {
        Phase = SessionPhase.Closing;
        
        Dispose();
        // TODO: unload stuff
        // TODO: dispose of stuff

        Phase = SessionPhase.Closed;
    }

    #endregion Lifecycle

    #region Private

    private void InitFromContext()
    {
        Components.Init();
        
        var description = new ContainerDescription($"[Game] {Name}", _engine.InjectionContainer);
        Components.InstallBindings(description);
        InjectionContainer = description.Build();
        InjectionContainer.DisposeWith(this);
        
        Components.Load();
    }

    #endregion Private

    #region IEnumerable

    public IEnumerator<ASessionComponent> GetEnumerator() => Components.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Components.GetEnumerator();

    #endregion IEnumerable
}