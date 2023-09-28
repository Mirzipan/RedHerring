using System.Runtime.CompilerServices;

namespace RedHerring.Extensions;

public static partial class IntExtensions
{
    /// <summary>
    /// Creates a <see cref="TimeSpan"/> with days equal to this.
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan Days(this int @this)
    {
        return TimeSpan.FromDays(@this);
    }

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> with hours equal to this.
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan Hours(this int @this)
    {
        return TimeSpan.FromHours(@this);
    }

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> with minutes equal to this.
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan Minutes(this int @this)
    {
        return TimeSpan.FromMinutes(@this);
    }

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> with seconds equal to this.
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan Seconds(this int @this)
    {
        return TimeSpan.FromSeconds(@this);
    }

    /// <summary>
    /// Creates a <see cref="TimeSpan"/> with milliseconds equal to this.
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan Milliseconds(this int @this)
    {
        return TimeSpan.FromMilliseconds(@this);
    }
}