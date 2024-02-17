using RedHerring.Alexandria.Disposables;

namespace RedHerring.Clues;

public static class Definitions
{
    private static DefinitionsContext? _context;

    #region Lifecycle
    
    public static DefinitionsContext CreateContext(DefinitionSet set)
    {
        var previous = CurrentContext();
        var context = new DefinitionsContext(set);
        CurrentContext(previous ?? context);
        
        return context;
    }

    public static void DestroyContext(DefinitionsContext? context = null)
    {
        var previous = CurrentContext();
        if (context is null)
        {
            context = previous;
        }

        CurrentContext(context != previous ? previous : null);
        context.TryDispose();
    }

    public static DefinitionsContext? CurrentContext() => _context;

    public static void CurrentContext(DefinitionsContext? context) => _context = context;

    #endregion Lifecycle

    #region Queries

    /// <summary>
    /// Returns all definitions of a given type.
    /// </summary>
    /// <typeparam name="T">Definition type</typeparam>
    /// <returns>Definition of type, if found, null otherwise</returns>
    public static IEnumerable<T> ByType<T>() where T : Definition
    {
        return _context is not null ? _context.Data.ByType<T>() : Enumerable.Empty<T>();
    }

    /// <summary>
    /// Returns a definition with the given id.
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T">Definition type</typeparam>
    /// <returns>Definition of type and id, if found, null otherwise</returns>
    public static T? ById<T>(DefinitionId id) where T : Definition
    {
        return _context?.Data.ById<T>(id);
    }

    /// <summary>
    /// Returns true if definition exists and assigns it to `definition`.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="definition"></param>
    /// <typeparam name="T">Definition type</typeparam>
    /// <returns>True if definition exists</returns>
    public static bool TryById<T>(DefinitionId id, out T? definition) where T : Definition
    {
        if (_context is null)
        {
            definition = null;
            return false;
        }
        
        return (definition = _context.Data.ById<T>(id)) is not null;
    }

    /// <summary>
    /// Returns true if definition exists.
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool Contains<T>(DefinitionId id) where T : Definition
    {
        return _context?.Data.Contains<T>(id) ?? false;
    }

    /// <summary>
    /// Returns the default definition for the type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? Default<T>() where T : Definition
    {
        Type type = typeof(T);
        if (_context is null)
        {
            return null;
        }
        
        return _context.Defaults.TryGetValue(type, out var result) ? (T)result : null;
    }

    #endregion Queries
}