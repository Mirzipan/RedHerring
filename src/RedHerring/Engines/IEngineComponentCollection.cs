using System.Collections;
using RedHerring.Alexandria.Components;

namespace RedHerring.Engines;

public interface IEngineComponentCollection : IComponentContainer, IEnumerable<AnEngineComponent>, IEnumerable
{
}