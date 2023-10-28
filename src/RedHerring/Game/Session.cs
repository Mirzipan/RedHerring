using RedHerring.Alexandria;
using RedHerring.Core;

namespace RedHerring.Game;

public sealed class Session : AThingamabob
{
    private Engine _engine = null!;
    public Engine Engine => _engine;
    public SessionContext Context { get; private set; } = null!;

    public SessionPhase Phase { get; private set; }
    public bool IsPlayable => Phase == SessionPhase.Initialized;

    #region Lifecycle

    public Session(Engine engine, SessionContext context): base(context.Name)
    {
        _engine = engine;
        Context = context;
    }

    internal void Update(GameTime gameTime)
    {
        Context.Update(gameTime);
    }

    internal void Draw(GameTime gameTime)
    {
        Context.Draw(gameTime);
    }

    internal void Initialize()
    {
        Phase = SessionPhase.Initializing;
        
        // TODO: loading magic
        Context.Init(_engine, this);

        Phase = SessionPhase.Initialized;
    }

    internal void Close()
    {
        Phase = SessionPhase.Closing;
        
        Dispose();
        // TODO: unload stuff
        // TODO: dispose of stuff

        Phase = SessionPhase.Closed;
    }

    #endregion Lifecycle
}