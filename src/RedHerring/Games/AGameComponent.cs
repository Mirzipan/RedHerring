using RedHerring.Core.Components;

namespace RedHerring.Games;

public abstract class AGameComponent : AComponent<GameComponentCollection>
{
    public override GameComponentCollection Container { get; }
}