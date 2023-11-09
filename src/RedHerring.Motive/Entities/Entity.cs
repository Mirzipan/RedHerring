using System.Collections;
using System.Numerics;
using RedHerring.Alexandria;
using RedHerring.Motive.Entities.Components;
using RedHerring.Motive.Worlds;
using RedHerring.Numbers;

namespace RedHerring.Motive.Entities;

public sealed class Entity : Essence, Nameable, IEnumerable<EntityComponent>
{
    internal TransformComponent? _transform;
    internal World? _world;
    
    public string? Name { get; set; }
    public Guid Id { get; set; }
    public EntityComponentCollection Components { get; }
    public TransformComponent? Transform => _transform;
    public World? World { get; internal set; }
    public bool InWorld => World is not null;

    #region Lifecycle

    public Entity() : this(null)
    {
    }

    public Entity(string? name) : this(name, Vector3.Zero)
    {
    }

    public Entity(string? name = null, Vector3 position = default, Quaternion? rotation = null) : this(name, true)
    {
        var matrix = Matrix4x4.CreateWorld(position, Vector3Direction.Forward, Vector3Direction.Up);
        if (rotation is not null)
        {
            matrix = Matrix4x4.Transform(matrix, rotation.Value);
        }
        
        _transform!.LocalMatrix = matrix;
    }

    // `placeholder` only exists to not cause conflicts.
    private Entity(string? name, bool placeholder)
    {
        Id = Guid.NewGuid();
        Name = name;
        
        Components = new EntityComponentCollection(this);
        _transform = new TransformComponent();
        _transform.LocalMatrix = Matrix4x4.Identity;
        Components.Add(_transform);
    }

    #endregion Lifecycle

    #region Queries

    public IEnumerator<EntityComponent> GetEnumerator() => Components.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Components.GetEnumerator();

    #endregion Queries

    #region Manipulation

    public void SetWorld(World? world)
    {
        if (World is not null)
        {
            World.Entities.Remove(this);
        }

        if (world is not null)
        {
            world.Entities.Add(this);
        }
    }

    #endregion Manipulation
}