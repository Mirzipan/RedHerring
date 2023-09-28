using RedHerring.Core;
using RedHerring.Exceptions;

namespace RedHerring;

public class Engine : AnEssence
{
    public EngineContext Context { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsExiting { get; private set; }

    public GameTime UpdateTime { get; private set; }
    public GameTime DrawTime { get; private set; }
    
    private Ticker _ticker;
    private int _frameCount;
    
    #region Lifecycle

    public Engine()
    {
        IsRunning = false;
        IsExiting = false;

        _ticker = new Ticker();
    }

    public void Run(EngineContext context)
    {
        if (IsRunning)
        {
            throw new EngineAlreadyRunningException();
        }

        Context = context;
        _ticker.Reset();
        _frameCount = 0;
        
        IsRunning = true;
    }

    public void Tick()
    {
        if (IsExiting)
        {
            return;
        }

        _ticker.Tick();
        
        ++_frameCount;
        
        // TODO: make draw independent
        DrawTime.Update(_ticker.TotalTime, _ticker.ElapsedTime, _frameCount);
        UpdateTime.Update(_ticker.TotalTime, _ticker.ElapsedTime, _frameCount);
    }

    public void Draw(GameTime time)
    {
    }

    public void Update(GameTime time)
    {
    }

    public void Exit()
    {
        IsExiting = true;
    }

    #endregion Lifecycle
}