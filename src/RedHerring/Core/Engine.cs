using RedHerring.Alexandria;
using RedHerring.Core.Systems;
using RedHerring.Exceptions;
using RedHerring.Game;
using Silk.NET.Maths;

namespace RedHerring.Core;

public sealed class Engine : ANamedDisposer
{
    public EngineContext Context { get; private set; } = null!;
    public GraphicsSystem? Renderer { get; private set; } = null!;
    public Session? Session { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsExiting { get; private set; }

    public GameTime UpdateTime => _updateTimeTracker.Time;
    public GameTime DrawTime => _drawTimeTracker.Time;

    public event Action? OnExit;
    
    private GameTimeTracker _updateTimeTracker;
    private GameTimeTracker _drawTimeTracker;
    private int _updateCount;
    private int _frameCount;
    
    #region Lifecycle

    public Engine()
    {
        IsRunning = false;
        IsExiting = false;

        _updateTimeTracker = new GameTimeTracker();
        _drawTimeTracker = new GameTimeTracker();
    }

    public void Run(SessionContext session)
    {
        if (!IsRunning)
        {
            throw new EngineNotRunningException();
        }

        Session = new Session(this, session);
        Session.Initialize();
    }

    public void Run(EngineContext context)
    {
        if (IsRunning)
        {
            throw new EngineAlreadyRunningException();
        }
        
        Context = context;
        _updateTimeTracker.Reset();
        _drawTimeTracker.Reset();
        
        _updateCount = 0;
        _frameCount = 0;

        InitFromContext();
        
        IsRunning = true;
    }

    public void Update(double delta)
    {
        if (IsExiting)
        {
            return;
        }

        ++_updateCount;
        _updateTimeTracker.Update(TimeSpan.FromSeconds(delta));
        Update(UpdateTime);
    }

    public void Draw(double delta)
    {
        if (IsExiting)
        {
            return;
        }
        
        ++_frameCount;
        _drawTimeTracker.Update(TimeSpan.FromSeconds(delta));
        
        bool isDrawing = Renderer?.BeginDraw() ?? false;
        if (isDrawing)
        {
            Draw(DrawTime);
            Renderer!.EndDraw();
        }
    }

    public async void Exit()
    {
        if (IsExiting)
        {
            return;
        }
        
        IsExiting = true;
        Session?.Close();
        
        await Context.Unload();
        
        OnExit?.Invoke();
    }

    #endregion Lifecycle

    #region Public

    public void Resize(Vector2D<int> size)
    {
        Renderer?.Resize(size);
    }

    #endregion Public

    #region Private

    private async void InitFromContext()
    {
        Context.Init(this);
        await Context.Load();

        Renderer = Context.Container.Resolve<GraphicsSystem>();
    }

    private void Draw(GameTime time)
    {
        Context.Draw(time);
        Session?.Draw(time);
        
        Renderer!.Draw();
    }
    
    private void Update(GameTime time)
    {
        Context.Update(time);
        Session?.Update(time);
    }

    #endregion Private
}