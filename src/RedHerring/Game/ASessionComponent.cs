using RedHerring.Alexandria;
using RedHerring.Alexandria.Components;

namespace RedHerring.Game;

public abstract class ASessionComponent : AComponent<SessionComponentCollection>, IEssence
{
    private SessionComponentCollection _container = null!;
    
    public Guid Id { get; set; } = Guid.NewGuid();
    public Session Session => Container.Session;
    public override SessionComponentCollection Container => _container;

    internal virtual bool SetContainer(SessionComponentCollection container)
    {
        if (Container == container)
        {
            return false;
        }

        _container = container;
        return true;
    }
}