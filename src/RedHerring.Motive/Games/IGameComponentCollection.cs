using System.Collections;
using RedHerring.Alexandria.Components;

namespace RedHerring.Motive.Games;

public interface IGameComponentCollection : IComponentContainer, IEnumerable<AGameComponent>, IEnumerable
{
}