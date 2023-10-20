using System.Runtime.CompilerServices;

namespace RedHerring.Alexandria.Extensions;

public static class UintExtensions
{
    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(this uint @this) => @this == 0U;
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotZero(this uint @this) => @this != 0U;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsMin(this uint @this) => @this == uint.MinValue;
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsMax(this uint @this) => @this == uint.MaxValue;
        
    #endregion Equality
}