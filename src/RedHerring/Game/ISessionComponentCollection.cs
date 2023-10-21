using System.Collections;
using RedHerring.Alexandria.Components;

namespace RedHerring.Game;

public interface ISessionComponentCollection : IComponentContainer, IEnumerable<ASessionComponent>, IEnumerable
{
}