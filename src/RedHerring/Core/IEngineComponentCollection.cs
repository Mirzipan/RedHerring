using System.Collections;
using RedHerring.Alexandria.Components;

namespace RedHerring.Core;

public interface IEngineComponentCollection : IComponentContainer, IEnumerable<AnEngineComponent>, IEnumerable
{
}