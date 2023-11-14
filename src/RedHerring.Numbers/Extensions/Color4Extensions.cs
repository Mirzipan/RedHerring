using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static class Color4Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Premultiply(this Color4 @this) => Color4.Premultiply(@this);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Unmultiply(this Color4 @this) => Color4.Unmultiply(@this);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 ToLinearRGB(this Color4 @this) => Color4.ToLinearRGB(@this);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 ToSRGB(this Color4 @this) => Color4.ToSRGB(@this);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Clamp(this Color4 @this, Color4 min, Color4 max) => Color4.Clamp(@this, min, max);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Saturate(this Color4 @this) => Color4.Saturate(@this);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Negate(this Color4 @this) => Color4.Negate(@this);
}