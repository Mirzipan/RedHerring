using RedHerring.Core.Components;

namespace RedHerring.Games;

public class GameComponent : AComponent<GameComponentCollection>
{
    public override GameComponentCollection Container { get; }
}