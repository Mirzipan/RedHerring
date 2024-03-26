namespace RedHerring.Alexandria.Pooling;

public class PooledObject<T>(ObjectPool<T> pool, T instance) : IDisposable
{
    private readonly Action _onDispose = () =>
    {
        pool.Return(instance);
    };

    public void Dispose() => _onDispose.Invoke();
}