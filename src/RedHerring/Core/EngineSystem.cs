using RedHerring.Alexandria;

namespace RedHerring.Core;

public abstract class EngineSystem : NamedDisposer
{
    private EngineContext _context = null!;
    public EngineContext Context => _context;

    internal void SetContext(EngineContext context) => _context = context;

    internal void RaiseInit()
    {
        Init();
    }

    internal async ValueTask<int> RaiseLoad()
    {
        return await Load();
    }

    internal async ValueTask<int> RaiseUnload()
    {
        return await Unload();
    }

    /// <summary>
    /// Called during engine initialization.
    /// </summary>
    protected abstract void Init();

    /// <summary>
    /// Called during engine startup, after initialization.
    /// </summary>
    protected virtual ValueTask<int> Load()
    {
        return ValueTask.FromResult(0);
    }

    /// <summary>
    /// Called when engine is shutting down.
    /// </summary>
    protected virtual ValueTask<int> Unload()
    {
        return ValueTask.FromResult(0);
    }
}