using RedHerring.Deduction;

namespace RedHerring.Clues;

[AttributeIndexer(typeof(DefinitionTypeAttribute))]
public sealed class DefinitionTypeIndexer : AttributeIndexer, IDisposable
{
    private Dictionary<Type, List<Type>> _indexedAs = new();
    
    public void Index(Attribute attribute, Type type)
    {
        if (attribute is DefinitionTypeAttribute definitionTypeAttribute)
        {
            GerOrCreateCollection(type).Add(definitionTypeAttribute.IndexedType);
        }
    }

    public List<Type>? IndexedAs(Type type)
    {
        return _indexedAs.TryGetValue(type, out var collection) ? new List<Type>(collection) : null;
    }

    public void IndexedAs(Type type, List<Type> result)
    {
        if (_indexedAs.TryGetValue(type, out var collection))
        {
            result.AddRange(collection);
        }
    }
    
    private List<Type> GerOrCreateCollection(Type key)
    {
        if (!_indexedAs.TryGetValue(key, out var collection))
        {
            collection = new List<Type>();
            _indexedAs[key] = collection;
        }

        return collection;
    }

    public void Dispose()
    {
        foreach (var pair in _indexedAs)
        {
            pair.Value.Clear();
        }
        
        _indexedAs.Clear();
    }
}