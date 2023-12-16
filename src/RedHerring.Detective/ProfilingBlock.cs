namespace RedHerring.Detective;

public struct ProfilingBlock : IDisposable
{
    private readonly ProfilingKey _key;
    
    internal ProfilingBlock(ProfilingKey key)
    {
        _key = key;
    }
    
    public void Dispose()
    {
        Profiler.End(_key);
    }
}