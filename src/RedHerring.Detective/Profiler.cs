namespace RedHerring.Detective;

public static class Profiler
{
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