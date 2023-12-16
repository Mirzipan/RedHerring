using RedHerring.Infusion.Exceptions;
using RedHerring.Infusion.Resolvers;

namespace RedHerring.Infusion;

internal class ResolverDescription
{
    public readonly Resolver Resolver;
    public readonly Type[] Contracts;

    private ResolverDescription(Resolver resolver, Type[] contracts)
    {
        Resolver = resolver;
        Contracts = contracts;
    }

    public static ResolverDescription Create(Resolver resolver, Type concrete, Type[] contracts)
    {
        for (int i = 0; i < contracts.Length; i++)
        {
            var contract = contracts[i];
            if (!contract.IsAssignableFrom(concrete))
            {
                throw new ContractNotAssignableException(concrete, contract);
            }
        }
        
        return new ResolverDescription(resolver, contracts);
    }
}