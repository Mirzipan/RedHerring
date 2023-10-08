using System.Collections;
using RedHerring.Alexandria.Components;
using RedHerring.Engines.Exceptions;
using RedHerring.Extensions.Collections;

namespace RedHerring.Engines;

public sealed class EngineComponentCollection : IEngineComponentCollection
{
    private readonly Dictionary<Type, AnEngineComponent> _componentIndex = new();
    private readonly List<AnEngineComponent> _components = new();
    private readonly Engine _engine;

    public Engine Engine => _engine;

    #region Lifecycle

    public EngineComponentCollection(Engine engine)
    {
        _engine = engine;
    }

    internal void Init()
    {
        if (_engine.Context.Components.IsNullOrEmpty())
        {
            return;
        }

        int count = _engine.Context.Components.Count;
        for (int i = 0; i < count; i++)
        {
            var component = _engine.Context.Components[i];
            if (!component.Type.IsSubclassOf(typeof(AnEngineComponent)))
            {
                throw new TypeIsNotAnEngineComponentException(component.Type);
            }
            
            object? instance = Activator.CreateInstance(component.Type);
            if (instance is null)
            {
                throw new NullReferenceException();
            }

            var componentInstance = (instance as AnEngineComponent)!;
            _components.Add(componentInstance);
            _componentIndex[component.Type] = componentInstance;
        }
        
        count = _components.Count;
        for (int i = 0; i < count; i++)
        {
            var component = _components[i];
            component.SetContainer(_engine);
            component.RaiseInit();
        }
    }

    internal void Load()
    {
        int count = _components.Count;
        for (int i = 0; i < count; i++)
        {
            var component = _components[i];
            component.RaiseLoad();
        }
    }

    internal void Unload()
    {
        int count = _components.Count;
        for (int i = 0; i < count; i++)
        {
            var component = _components[i];
            component.RaiseUnload();
        }
    }

    #endregion Lifecycle

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