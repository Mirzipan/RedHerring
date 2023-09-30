using RedHerring.Core;
using RedHerring.Worlds;

namespace RedHerring.Entities;

public sealed class Entity : AnEssence
{
    public EntityComponentCollection Components { get; }
    public World? World { get; internal set; }
    public bool InWorld => World is not null;

    public Entity(string? name = null) : base(name)
    {
        Components = new EntityComponentCollection(this);
    }
}