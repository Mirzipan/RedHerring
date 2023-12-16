namespace RedHerring.Detective;

public static class Profiler
{
    private static Task _profilerTask;
    
    
    // TODO(mirzipan) expose recorded data
    // TODO(mirzipan) figure out how long to keep data for
    
    static Profiler()
    {
        _profilerTask = Task.Run(async () =>
        {
            // TODO(mirzipan) await stuff
        });
    }
    
    public static IDisposable Begin(string name)
    {
        var key = new ProfilingKey(name);
        return new ProfilingBlock(key);
    }

    public static void End()
    {
    }

    internal static void End(ProfilingKey key)
    {
        
    }
}