using System.Collections;
using RedHerring.Core.Components;

namespace RedHerring.Engines;

public sealed class EngineComponentCollection : IComponentContainer, IEngineComponentCollection
{
    private readonly Dictionary<Type, EngineComponent> _componentIndex = new();
    private readonly List<EngineComponent> _components = new();

    public EngineComponentCollection()
    {
    }

    #region Queries

    AComponent? IComponentContainer.Get(Type type)
    {
        return _componentIndex.TryGetValue(type, out var value) ? value : null;
    }

    public TEngineComponent? Get<TEngineComponent>() where TEngineComponent : EngineComponent
    {
        return _componentIndex.TryGetValue(typeof(TEngineComponent), out var value) ? (TEngineComponent)value : null;
    }

    public bool TryGet<TEngineComponent>(out TEngineComponent? component) where TEngineComponent : EngineComponent
    {
        if (_componentIndex.TryGetValue(typeof(TEngineComponent), out var value))
        {
            component = (TEngineComponent)value;
            return true;
        }

        component = null;
        return false;
    }

    public bool TryGet(Type type, out EngineComponent? component)
    {
        return _componentIndex.TryGetValue(type, out component);
    }
    
    #endregion Queries

    #region IEnumerable

    IEnumerator<EngineComponent> IEnumerable<EngineComponent>.GetEnumerator()
    {
        return _components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _components.GetEnumerator();
    }
    
    #endregion IEnumerable
}