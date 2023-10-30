namespace RedHerring.Alexandria;

public sealed class GameTimeTracker
{
    private GameTime _time = new();

    public GameTime Time => _time;
    
    public GameTime Reset()
    {
        _time.Elapsed = TimeSpan.Zero;
        _time.Total = TimeSpan.Zero;
        _time.FrameCount = 0;
        return _time;
    }

    public GameTime Update(TimeSpan delta)
    {
        _time.Elapsed = delta;
        _time.Total = _time.Total.Add(delta);
        _time.FrameCount++;
        return _time;
    }
}