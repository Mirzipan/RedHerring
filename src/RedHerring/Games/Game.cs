using RedHerring.Core;
using RedHerring.Worlds;

namespace RedHerring.Games;

public sealed class Game : AnEssence
{
    public GameComponentCollection Components { get; }
    public GameContext? Context { get; }
    public World? World { get; }
    
    public GamePhase Phase { get; private set; }
    
    public bool IsRunning { get; private set; }
    public bool IsExiting { get; private set; }

    #region Lifecycle

    public Game(string? name = null) : base(name)
    {
        Components = new GameComponentCollection();
    }

    public Game(GameContext? context): this(context?.Name)
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
}