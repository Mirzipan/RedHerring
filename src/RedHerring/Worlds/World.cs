using System.Collections.ObjectModel;
using RedHerring.Core;
using RedHerring.Entities;

namespace RedHerring.Worlds;

public sealed class World : AnEssence
{
    private World? _parent;
    private ObservableCollection<World> _children;
    private ObservableCollection<Entity> _entities;

    public World? Parent => _parent;
    public ObservableCollection<World> Children => _children;
    public ObservableCollection<Entity> Entities => _entities;

    public World(string? name = null) : base(name)
    {
        _children = new WorldCollection(this);
        _entities = new EntityCollection(this);
    }

    public bool SetParent(World? parent)
    {
        if (_parent == parent)
        {
            return false;
        }

        if (_parent is not null)
        {
            _parent._children.Remove(this);
        }
        
        _parent = parent;

        if (parent is not null)
        {
            parent._children.Add(this);
        }

        return true;
    }
}