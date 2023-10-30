namespace RedHerring.Alexandria;

public sealed class GameTime
{
    public TimeSpan Total { get; internal set; }
    public TimeSpan Elapsed { get; internal set; }
    public int FrameCount { get; internal set; }

    public GameTime() : this(TimeSpan.Zero, TimeSpan.Zero)
    {
    }

    public GameTime(TimeSpan total, TimeSpan elapsed)
    {
        Total = total;
        Elapsed = elapsed;
    }
}