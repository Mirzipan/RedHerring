using RedHerring.Core;
using RedHerring.Core.Components;

namespace RedHerring.Games;

public abstract class AGameComponent : AComponent<GameComponentCollection>, IEssence
{
    private GameComponentCollection _container = null!;
    
    public Guid Id { get; set; } = Guid.NewGuid();
    public Game Game => Container.Game;
    public override GameComponentCollection Container => _container;

    internal virtual bool SetContainer(GameComponentCollection container)
    {
        if (Container == container)
        {
            return false;
        }

        _container = container;
        return true;
    }
}