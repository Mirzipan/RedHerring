using RedHerring.Infusion.Resolvers;

namespace RedHerring.Infusion;

internal class ResolverDescription
{
    public readonly AResolver Resolver;
    public readonly Type[] Contracts;

    public ResolverDescription(AResolver resolver, Type[] contracts)
    {
        Resolver = resolver;
        Contracts = contracts;
    }
}