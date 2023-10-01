using RedHerring.Core;
using RedHerring.Core.Components;
using RedHerring.Worlds;

namespace RedHerring.Entities;

public abstract class AnEntityComponent : AComponent<EntityComponentCollection>, IEssence
{
    private EntityComponentCollection? _container;

    public Guid Id { get; set; } = Guid.NewGuid();
    public Entity? Entity => _container?.Entity;
    public World? World => _container?.Entity.World;
    public override EntityComponentCollection? Container => _container;

    internal virtual bool SetContainer(EntityComponentCollection? container)
    {
        if (_container == container)
        {
            return false;
        }

        if (_container is not null)
        {
            RaiseRemoveFromContainer();
        }
        
        _container = container;

        if (container is not null)
        {
            RaiseAddedToContainer();
        }

        return true;
    }

    internal void RaiseAddedToContainer()
    {
        AddedToContainer();
    }

    internal void RaiseRemoveFromContainer()
    {
        RemoveFromContainer();
    }

    internal void RaiseAddedToWorld()
    {
        AddedToWorld();
    }

    internal void RaiseRemoveFromWorld()
    {
        RemoveFromWorld();
    }

    protected virtual void AddedToContainer()
    {
    }

    protected virtual void RemoveFromContainer()
    {
    }

    protected virtual void AddedToWorld()
    {
    }

    protected virtual void RemoveFromWorld()
    {
    }
}