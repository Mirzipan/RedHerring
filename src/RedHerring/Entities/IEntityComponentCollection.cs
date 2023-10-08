using System.Collections;
using RedHerring.Alexandria.Components;

namespace RedHerring.Entities;

public interface IEntityComponentCollection : IComponentContainer, IEnumerable<AnEntityComponent>, IEnumerable
{
}