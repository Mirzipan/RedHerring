using System.Collections;

namespace RedHerring.Entities;

public interface IEntityComponentCollection : IEnumerable<EntityComponent>, IEnumerable
{
}