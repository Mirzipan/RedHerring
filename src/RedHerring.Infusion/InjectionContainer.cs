﻿using RedHerring.Alexandria;
using RedHerring.Alexandria.Collections;
using RedHerring.Alexandria.Disposables;
using RedHerring.Infusion.Injectors;
using RedHerring.Infusion.Resolvers;

namespace RedHerring.Infusion;

public sealed class InjectionContainer : NamedDisposer
{
    internal InjectionContainer? Parent { get; private set; }
    internal List<InjectionContainer> Children { get; } = new();
    internal ListDictionary<Type, Resolver> ResolversByContract;

    #region Lifecycle

    internal InjectionContainer(string name, ListDictionary<Type, Resolver> resolvers, IDisposable disposable) : base(name)
    {
        ResolversByContract = resolvers;
        disposable.DisposeWith(this);
        
        InjectSelf();
    }

    protected override void Destroy()
    {
        ResolversByContract.Clear();
    }

    #endregion Lifecycle

    #region Creation

    public T Instantiate<T>()
    {
        return (T)Instantiate(typeof(T));
    }

    public object Instantiate(Type concrete)
    {
        object instance = ConstructorInjector.Construct(concrete, this);
        AttributeInjector.Inject(instance, this);
        return instance;
    }

    #endregion Creation

    #region Queries

    public bool HasBinding<T>() => HasBinding(typeof(T));
    public bool HasBinding(Type contract) => ResolversByContract.ContainsKey(contract);

    public TContract Resolve<TContract>() => (TContract) Resolve(typeof(TContract));

    public object Resolve(Type contract)
    {
        if (ResolversByContract.TryGet(contract, out var resolvers))
        {
            return resolvers.Last().Resolve(this);
        }

        return Instantiate(contract);
    }
    
    public IEnumerable<TContract> ResolveAll<TContract>()
    {
        return ResolversByContract.TryGet(typeof(TContract), out var resolvers)
            ? resolvers.Select(e => (TContract) e.Resolve(this))
            : Enumerable.Empty<TContract>();
    }
    
    public IEnumerable<object> ResolveAll(Type contract)
    {
        return ResolversByContract.TryGet(contract, out var resolvers)
            ? resolvers.Select(e => e.Resolve(this))
            : Enumerable.Empty<object>();
    }

    #endregion Queries

    #region Private

    private void InjectSelf()
    {
        ResolversByContract.Add(typeof(InjectionContainer), new SingletonInstanceResolver(this));
    }

    private IEnumerable<Resolver>? Resolvers(Type type)
    {
        return ResolversByContract.TryGet(type, out var result) ? result : null;
    }

    #endregion Private

    #region Internal

    private void SetParent(InjectionContainer? parent)
    {
        if (Parent is not null)
        {
            Parent.Children.Remove(this);
            Parent = null;
        }

        if (parent is not null)
        {
            Parent = parent;
            parent.Children.Add(this);
        }
    }

    #endregion Internal
}