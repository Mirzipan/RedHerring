namespace RedHerring.Alexandria.Pooling;

public static class ListPool<TItem>
{
    internal static readonly CollectionPool<List<TItem>> Pool = new(() => new List<TItem>(), OnBorrow, OnReturn, 8);

    private static void OnBorrow(List<TItem> list)
    {
    }

    private static void OnReturn(List<TItem> list)
    {
        list.Clear();
    }

    public static List<TItem> Borrow() => Pool.Borrow();
    
    public static PooledObject<List<TItem>> Borrow(out List<TItem> instance) => Pool.Borrow(out instance);

    public static void Return(List<TItem> instance) => Pool.Return(instance);
}