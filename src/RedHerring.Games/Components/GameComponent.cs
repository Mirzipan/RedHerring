using RedHerring.Core.Components;

namespace RedHerring.Games.Components;

public class GameComponent : AComponent<GameComponentCollection>
{
    public override GameComponentCollection Container { get; }
}