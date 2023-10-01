using System.Collections;
using RedHerring.Core.Components;

namespace RedHerring.Engines;

public sealed class EngineComponentCollection : IEngineComponentCollection
{
    private readonly Dictionary<Type, AnEngineComponent> _componentIndex = new();
    private readonly List<AnEngineComponent> _components = new();

    public EngineComponentCollection()
    {
    }

    #region Queries

    IComponent? IComponentContainer.Get(Type type)
    {
        return _componentIndex.TryGetValue(type, out var value) ? value : null;
    }

    public TEngineComponent? Get<TEngineComponent>() where TEngineComponent : AnEngineComponent
    {
        return _componentIndex.TryGetValue(typeof(TEngineComponent), out var value) ? (TEngineComponent)value : null;
    }

    public bool TryGet<TEngineComponent>(out TEngineComponent? component) where TEngineComponent : AnEngineComponent
    {
        if (_componentIndex.TryGetValue(typeof(TEngineComponent), out var value))
        {
            component = (TEngineComponent)value;
            return true;
        }

        component = null;
        return false;
    }

    public bool TryGet(Type type, out AnEngineComponent? component)
    {
        return _componentIndex.TryGetValue(type, out component);
    }
    
    #endregion Queries

    #region IEnumerable

    IEnumerator<AnEngineComponent> IEnumerable<AnEngineComponent>.GetEnumerator()
    {
        return _components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _components.GetEnumerator();
    }
    
    #endregion IEnumerable
}