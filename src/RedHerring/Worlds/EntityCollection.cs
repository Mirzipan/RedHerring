using System.Collections.ObjectModel;
using RedHerring.Entities;
using RedHerring.Worlds.Exceptions;

namespace RedHerring.Worlds;

public class EntityCollection : ObservableCollection<Entity>
{
    private readonly World _world;
    
    public EntityCollection(World world)
    {
        _world = world;
    }

    protected override void InsertItem(int index, Entity item)
    {
        if (item.World is not null)
        {
            throw new EntityAlreadyInWorldException();
        }

        item.World = _world;
        base.InsertItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        var item = this[index];
        if (item.World != _world)
        {
            throw new EntityInDifferentWorldException();
        }

        item.World = null;
        base.RemoveItem(index);
    }
}