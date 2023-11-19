using RedHerring.Alexandria;
using RedHerring.Exceptions;
using RedHerring.Game;
using RedHerring.Render;
using Silk.NET.Maths;

namespace RedHerring.Core;

public sealed class Engine : NamedDisposer
{
    private Session? _session;
    
    private GameTimeTracker _updateTimeTracker;
    private GameTimeTracker _drawTimeTracker;
    private int _updateCount;
    private int _frameCount;
    
    public EngineContext Context { get; private set; } = null!;
    public Renderer Renderer { get; private set; } = null!;
    public Session? Session => _session;
    public bool IsRunning { get; private set; }
    public bool IsExiting { get; private set; }

    public GameTime UpdateTime => _updateTimeTracker.Time;
    public GameTime DrawTime => _drawTimeTracker.Time;

    public event Action? OnExit;
    
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

        if (_session is not null)
        {
            CloseSession(ref _session);
        }

        _session = new Session(this, session);
        _session.Initialize();
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

        InitFromContext().GetAwaiter().GetResult();
        
        IsRunning = true;
    }

    public void Update(double delta)
    {
        if (!IsRunning || IsExiting)
        {
            return;
        }

        ++_updateCount;
        _updateTimeTracker.Update(TimeSpan.FromSeconds(delta));
        Update(UpdateTime);
    }

    public void Draw(double delta)
    {
        if (!IsRunning || IsExiting)
        {
            return;
        }
        
        ++_frameCount;
        _drawTimeTracker.Update(TimeSpan.FromSeconds(delta));
        
        bool isDrawing = Renderer.BeginDraw();
        if (isDrawing)
        {
            Draw(DrawTime);
            Renderer.EndDraw();
        }
    }

    public void Close(Session session)
    {
        CloseSession(ref session!);
    }

    public void Close(SessionContext context)
    {
        if (context.Session != Session)
        {
            return;
        }
        
        CloseSession(ref _session);
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

    private async Task InitFromContext(CancellationToken cancellationToken = default)
    {
        Context.Init(this);
        await Context.Load();

        if (Context.Container.HasBinding<Renderer>())
        {
            Renderer = Context.Container.Resolve<Renderer>();
        }
        else
        {
            Renderer = new NullRenderer();
        }
    }

    private void Draw(GameTime time)
    {
        Context.Draw(time);
        Session?.Draw(time);
        
        Renderer.Draw();
    }
    
    private void Update(GameTime time)
    {
        Context.Update(time);
        Session?.Update(time);
    }

    private void CloseSession(ref Session? session)
    {
        session?.Close();
        session?.Dispose();
        session = null;
    }

    #endregion Private
}