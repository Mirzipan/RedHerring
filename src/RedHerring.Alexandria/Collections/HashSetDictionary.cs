using RedHerring.Extensions.Collections;

namespace RedHerring.Alexandria.Collections;

public class HashSetDictionary<TKey, TValue>: CollectionDictionary<TKey, HashSet<TValue>, TValue>
{
    private readonly Stack<HashSet<TValue>> _pool;
        
    private readonly IEqualityComparer<TValue> _comparer;

    #region Lifecycle

    public HashSetDictionary() : this(0, null, null)
    {
    }

    public HashSetDictionary(int capacity) : this(capacity, null, null)
    {
    }

    public HashSetDictionary(IEqualityComparer<TKey> keyComparer) : this(0, keyComparer, null)
    {
    }

    public HashSetDictionary(IEqualityComparer<TValue> valueComparer) : this(0, null, valueComparer)
    {
    }

    public HashSetDictionary(int capacity, IEqualityComparer<TKey> keyComparer) : this(capacity, keyComparer, null)
    {
    }

    public HashSetDictionary(int capacity, IEqualityComparer<TValue> valueComparer) : this(capacity, null, valueComparer)
    {
    }

    public HashSetDictionary(int capacity, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) : base(capacity, keyComparer)
    {
        _comparer = valueComparer ?? EqualityComparer<TValue>.Default;
        _pool = new Stack<HashSet<TValue>>();
    }

    #endregion Lifecycle

    #region Protected

    protected override HashSet<TValue> CreateCollection()
    {
        return _pool.TryPop(out var result) ? result : new HashSet<TValue>(_comparer);
    }

    protected override HashSet<TValue> CreateCollection(HashSet<TValue> source)
    {
        var collection = CreateCollection();
        collection.AddRange(source);
        return collection;
    }

    protected override void DestroyCollection(HashSet<TValue> collection)
    {
        collection.Clear();
        _pool.Push(collection);
    }

    #endregion Protected
}