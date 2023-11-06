using System.Reactive.Disposables;
using RedHerring.Alexandria.Collections;
using RedHerring.Infusion.Delegates;
using RedHerring.Infusion.Exceptions;
using RedHerring.Infusion.Resolvers;

namespace RedHerring.Infusion;

public sealed class ContainerDescription
{
    private string _name;
    private InjectionContainer? _parent;
    private List<ResolverDescription> _resolvers = new();

    public event ContainerBuilt ContainerBuilt = null!;

    #region Lifecycle

    public ContainerDescription(string name, InjectionContainer? parent = null)
    {
        _name = name;
        _parent = parent;
    }

    public InjectionContainer Build()
    {
        Build(out var disposable, out var resolvers);
        var result = new InjectionContainer(_name, resolvers, disposable);
        
        return result;
    }

    #endregion Lifecycle

    #region Manipulation

    public ContainerDescription AddSingleton(Type concrete) => AddSingleton(concrete, concrete);

    public ContainerDescription AddSingleton(Type concrete, params Type[] contracts)
    {
        return Add(concrete, contracts, () => new SingletonResolver(concrete));
    }

    public ContainerDescription AddTransient(Type concrete) => AddTransient(concrete, concrete);

    public ContainerDescription AddTransient(Type concrete, params Type[] contracts)
    {
        return Add(concrete, contracts, () => new TransientResolver(concrete));
    }

    public ContainerDescription AddInstance(object instance) => AddInstance(instance, instance.GetType());

    public ContainerDescription AddInstance(object instance, params Type[] contracts)
    {
        return Add(instance.GetType(), contracts, () => new InstanceResolver(instance));
    }

    #endregion Manipulation

    #region Queries
    
    public bool HasBinding(Type type)
    {
        return _resolvers.Any(e => e.Contracts.Contains(type));
    }

    #endregion Queries
    
    #region Private

    private void Build(out IDisposable disposable, out ListDictionary<Type, Resolver> resolvers)
    {
        resolvers = new ListDictionary<Type, Resolver>();
        var disposer = new CompositeDisposable();
        disposable = disposer;
        
        if (_parent is not null)
        {
            foreach (var pair in _parent.ResolversByContract)
            {
                resolvers[pair.Key] = pair.Value;
            }
        }

        foreach (var description in _resolvers)
        {
            disposer.Add(description.Resolver);
            
            foreach (var contract in description.Contracts)
            {
                resolvers.Add(contract, description.Resolver);
            }
        }
    }
    
    private ContainerDescription Add(Type concrete, Type[] contracts, Func<Resolver> resolverFactory)
    {
        ValidateContracts(concrete, contracts);
        var resolver = resolverFactory.Invoke();
        var resolverDescriptor = new ResolverDescription(resolver, contracts);
        _resolvers.Add(resolverDescriptor);
        return this;
    }

    private static void ValidateContracts(Type concrete, Type[] contracts)
    {
        for (int i = 0; i < contracts.Length; i++)
        {
            var contract = contracts[i];
            if (!contract.IsAssignableFrom(concrete))
            {
                throw new ContractNotAssignableException(concrete, contract);
            }
        }
    }

    #endregion Private
}