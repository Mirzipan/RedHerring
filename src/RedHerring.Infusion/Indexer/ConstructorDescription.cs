using RedHerring.Infusion.Delegates;

namespace RedHerring.Infusion.Indexer;

internal sealed class ConstructorDescription
{
    public readonly ObjectActivator? Activator;
    public readonly Type[] Parameters;

    public ConstructorDescription(ObjectActivator? activator, Type[] parameters)
    {
        Activator = activator;
        Parameters = parameters;
    }
}