using RedHerring.Alexandria.Identifiers;

namespace RedHerring.Core;

public interface IDefinable
{
    CompositeId DefinitionId { get; } // TODO: will be used once Clues are added
}