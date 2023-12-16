using System.Reactive.Disposables;
using RedHerring.Alexandria.Disposables;

namespace RedHerring.Infusion.Resolvers;

internal sealed class TransientTypeResolver : Resolver
{
    private readonly Type _contract;
    private readonly CompositeDisposable _composite = new();
    
    public TransientTypeResolver(Type contract)
    {
        _contract = contract;
    }

    public object Resolve(InjectionContainer container)
    {
        object instance = container.Instantiate(_contract);
        instance.TryDisposeWith(_composite);
        
        return instance;
    }

    public void Dispose()
    {
        _composite.Dispose();
    }
}