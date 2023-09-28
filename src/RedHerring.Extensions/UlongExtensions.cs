using System.Runtime.CompilerServices;

namespace RedHerring.Extensions;

public static class UlongExtensions
{
    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(this ulong @this) => @this == 0UL;
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotZero(this ulong @this) => @this != 0UL;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsMin(this ulong @this) => @this == ulong.MinValue;
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsMax(this ulong @this) => @this == ulong.MaxValue;
        
    #endregion Equality
}