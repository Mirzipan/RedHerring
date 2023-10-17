using System.Reflection;

namespace RedHerring.Infusion.Indexer;

public struct Parameter
{
    public readonly Type Type;
    public readonly string Name;

    public Parameter(ParameterInfo info)
    {
        Type = info.ParameterType;
        Name = info.Name;
    }
}