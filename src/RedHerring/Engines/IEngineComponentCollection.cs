using System.Collections;
using RedHerring.Core.Components;

namespace RedHerring.Engines;

public interface IEngineComponentCollection : IComponentContainer, IEnumerable<AnEngineComponent>, IEnumerable
{
}