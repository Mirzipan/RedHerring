using System.Collections;
using RedHerring.Core.Components;

namespace RedHerring.Entities;

public sealed class EntityComponentCollection : IEntityComponentCollection
{
    // We don't use a dictionary, because we allows for duplicate components.
    private readonly List<EntityComponent> _components = new();

    public Entity Entity { get; }
    
    public EntityComponentCollection(Entity entity)
    {
        Entity = entity;
    }

    #region Queries

    AComponent? IComponentContainer.Get(Type type)
    {
        int index = IndexOf(type);
        return index >= 0 ? _components[index] : null;
    }

    public TComponent? Get<TComponent>() where TComponent : EntityComponent
    {
        int index = IndexOf<TComponent>();
        return index >= 0 ? _components[index] as TComponent : null;
    }

    public TComponent GetOrCreate<TComponent>() where TComponent : EntityComponent, new()
    {
        if (!TryGet<TComponent>(out var component))
        {
            component = new TComponent();
            AddInternal(component);
        }

        return component!;
    }

    public IEnumerable<TComponent> GetAll<TComponent>() where TComponent : EntityComponent
    {
        for (int i = 0; i < _components.Count; i++)
        {
            var component = _components[i] as TComponent;
            if (component is not null)
            {
                yield return component;
            }
        }
    }

    public bool TryGet<TComponent>(out TComponent? component) where TComponent : EntityComponent
    {
        int index = IndexOf<TComponent>();
        if (index >= 0)
        {
            component = _components[index] as TComponent;
            return true;
        }

        component = null;
        return false;
    }

    public bool TryGet(Type type, out EntityComponent? component)
    {
        int index = IndexOf(type);
        if (index >= 0)
        {
            component = _components[index];
            return true;
        }

        component = null;
        return false;
    }

    public bool Has<TComponent>() where TComponent : EntityComponent => IndexOf<TComponent>() >= 0;

    public bool Has(Type type) => IndexOf(type) >= 0;

    /// <summary>
    /// Returns the first index of this component type.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <returns>-1 if no component of the type was found.</returns>
    public int IndexOf<TComponent>() where TComponent : EntityComponent
    {
        for (int i = 0; i < _components.Count; i++)
        {
            if (_components[i] is TComponent)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Returns the first index of this component type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns>-1 if no component of the type was found.</returns>
    public int IndexOf(Type type)
    {
        for (int i = 0; i < _components.Count; i++)
        {
            if (_components[i].GetType() == type)
            {
                return i;
            }
        }

        return -1;
    }
    
    #endregion Queries

    #region Manipulation

    public TComponent Add<TComponent>(TComponent component) where TComponent : EntityComponent
    {
        AddInternal(component);
        return component;
    }

    public bool Remove(EntityComponent component)
    {
        return RemoveInternal(component);
    }

    public bool Remove<TComponent>(RemovalFilter filter) where TComponent : EntityComponent
    {
        return Remove(typeof(TComponent), filter);
    }

    public bool Remove(Type type, RemovalFilter filter)
    {
        return filter switch
        {
            RemovalFilter.First => RemoveFirst(type),
            RemovalFilter.Last => RemoveLast(type),
            RemovalFilter.All => RemoveAll(type),
            _ => RemoveAll(type),
        };
    }

    public void Clear()
    {
        ClearInternal();
    }

    #endregion Manipulation

    #region IEnumerable

    public IEnumerator<EntityComponent> GetEnumerator()
    {
        return _components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _components.GetEnumerator();
    }
    
    #endregion IEnumerable

    #region Internals

    internal void AddInternal(EntityComponent component)
    {
        // TODO: this will need some more logic and safety
        _components.Add(component);
        if (!component.SetContainer(this))
        {
            return;
        }

        if (Entity.InWorld)
        {
            component.RaiseAddedToWorld();
        }
    }

    internal bool RemoveInternal(EntityComponent component)
    {
        // TODO: this will need some more logic and safety
        if (Entity.InWorld)
        {
            component.RaiseRemoveFromWorld();
        }
        
        component.SetContainer(null);
        return _components.Remove(component);
    }

    internal void ClearInternal()
    {
        // TODO: this will need some more logic and safety
        for (int i = 0; i < _components.Count; i++)
        {
            _components[i].SetContainer(null);
        }
        _components.Clear();
    }

    internal bool RemoveFirst(Type type)
    {
        for (int i = 0; i < _components.Count; i++)
        {
            if (type.IsInstanceOfType(_components[i]))
            {
                RemoveInternal(_components[i]);
                return true;
            }
        }

        return false;
    }

    internal bool RemoveLast(Type type)
    {
        int count = _components.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            if (type.IsInstanceOfType(_components[i]))
            {
                RemoveInternal(_components[i]);
                return true;
            }
        }

        return false;
    }

    internal bool RemoveAll(Type type)
    {
        bool result = false;
        int count = _components.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            if (type.IsInstanceOfType(_components[i]))
            {
                RemoveInternal(_components[i]);
                result = true;
            }
        }

        return result;
    }

    #endregion Internals
}