using RedHerring.Alexandria.Disposables;

namespace RedHerring.Infusion.Resolvers;

internal sealed class InstanceResolver : AResolver
{
    private readonly object _instance;

    public InstanceResolver(object instance)
    {
        _instance = instance;
        _instance.TryDisposeWith(this);

        Contract = _instance.GetType();
    }

    public override object Resolve(InjectionContainer container)
    {
        return _instance;
    }
}