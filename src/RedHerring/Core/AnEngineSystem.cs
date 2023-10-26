using RedHerring.Alexandria;

namespace RedHerring.Core;

public abstract class AnEngineSystem : AThingamabob
{
    private EngineContext _context = null!;
    public EngineContext Context => _context;

    internal void SetContext(EngineContext context) => _context = context;

    internal void RaiseInit()
    {
        Init();
    }

    internal void RaiseLoad()
    {
        Load();
    }

    internal void RaiseUnload()
    {
        Unload();
    }

    /// <summary>
    /// Called during engine initialization.
    /// </summary>
    protected abstract void Init();

    /// <summary>
    /// Called during engine startup, after initialization.
    /// </summary>
    protected virtual void Load()
    {
        
    }

    /// <summary>
    /// Called when engine is shutting down.
    /// </summary>
    protected virtual void Unload()
    {
        
    }
}