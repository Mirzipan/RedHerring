using System.Collections.ObjectModel;
using System.Numerics;
using RedHerring.Alexandria;
using RedHerring.Entities;

namespace RedHerring.Worlds;

public sealed class World : IEssence, INameable
{
    private World? _parent;
    private ObservableCollection<World> _children;
    private ObservableCollection<Entity> _entities;
    private Matrix4x4 _worldMatrix;

    public Guid Id { get; set; }
    public string? Name { get; }
    public World? Parent => _parent;
    public ObservableCollection<World> Children => _children;
    public ObservableCollection<Entity> Entities => _entities;
    public Matrix4x4 WorldMatrix => _worldMatrix;

    #region Lifecycle

    public World(string? name = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        
        _children = new WorldCollection(this);
        _entities = new EntityCollection(this);
    }

    #endregion Lifecycle

    #region Queries

    public bool IsEmpty(bool checkChildren)
    {
        if (_entities.Count > 0)
        {
            return true;
        }

        if (!checkChildren)
        {
            return false;
        }

        if (_children.Count == 0)
        {
            return true;
        }

        foreach (var child in _children)
        {
            if (!child.IsEmpty(checkChildren))
            {
                return true;
            }
        }

        return false;
    }

    #endregion Queries

    #region Manipulation

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

    public void SetWorldMatrix(Matrix4x4 matrix, bool applyRecursively)
    {
        _worldMatrix = matrix;
        
        if (applyRecursively && _parent is not null)
        {
            _parent.SetWorldMatrix(matrix, applyRecursively);
        }
    }

    #endregion Manipulation
}