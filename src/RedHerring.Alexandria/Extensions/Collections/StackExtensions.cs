namespace RedHerring.Alexandria.Extensions.Collections;

public static class StackExtensions
{
    #region Manipulation

    /// <summary>
    /// Returns the stack with item pushed to it.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="item"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Stack<T> WithItem<T>(this Stack<T> @this, T item)
    {
        @this.Push(item);
        return @this;
    }

    /// <summary>
    /// Returns the stack with items pushed to it.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="items"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Stack<T> WithItems<T>(this Stack<T> @this, params T[] items)
    {
        @this.PushRange(items);
        return @this;
    }

    /// <summary>
    /// Returns the stack with items pushed to it.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="items"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Stack<T> WithItems<T>(this Stack<T> @this, IEnumerable<T> items)
    {
        @this.PushRange(items);
        return @this;
    }
    
    /// <summary>
    /// Pushes values from the source.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    public static void PushRange<T>(this Stack<T> @this, IEnumerable<T> source)
    {
        if (source == null)
        {
            return;
        }

        foreach (var entry in source)
        {
            @this.Push(entry);
        }
    }

    #endregion Manipulation
}