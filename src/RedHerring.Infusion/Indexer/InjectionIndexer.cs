namespace RedHerring.Infusion.Indexer;

internal sealed class InjectionIndexer
{
    private static readonly Dictionary<Type, TypeDescription> Types = new();

    #region Queries

    public static void Index(Type type)
    {
        if (Types.ContainsKey(type))
        {
            return;
        }

        Types[type] = new TypeDescription(type);
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