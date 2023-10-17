using RedHerring.Infusion.Delegates;

namespace RedHerring.Infusion.Indexer;

internal sealed class MethodDescription
{
    public readonly string Name;
    public readonly InjectMethod Action;
    public readonly Type[] Parameters;

    public MethodDescription(string name, InjectMethod action, Type[] parameters)
    {
        Name = name;
        Action = action;
        Parameters = parameters;
    }
}