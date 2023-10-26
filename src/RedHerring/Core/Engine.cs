using RedHerring.Alexandria;
using RedHerring.Core.Systems;
using RedHerring.Exceptions;
using RedHerring.Game;
using Silk.NET.Maths;

namespace RedHerring.Core;

public sealed class Engine : AThingamabob
{
    public EngineContext Context { get; private set; } = null!;
    public GraphicsSystem? Renderer { get; private set; } = null!;
    public Session? Session { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsExiting { get; private set; }

    public GameTime UpdateTime { get; }
    public GameTime DrawTime { get; }

    public event Action? OnExit;
    
    private Cronos _cronos;
    private int _updateCount;
    private int _frameCount;
    
    #region Lifecycle

    public Engine()
    {
        IsRunning = false;
        IsExiting = false;

        UpdateTime = new GameTime();
        DrawTime = new GameTime();

        _cronos = new Cronos();
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
        _cronos.Reset();
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
        _cronos.Tick();
        UpdateTime.Update(_cronos.TotalTime, delta, _updateCount);
        Update(UpdateTime);
    }

    public void Draw(double delta)
    {
        if (IsExiting)
        {
            return;
        }
        
        ++_frameCount;
        _cronos.Tick();
        DrawTime.Update(_cronos.TotalTime, _cronos.ElapsedTime, _frameCount);
        
        bool isDrawing = Renderer?.BeginDraw() ?? false;
        if (isDrawing)
        {
            Draw(DrawTime);
            Renderer!.EndDraw();
        }
    }

    public void Exit()
    {
        if (IsExiting)
        {
            return;
        }
        
        IsExiting = true;
        Session?.Close();
        
        Context.Unload();
        
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

    private void InitFromContext()
    {
        Context.Init(this);
        Context.Load();

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