using System.Runtime.CompilerServices;

namespace RedHerring.Alexandria.Extensions;

public static partial class IntExtensions
{
    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(this int @this) => @this == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotZero(this int @this) => @this != 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsMin(this int @this) => @this == int.MinValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsMax(this int @this) => @this == int.MaxValue;

    #endregion Equality

    #region Manipulation

    /// <summary>
    /// Clamps this between min and max values.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="min">Lower bound</param>
    /// <param name="max">Upped bound</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Clamp(this int @this, int min, int max) => Math.Clamp(@this, min, max);

    /// <summary>
    /// Returns -1 for negative numbers, 1 for positive numbers and 0 for zero.
    /// </summary>
    /// <param name="this"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sign(this int @this) => @this == 0 ? 0 : (@this > 0 ? 1 : -1);

    #endregion Manipulation
}