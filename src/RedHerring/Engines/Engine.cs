using RedHerring.Core;
using RedHerring.Engines.Exceptions;
using RedHerring.Games;
using RedHerring.Render;

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

        _cronos = new Cronos();
    }

    public void Run(Game game)
    {
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
        
        // TODO: make draw independent
        DrawTime.Update(_cronos.TotalTime, _cronos.ElapsedTime, _frameCount);
        UpdateTime.Update(_cronos.TotalTime, _cronos.ElapsedTime, _frameCount);
    }

    public void Draw(GameTime time)
    {
        Renderer.BeginDraw();
        
        Game?.Draw(time);
        
        Renderer.Draw();
        Renderer.EndDraw();
    }

    public void Update(GameTime time)
    {
        Game?.Update(time);
    }

    public void Exit()
    {
        IsExiting = true;
        Game?.Close();
    }

    #endregion Lifecycle
}