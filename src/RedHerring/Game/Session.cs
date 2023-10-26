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

    public void Update(GameTime gameTime)
    {
        Context.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        Context.Draw(gameTime);
    }

    public void Initialize()
    {
        Phase = SessionPhase.Initializing;
        
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
}