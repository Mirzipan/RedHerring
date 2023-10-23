namespace RedHerring.Alexandria;

/// <summary>
/// All times are in milliseconds.
/// </summary>
public class GameTime
{
    public long Total { get; private set; }
    public double Elapsed { get; private set; }
    public int FrameCount { get; private set; }

    public GameTime() : this(0, 0)
    {
    }

    public GameTime(long total, double elapsed)
    {
        Total = total;
        Elapsed = elapsed;
    }

    public void Update(long total, double elapsed, int frameCount)
    {
        Total = total;
        Elapsed = elapsed;
        FrameCount = frameCount;
    }
}