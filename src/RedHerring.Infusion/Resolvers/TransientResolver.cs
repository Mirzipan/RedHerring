using RedHerring.Alexandria.Disposables;

namespace RedHerring.Infusion.Resolvers;

internal sealed class TransientResolver : Resolver
{
    public TransientResolver(Type contract)
    {
        Contract = contract;
    }

    public override object Resolve(InjectionContainer container)
    {
        var instance = container.Instantiate(Contract);
        instance.TryDisposeWith(this);
        
        return instance;
    }
}