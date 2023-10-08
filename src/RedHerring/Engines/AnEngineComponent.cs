using RedHerring.Alexandria.Components;

namespace RedHerring.Engines;

public abstract class AnEngineComponent : AComponent<Engine>
{
    private Engine _container = null!;
    public override Engine Container => _container;
    protected AnEngineContext Context => _container.Context;

    internal void SetContainer(Engine container)
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