using RedHerring.Alexandria;

namespace RedHerring.Game;

public abstract class ASessionComponent : ANamedDisposer
{
    private SessionContext _context = null!;
    public SessionContext Context => _context;

    internal void SetContext(SessionContext context)
    {
        _context = context;
    }
}