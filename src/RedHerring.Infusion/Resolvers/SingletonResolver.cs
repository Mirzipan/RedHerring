using RedHerring.Alexandria.Disposables;

namespace RedHerring.Infusion.Resolvers;

internal sealed class SingletonResolver : AResolver
{
    private object? _instance;
    
    public SingletonResolver(Type contract)
    {
        Contract = contract;
    }

    public override object Resolve(InjectionContainer container)
    {
        if (_instance is null)
        {
            _instance = container.Instantiate(Contract);
            _instance.TryDisposeWith(this);
        }

        return _instance;
    }
}