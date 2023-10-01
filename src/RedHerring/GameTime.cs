namespace RedHerring;

/// <summary>
/// All times are in milliseconds.
/// </summary>
public class GameTime
{
    public long Total { get; private set; }
    public long Elapsed { get; private set; }
    public int FrameCount { get; private set; }

    public GameTime() : this(0, 0)
    {
    }

    public GameTime(long total, long elapsed)
    {
        Total = total;
        Elapsed = elapsed;
    }

    internal void Update(long total, long elapsed, int frameCount)
    {
        Total = total;
        Elapsed = elapsed;
        FrameCount = frameCount;
    }
}