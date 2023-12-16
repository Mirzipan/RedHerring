using System.Reactive.Disposables;
using RedHerring.Alexandria.Disposables;

namespace RedHerring.Infusion.Resolvers;

internal sealed class SingletonFactoryResolver : Resolver
{
    private object? _instance;

    private readonly Func<InjectionContainer, object> _factory;
    private readonly CompositeDisposable _composite = new();
    
    public SingletonFactoryResolver(Func<InjectionContainer, object> factory)
    {
        _factory = factory;
    }

    public object Resolve(InjectionContainer container)
    {
        if (_instance is null)
        {
            _instance = _factory.Invoke(container);
            _instance.TryDisposeWith(_composite);
        }

        return _instance;
    }

    public void Dispose()
    {
        _composite.Dispose();
    }
}