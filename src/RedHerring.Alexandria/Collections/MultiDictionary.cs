namespace RedHerring.Alexandria.Collections;

public class MultiDictionary<TOuterKey, TInnerKey, TValue>: CollectionDictionary<TOuterKey, Dictionary<TInnerKey, TValue>, KeyValuePair<TInnerKey, TValue>>
{
    private readonly Stack<Dictionary<TInnerKey, TValue>> _pool;
        
    private readonly IEqualityComparer<TInnerKey> _comparer;
        
    public TValue this[TOuterKey outerKey, TInnerKey innerKey]
    {
        get => TryGet(outerKey, out var collection) ? collection[innerKey] : default;
        set => GetOrAdd(outerKey)[innerKey] = value;
    }

    #region Lifecycle

    public MultiDictionary() : this(0, null, null)
    {
    }

    public MultiDictionary(int capacity) : this(capacity, null, null)
    {
    }

    public MultiDictionary(IEqualityComparer<TOuterKey> outerKeyComparer) : this(0, outerKeyComparer, null)
    {
    }

    public MultiDictionary(IEqualityComparer<TInnerKey> innerKeyComparer) : this(0, null, innerKeyComparer)
    {
    }

    public MultiDictionary(int capacity, IEqualityComparer<TOuterKey> outerKeyComparer) : this(capacity, outerKeyComparer, null)
    {
    }

    public MultiDictionary(int capacity, IEqualityComparer<TInnerKey> innerKeyComparer) : this(capacity, null, innerKeyComparer)
    {
    }

    public MultiDictionary(int capacity, IEqualityComparer<TOuterKey> outerKeyComparer, IEqualityComparer<TInnerKey> innerKeyComparer) : base(capacity, outerKeyComparer)
    {
        _comparer = innerKeyComparer ?? EqualityComparer<TInnerKey>.Default;
        _pool = new Stack<Dictionary<TInnerKey, TValue>>();
    }

    #endregion Lifecycle

    #region Manipulation

    public void Add(TOuterKey outerKey, TInnerKey innerKey, TValue value)
    {
        GetOrAdd(outerKey).Add(innerKey, value);
    }

    public void Add(TOuterKey outerKey, (TInnerKey innerKey, TValue value) tuple)
    {
        GetOrAdd(outerKey).Add(tuple.innerKey, tuple.value);
    }

    public void Add(TOuterKey outerKey, params (TInnerKey innerKey, TValue value)[] tuples)
    {
        var collection = GetOrAdd(outerKey);
        for (int i = 0; i < tuples.Length; i++)
        {
            collection.Add(tuples[i].innerKey, tuples[i].value);
        }
    }

    public void Add(TOuterKey outerKey, IEnumerable<(TInnerKey innerKey, TValue value)> tuples)
    {
        var collection = GetOrAdd(outerKey);
        foreach (var entry in tuples)
        {
            collection.Add(entry.innerKey, entry.value);
        }
    }

    public bool Remove(TOuterKey outerKey, TInnerKey innerKey)
    {
        return TryGet(outerKey, out var collection) && collection.Remove(innerKey);
    }

    #endregion Manipulation
        
    #region Protected

    protected override Dictionary<TInnerKey, TValue> CreateCollection()
    {
        return _pool.TryPop(out var result) ? result : new Dictionary<TInnerKey, TValue>(_comparer);
    }

    protected override Dictionary<TInnerKey, TValue> CreateCollection(Dictionary<TInnerKey, TValue> source)
    {
        if (!_pool.TryPop(out var result))
        {
            return new Dictionary<TInnerKey, TValue>(source, _comparer);
        }

        foreach (var pair in source)
        {
            result.Add(pair.Key, pair.Value);
        }

        return result;
    }

    protected override void DestroyCollection(Dictionary<TInnerKey, TValue> collection)
    {
        collection.Clear();
        _pool.Push(collection);
    }

    #endregion Protected
}