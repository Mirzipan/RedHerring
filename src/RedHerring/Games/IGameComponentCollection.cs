using System.Collections;
using RedHerring.Core.Components;

namespace RedHerring.Games;

public interface IGameComponentCollection : IComponentContainer, IEnumerable<GameComponent>, IEnumerable
{
}