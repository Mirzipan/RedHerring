using RedHerring.Alexandria.Identifiers;
using RedHerring.Core;
using RedHerring.Infusion.Attributes;

namespace RedHerring.Clues;

public sealed class DefinitionSystem : EngineSystem
{
    [Infuse]
    private DefinitionIndexer _indexer = null!;
    
    private readonly DefinitionSet _data = new();
    private readonly Dictionary<Type, Definition> _defaults = new();

    #region Lifecycle

    protected override void Init()
    {
    }

    protected override ValueTask<int> Load()
    {
        using var loader = new UnsortedDefinitionProcessor(_indexer);
        // TODO: get serialized definitions from somewhere, probably a JSON
        loader.Process(_data);

        PopulateDefaults();
        
        return ValueTask.FromResult(0);
    }

    protected override ValueTask<int> Unload()
    {
        return ValueTask.FromResult(0);
    }

    #endregion Lifecycle

    #region Queries

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
    public T? ById<T>(CompositeId id) where T : Definition
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
    public bool TryById<T>(CompositeId id, out T? definition) where T : Definition
    {
        return (definition = _data.ById<T>(id)) is not null;
    }

    /// <summary>
    /// Returns true if definition exists.
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool Contains<T>(CompositeId id) where T : Definition
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