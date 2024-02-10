namespace RedHerring.Clues;

public static class Definitions
{
    private static DefinitionSet _data = new();
    private static readonly Dictionary<Type, Definition> Defaults = new();

    #region Lifecycle
    
    public static void CreateContext(DefinitionSet set)
    {
        _data = set;
        PopulateDefaults();
    }

    public static void DestroyContext()
    {
        _data.Dispose();
    }

    #endregion Lifecycle

    #region Queries

    /// <summary>
    /// Returns all definitions of a given type.
    /// </summary>
    /// <typeparam name="T">Definition type</typeparam>
    /// <returns>Definition of type, if found, null otherwise</returns>
    public static IEnumerable<T> ByType<T>() where T : Definition
    {
        return _data.ByType<T>();
    }

    /// <summary>
    /// Returns a definition with the given id.
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T">Definition type</typeparam>
    /// <returns>Definition of type and id, if found, null otherwise</returns>
    public static T? ById<T>(Guid id) where T : Definition
    {
        return _data.ById<T>(id);
    }

    /// <summary>
    /// Returns true if definition exists and assigns it to `definition`.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="definition"></param>
    /// <typeparam name="T">Definition type</typeparam>
    /// <returns>True if definition exists</returns>
    public static bool TryById<T>(Guid id, out T? definition) where T : Definition
    {
        return (definition = _data.ById<T>(id)) is not null;
    }

    /// <summary>
    /// Returns true if definition exists.
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool Contains<T>(Guid id) where T : Definition
    {
        return _data.Contains<T>(id);
    }

    /// <summary>
    /// Returns the default definition for the type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? Default<T>() where T : Definition
    {
        Type type = typeof(T);
        return Defaults.TryGetValue(type, out var result) ? (T)result : null;
    }

    #endregion Queries

    #region Private

    private static void PopulateDefaults()
    {
        foreach (var entry in _data.All())
        {
            if (!entry.IsDefault)
            {
                continue;
            }

            var type = entry.GetType();
            Defaults[type] = entry;
        }
    }

    #endregion Private
}