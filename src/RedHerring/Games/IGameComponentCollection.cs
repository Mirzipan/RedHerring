using System.Collections;
using RedHerring.Alexandria.Components;

namespace RedHerring.Games;

public interface IGameComponentCollection : IComponentContainer, IEnumerable<AGameComponent>, IEnumerable
{
}