using System.Reactive.Disposables;
using RedHerring.Alexandria.Disposables;

namespace RedHerring.Infusion.Resolvers;

internal sealed class SingletonInstanceResolver : Resolver
{
    private readonly object _instance;
    private readonly CompositeDisposable _composite = new();

    public SingletonInstanceResolver(object instance)
    {
        _instance = instance;
        _instance.TryDisposeWith(_composite);
    }

    public object Resolve(InjectionContainer container)
    {
        return _instance;
    }

    public void Dispose()
    {
        _composite.Dispose();
    }
}