namespace RedHerring.Alexandria.Pooling;

public class GenericPool<T> : ObjectPool<T> where T : new()
{
    private readonly Stack<T> _instances;
    private readonly Func<T> _factory;
    private readonly Action<T> _onBorrow;
    private readonly Action<T> _onReturn;

    public int Count => _instances.Count;
    
    public GenericPool(Func<T> factory, Action<T> onBorrow, Action<T> onReturn, int capacity = 0)
    {
        _instances = new Stack<T>(capacity);

        _factory = factory;
        _onBorrow = onBorrow;
        _onReturn = onReturn;
    }

    public T Borrow()
    {
        T instance;
        
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

    public PooledObject<T> Borrow(out T instance)
    {
        instance = Borrow();
        return new PooledObject<T>(this, instance);
    }

    public void Return(T instance)
    {
        _onReturn(instance);
        _instances.Push(instance);
    }
}