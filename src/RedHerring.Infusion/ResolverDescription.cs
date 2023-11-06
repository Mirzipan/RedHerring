using RedHerring.Infusion.Resolvers;

namespace RedHerring.Infusion;

internal class ResolverDescription
{
    public readonly Resolver Resolver;
    public readonly Type[] Contracts;

    public ResolverDescription(Resolver resolver, Type[] contracts)
    {
        Resolver = resolver;
        Contracts = contracts;
    }
}