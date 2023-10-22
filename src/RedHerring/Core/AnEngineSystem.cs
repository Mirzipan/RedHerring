using RedHerring.Alexandria.Components;

namespace RedHerring.Core;

public abstract class AnEngineSystem : AComponent<EngineSystemCollection>
{
    private EngineSystemCollection _container = null!;
    public override EngineSystemCollection Container => _container;
    protected AnEngineContext Context => _container.Engine.Context;

    internal void SetContainer(EngineSystemCollection container)
    {
        _container = container;
    }
    
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