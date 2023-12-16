using System.Reactive.Disposables;
using RedHerring.Alexandria.Disposables;

namespace RedHerring.Infusion.Resolvers;

internal sealed class TransientFactoryResolver : Resolver
{
    private readonly Func<InjectionContainer, object> _factory;
    private readonly CompositeDisposable _composite = new();
    
    public TransientFactoryResolver(Func<InjectionContainer, object> factory)
    {
        _factory = factory;
    }

    public object Resolve(InjectionContainer container)
    {
        object instance = _factory.Invoke(container);
        instance.TryDisposeWith(_composite);
        
        return instance;
    }

    public void Dispose()
    {
        _composite.Dispose();
    }
}