using RedHerring.Core;
using RedHerring.Engines.Exceptions;
using RedHerring.Games;
using RedHerring.Render;
using Silk.NET.Maths;

namespace RedHerring.Engines;

public sealed class Engine : AThingamabob
{
    public AnEngineContext Context { get; private set; }
    public Game? Game { get; private set; }
    public Renderer Renderer { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsExiting { get; private set; }

    public GameTime UpdateTime { get; private set; }
    public GameTime DrawTime { get; private set; }
    
    private Cronos _cronos;
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

    public void Run(Game game)
    {
        if (!IsRunning)
        {
            throw new EngineNotRunningException();
        }
        
        Game = game;
        Game.Initialize();
    }

    public void Run(AnEngineContext context)
    {
        if (IsRunning)
        {
            throw new EngineAlreadyRunningException();
        }

        Context = context;
        _cronos.Reset();
        _frameCount = 0;

        InitFromContext();
        
        IsRunning = true;
    }

    public void Tick()
    {
        if (IsExiting)
        {
            return;
        }

        _cronos.Tick();
        
        ++_frameCount;

        TickInternal();
    }

    public void Exit()
    {
        IsExiting = true;
        Game?.Close();
    }

    #endregion Lifecycle

    #region Public

    public void Resize(Vector2D<int> size)
    {
        Renderer.Resize(size);
    }

    #endregion Public

    #region Private

    private void InitFromContext()
    {
        Renderer = new Renderer(Context.View, Context.GraphicsBackend);
    }

    private void TickInternal()
    {
        bool isDrawing = Renderer.BeginDraw();

        // TODO: make draw and update independent
        UpdateTime.Update(_cronos.TotalTime, _cronos.ElapsedTime, _frameCount);
        Update(UpdateTime);        
        
        DrawTime.Update(_cronos.TotalTime, _cronos.ElapsedTime, _frameCount);
        Draw(DrawTime);

        if (isDrawing)
        {
            Renderer.EndDraw();
        }
    }

    private void Draw(GameTime time)
    {
        Game?.Draw(time);
        
        Renderer.Draw();
    }

    private void Update(GameTime time)
    {
        Game?.Update(time);
    }

    #endregion Private
}