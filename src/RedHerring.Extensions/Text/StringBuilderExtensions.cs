using System.Runtime.CompilerServices;
using System.Text;

namespace RedHerring.Extensions.Text;

public static class StringBuilderExtensions
{
    #region Append

    public static StringBuilder AppendIf<T>(this StringBuilder @this, Predicate<T> predicate, T value)
    {
        if (predicate(value))
        {
            @this.Append(value);
        }
            
        return @this;
    }

    public static StringBuilder AppendIf<T>(this StringBuilder @this, Predicate<T> predicate, IEnumerable<T> values)
    {
        foreach (var entry in values)
        {
            if (!predicate(entry))
            {
                continue;
            }

            @this.Append(entry);
        }

        return @this;
    }

    public static StringBuilder AppendIf<T>(this StringBuilder @this, Predicate<T> predicate, params T[] values)
    {
        foreach (var entry in values)
        {
            if (!predicate(entry))
            {
                continue;
            }

            @this.Append(entry);
        }

        return @this;
    }

    #endregion Append
        
    #region Prepend

    [MethodImpl(MethodImplOptions.AggressiveInlining)]        
    public static StringBuilder Prepend(this StringBuilder @this, object value) => @this.Insert(0, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]        
    public static StringBuilder Prepend(this StringBuilder @this, string value) => @this.Insert(0, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]        
    public static StringBuilder Prepend(this StringBuilder @this, char value) => @this.Insert(0, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]        
    public static StringBuilder Prepend(this StringBuilder @this, byte value) => @this.Insert(0, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]        
    public static StringBuilder Prepend(this StringBuilder @this, short value) => @this.Insert(0, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]        
    public static StringBuilder Prepend(this StringBuilder @this, int value) => @this.Insert(0, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]        
    public static StringBuilder Prepend(this StringBuilder @this, long value) => @this.Insert(0, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]        
    public static StringBuilder Prepend(this StringBuilder @this, sbyte value) => @this.Insert(0, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]        
    public static StringBuilder Prepend(this StringBuilder @this, ushort value) => @this.Insert(0, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]        
    public static StringBuilder Prepend(this StringBuilder @this, uint value) => @this.Insert(0, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]        
    public static StringBuilder Prepend(this StringBuilder @this, ulong value) => @this.Insert(0, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]        
    public static StringBuilder Prepend(this StringBuilder @this, float value) => @this.Insert(0, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]        
    public static StringBuilder Prepend(this StringBuilder @this, double value) => @this.Insert(0, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]        
    public static StringBuilder Prepend(this StringBuilder @this, decimal value) => @this.Insert(0, value);

    #endregion Prepend
}