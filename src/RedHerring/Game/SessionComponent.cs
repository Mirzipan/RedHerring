using RedHerring.Alexandria;

namespace RedHerring.Game;

public abstract class SessionComponent : NamedDisposer
{
    private SessionContext _context = null!;
    public SessionContext Context => _context;

    internal void SetContext(SessionContext context)
    {
        _context = context;
    }

    internal void RaiseInit() => Init();
    internal void RaiseClose() => Close();

    protected virtual void Init()
    {
    }

    protected virtual void Close()
    {
    }
}