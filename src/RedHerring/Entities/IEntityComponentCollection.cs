using System.Collections;
using RedHerring.Core.Components;

namespace RedHerring.Entities;

public interface IEntityComponentCollection : IComponentContainer, IEnumerable<EntityComponent>, IEnumerable
{
}