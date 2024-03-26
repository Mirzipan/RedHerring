using System.Collections;

namespace RedHerring.Alexandria.Pooling;

public class CollectionPool<TCollection> : ObjectPool<TCollection> where TCollection : ICollection
{
    private readonly Stack<TCollection> _instances;
    private readonly Func<TCollection> _factory;
    private readonly Action<TCollection> _onBorrow;
    private readonly Action<TCollection> _onReturn;
    
    public int Count => _instances.Count;
    
    public CollectionPool(Func<TCollection> factory, Action<TCollection> onBorrow, Action<TCollection> onReturn, int capacity = 0)
    {
        _instances = new Stack<TCollection>(capacity);

        _factory = factory;
        _onBorrow = onBorrow;
        _onReturn = onReturn;
    }
    
    public TCollection Borrow()
    {
        TCollection instance;
        
        if (_instances.Count > 0)
        {
            instance = _instances.Pop();
        }
        else
        {
            instance = _factory();
        }

        _onBorrow(instance);
        return instance;
    }

    public PooledObject<TCollection> Borrow(out TCollection instance)
    {
        instance = Borrow();
        return new PooledObject<TCollection>(this, instance);
    }

    public void Return(TCollection instance)
    {
        _onReturn(instance);
        _instances.Push(instance);
    }
}