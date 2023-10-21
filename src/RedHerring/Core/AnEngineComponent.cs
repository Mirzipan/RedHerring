using RedHerring.Alexandria.Components;

namespace RedHerring.Core;

public abstract class AnEngineComponent : AComponent<EngineComponentCollection>
{
    private EngineComponentCollection _container = null!;
    public override EngineComponentCollection Container => _container;
    protected AnEngineContext Context => _container.Engine.Context;

    internal void SetContainer(EngineComponentCollection container)
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