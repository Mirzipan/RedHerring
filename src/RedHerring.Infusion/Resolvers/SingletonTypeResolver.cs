using System.Reactive.Disposables;
using RedHerring.Alexandria.Disposables;

namespace RedHerring.Infusion.Resolvers;

internal sealed class SingletonTypeResolver : Resolver
{
    private object? _instance;

    private readonly Type _contract;
    private readonly CompositeDisposable _composite = new();
    
    public SingletonTypeResolver(Type contract)
    {
        _contract = contract;
    }

    public object Resolve(InjectionContainer container)
    {
        if (_instance is null)
        {
            _instance = container.Instantiate(_contract);
            _instance.TryDisposeWith(_composite);
        }

        return _instance;
    }

    public void Dispose()
    {
        _composite.Dispose();
    }
}