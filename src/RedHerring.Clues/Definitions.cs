using RedHerring.Infusion.Attributes;

namespace RedHerring.Clues;

public sealed class Definitions : IDisposable
{
    [Infuse]
    private DefinitionIndexer _indexer = null!;
    
    private readonly DefinitionSet _data = new();
    private readonly Dictionary<Type, Definition> _defaults = new();

    #region Lifecycle

    public void Dispose()
    {
        _indexer.Dispose();
        _data.Dispose();
    }

    #endregion Lifecycle

    #region Queries

    public DefinitionProcessor CreateProcessor()
    {
        return new UnsortedDefinitionProcessor(_indexer, Process);
    }

    /// <summary>
    /// Returns all definitions of a given type.
    /// </summary>
    /// <typeparam name="T">Definition type</typeparam>
    /// <returns>Definition of type, if found, null otherwise</returns>
    public IEnumerable<T> ByType<T>() where T : Definition
    {
        return _data.ByType<T>();
    }

    /// <summary>
    /// Returns a definition with the given id.
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T">Definition type</typeparam>
    /// <returns>Definition of type and id, if found, null otherwise</returns>
    public T? ById<T>(Guid id) where T : Definition
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
    public bool TryById<T>(Guid id, out T? definition) where T : Definition
    {
        return (definition = _data.ById<T>(id)) is not null;
    }

    /// <summary>
    /// Returns true if definition exists.
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool Contains<T>(Guid id) where T : Definition
    {
        return _data.Contains<T>(id);
    }

    /// <summary>
    /// Returns the default definition for the type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T? Default<T>() where T : Definition
    {
        Type type = typeof(T);
        return _defaults.TryGetValue(type, out var result) ? (T)result : null;
    }

    #endregion Queries

    #region Private

    private void Process(DefinitionProcessor processor)
    {
        processor.Process(_data);

        PopulateDefaults();
    }

    private void PopulateDefaults()
    {
        foreach (var entry in _data.All())
        {
            if (!entry.IsDefault)
            {
                continue;
            }

            var type = entry.GetType();
            _defaults[type] = entry;
        }
    }

    #endregion Private
}