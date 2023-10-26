namespace RedHerring.Alexandria.Extensions.Collections;

public static class QueueExtensions
{
    #region Manipulation

    /// <summary>
    /// Returns the queue with item enqueued to it.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="item"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Queue<T> WithItem<T>(this Queue<T> @this, T item)
    {
        @this.Enqueue(item);
        return @this;
    }

    /// <summary>
    /// Returns the queue with items enqueued to it.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="items"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Queue<T> WithItems<T>(this Queue<T> @this, params T[] items)
    {
        @this.EnqueueRange(items);
        return @this;
    }

    /// <summary>
    /// Returns the queue with items enqueued to it.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="items"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Queue<T> WithItems<T>(this Queue<T> @this, IEnumerable<T> items)
    {
        @this.EnqueueRange(items);
        return @this;
    }

    /// <summary>
    /// Adds values from the source.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    public static void EnqueueRange<T>(this Queue<T> @this, IEnumerable<T>? source)
    {
        if (source is null)
        {
            return;
        }

        foreach (var entry in source)
        {
            @this.Enqueue(entry);
        }
    }

    #endregion Manipulation
}