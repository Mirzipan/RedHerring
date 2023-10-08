using System.Collections;
using RedHerring.Alexandria.Components;

namespace RedHerring.Motive.Entities;

public interface IEntityComponentCollection : IComponentContainer, IEnumerable<AnEntityComponent>, IEnumerable
{
}