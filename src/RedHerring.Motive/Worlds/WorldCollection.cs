using System.Collections.ObjectModel;
using RedHerring.Motive.Worlds.Exceptions;

namespace RedHerring.Motive.Worlds;

public class WorldCollection : ObservableCollection<World>
{
    private readonly World _world;
    
    public WorldCollection(World world)
    {
        _world = world;
    }

    protected override void InsertItem(int index, World item)
    {
        if (item.Parent is not null)
        {
            throw new WorldAlreadyHasParentException();
        }

        item.SetParent(_world);
        base.InsertItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        var item = this[index];
        if (item.Parent != _world)
        {
            throw new WorldHasDifferentParentException();
        }

        item.SetParent(null);
        base.RemoveItem(index);
    }
}