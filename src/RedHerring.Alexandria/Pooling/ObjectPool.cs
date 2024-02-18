namespace RedHerring.Alexandria.Pooling;

public interface ObjectPool<T>
{
    T Borrow();
    PooledObject<T> Borrow(out T instance);
    void Return(T instance);
}