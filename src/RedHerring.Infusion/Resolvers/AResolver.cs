using RedHerring.Alexandria;

namespace RedHerring.Infusion.Resolvers;

public abstract class AResolver : ANamedDisposer
{
    public Type Contract { get; protected set; }
    
    public abstract object Resolve(InjectionContainer container);
}