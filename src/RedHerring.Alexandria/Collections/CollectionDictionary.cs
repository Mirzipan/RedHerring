using System.Collections;

namespace RedHerring.Alexandria.Collections;

public abstract class CollectionDictionary<TKey, TCollection, TValue> : IDictionary<TKey, TCollection>
    where TCollection : class, ICollection<TValue>, new()
{
    private readonly Dictionary<TKey, TCollection> _data;

    ICollection<TKey> IDictionary<TKey, TCollection>.Keys => _data.Keys;
    ICollection<TCollection> IDictionary<TKey, TCollection>.Values => _data.Values;
    int ICollection<KeyValuePair<TKey, TCollection>>.Count => Count;
        
    public int Count => _data.Count;
    public bool IsReadOnly => false;
        
    public TCollection this[TKey key]
    {
        get => _data[key];
        set => _data[key] = GetOrAdd(key, value);
    }

    #region Lifecycle

    protected CollectionDictionary() : this(0, null)
    {
    }

    protected CollectionDictionary(int capacity) : this(capacity, null)
    {
    }
        
    protected CollectionDictionary(IEqualityComparer<TKey> comparer) : this(0, comparer)
    {
    }

    protected CollectionDictionary(int capacity, IEqualityComparer<TKey> comparer)
    {
        _data = new Dictionary<TKey, TCollection>(capacity, comparer);
    }

    protected CollectionDictionary(CollectionDictionary<TKey, TCollection, TValue> source) : this(source, null)
    {
    }

    protected CollectionDictionary(CollectionDictionary<TKey, TCollection, TValue> source, IEqualityComparer<TKey> comparer)
    {
        _data = new Dictionary<TKey,TCollection>(source._data, comparer);
    }

    #endregion Lifecycle
        
    #region Manipulation

    public void Add(TKey key, TValue value) => GetOrAdd(key).Add(value);

    public void Add(TKey key, params TValue[] values)
    {
        var collection = GetOrAdd(key);
        for (int i = 0; i < values.Length; i++)
        {
            collection.Add(values[i]);
        }
    }

    public void Add(TKey key, IEnumerable<TValue> values)
    {
        var collection = GetOrAdd(key);
        foreach (var entry in values)
        {
            collection.Add(entry);
        }
    }

    void IDictionary<TKey, TCollection>.Add(TKey key, TCollection value)
    {
        var collection = GetOrAdd(key);
        foreach (var entry in value)
        {
            collection.Add(entry);
        }
    }

    void ICollection<KeyValuePair<TKey, TCollection>>.Add(KeyValuePair<TKey, TCollection> item)
    {
        GetOrAdd(item.Key, item.Value);
    }

    public bool Remove(TKey key)
    {
        if (!_data.TryGetValue(key, out var collection))
        {
            return false;
        }

        _data.Remove(key);
        DestroyCollection(collection);
        return true;

    }

    public bool Remove(TKey key, TValue value)
    {
        if (!_data.TryGetValue(key, out var collection))
        {
            return false;
        }

        if (!collection.Remove(value))
        {
            return false;
        }

        if (collection.Count == 0)
        {
            _data.Remove(key);
            DestroyCollection(collection);
        }

        return true;
    }

    public bool Remove(KeyValuePair<TKey, TCollection> item)
    {
        return ((ICollection<KeyValuePair<TKey, TCollection>>)_data).Remove(item);
    }

    public void Clear()
    {
        foreach (var pair in _data)
        {
            DestroyCollection(pair.Value);
        }
            
        _data.Clear();
    }

    #endregion Manipulation

    #region Queries

    public TCollection GetOrAdd(TKey key)
    {
        if (_data.TryGetValue(key, out var collection))
        {
            return collection;
        }

        collection = CreateCollection();
        _data[key] = collection;
        return collection;
    }

    public bool ContainsKey(TKey key) => _data.ContainsKey(key);

    bool ICollection<KeyValuePair<TKey, TCollection>>.Contains(KeyValuePair<TKey, TCollection> item)
    {
        return ((ICollection<KeyValuePair<TKey, TCollection>>)_data).Contains(item);
    }
        
    public bool TryGet(TKey key, out TCollection collection) => _data.TryGetValue(key, out collection);

    bool IDictionary<TKey, TCollection>.TryGetValue(TKey key, out TCollection value) => _data.TryGetValue(key, out value);

    public void CopyTo(KeyValuePair<TKey, TCollection>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<TKey, TCollection>>)_data).CopyTo(array, arrayIndex);
    }

    public Dictionary<TKey, TCollection>.Enumerator GetEnumerator() => _data.GetEnumerator();

    IEnumerator<KeyValuePair<TKey, TCollection>> IEnumerable<KeyValuePair<TKey, TCollection>>.GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion Queries

    #region Protected

    protected abstract TCollection CreateCollection();

    protected abstract TCollection CreateCollection(TCollection source);

    protected abstract void DestroyCollection(TCollection collection);

    protected TCollection GetOrAdd(TKey key, TCollection values)
    {
        if (_data.TryGetValue(key, out var collection))
        {
            return collection;
        }

        collection = CreateCollection(values);
        _data[key] = collection;
        return collection;
    }

    #endregion Protected
}