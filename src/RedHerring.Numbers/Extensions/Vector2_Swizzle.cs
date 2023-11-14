using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static partial class Vector2Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 XX(this Vector2 @this) => new(@this.X, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 YY(this Vector2 @this) => new(@this.Y, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 XY(this Vector2 @this) => new(@this.X, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 YX(this Vector2 @this) => new(@this.Y, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void XY(ref this Vector2 @this, Vector2 other)
    {
        @this.X = other.X;
        @this.Y = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void YX(ref this Vector2 @this, Vector2 other)
    {
        @this.Y = other.X;
        @this.X = other.Y;
    }
}