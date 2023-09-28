namespace RedHerring.Alexandria.Collections;

public class ListDictionary<TKey, TValue>: CollectionDictionary<TKey, List<TValue>, TValue>
{
    private readonly Stack<List<TValue>> _pool;
        
    #region Lifecycle

    public ListDictionary() : this(0, null)
    {
    }

    public ListDictionary(int capacity) : this(capacity, null)
    {
    }

    public ListDictionary(IEqualityComparer<TKey> keyComparer) : this(0, keyComparer)
    {
    }

    public ListDictionary(int capacity, IEqualityComparer<TKey> keyComparer) : base(capacity, keyComparer)
    {
        _pool = new Stack<List<TValue>>();
    }

    #endregion Lifecycle
        
    #region Protected

    protected override List<TValue> CreateCollection()
    {
        return _pool.TryPop(out var result) ? result : new List<TValue>();
    }

    protected override List<TValue> CreateCollection(List<TValue> source)
    {
        var collection = CreateCollection();
        collection.AddRange(source);
        return collection;
    }

    protected override void DestroyCollection(List<TValue> collection)
    {
        collection.Clear();
        _pool.Push(collection);
    }

    #endregion Protected
}