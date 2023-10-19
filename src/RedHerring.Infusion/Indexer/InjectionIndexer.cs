namespace RedHerring.Infusion.Indexer;

internal sealed class InjectionIndexer
{
    private static readonly Dictionary<Type, TypeDescription> Types = new();

    #region Queries

    public static TypeDescription Index(Type type)
    {
        if (!Types.TryGetValue(type, out var description))
        {
            description = new TypeDescription(type);
            Types[type] = description;
        }

        return description;
    }

    public static bool TryGetInfo(Type type, out TypeDescription? info)
    {
        return Types.TryGetValue(type, out info);
    }

    #endregion Queries

    #region Manipulation

    public static void Clear()
    {
        Types.Clear();
    }

    #endregion Manipulation
}