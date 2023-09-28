using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Extensions.Numerics;

public static partial class Vector4Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 XX(this Vector4 @this) => new(@this.X, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 XY(this Vector4 @this) => new(@this.X, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 XZ(this Vector4 @this) => new(@this.X, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 XW(this Vector4 @this) => new(@this.X, @this.W);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 YX(this Vector4 @this) => new(@this.Y, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 YY(this Vector4 @this) => new(@this.Y, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 YZ(this Vector4 @this) => new(@this.Y, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 YW(this Vector4 @this) => new(@this.Y, @this.W);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ZX(this Vector4 @this) => new(@this.Z, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ZY(this Vector4 @this) => new(@this.Z, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ZZ(this Vector4 @this) => new(@this.Z, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ZW(this Vector4 @this) => new(@this.Z, @this.W);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 WX(this Vector4 @this) => new(@this.W, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 WY(this Vector4 @this) => new(@this.W, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 WZ(this Vector4 @this) => new(@this.W, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 WW(this Vector4 @this) => new(@this.W, @this.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void XY(ref this Vector4 @this, Vector2 other)
    {
        @this.X = other.X;
        @this.Y = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void XZ(ref this Vector4 @this, Vector2 other)
    {
        @this.X = other.X;
        @this.Z = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void XW(ref this Vector4 @this, Vector2 other)
    {
        @this.X = other.X;
        @this.W = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void YX(ref this Vector4 @this, Vector2 other)
    {
        @this.Y = other.X;
        @this.X = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void YZ(ref this Vector4 @this, Vector2 other)
    {
        @this.Y = other.X;
        @this.Z = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void YW(ref this Vector4 @this, Vector2 other)
    {
        @this.Y = other.X;
        @this.W = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ZX(ref this Vector4 @this, Vector2 other)
    {
        @this.Z = other.X;
        @this.X = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ZY(ref this Vector4 @this, Vector2 other)
    {
        @this.Z = other.X;
        @this.Y = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ZW(ref this Vector4 @this, Vector2 other)
    {
        @this.Z = other.X;
        @this.W = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WX(ref this Vector4 @this, Vector2 other)
    {
        @this.W = other.X;
        @this.X = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WY(ref this Vector4 @this, Vector2 other)
    {
        @this.W = other.X;
        @this.Y = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WZ(ref this Vector4 @this, Vector2 other)
    {
        @this.W = other.X;
        @this.Z = other.Y;
    }
}