namespace RedHerring.Infusion.Resolvers;

public interface Resolver : IDisposable
{
    object Resolve(InjectionContainer container);
}