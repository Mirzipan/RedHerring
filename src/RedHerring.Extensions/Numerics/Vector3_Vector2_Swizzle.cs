using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Extensions.Numerics;

public static partial class Vector3Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 XX(this Vector3 @this) => new(@this.X, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 XY(this Vector3 @this) => new(@this.X, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 XZ(this Vector3 @this) => new(@this.X, @this.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 YX(this Vector3 @this) => new(@this.Y, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 YY(this Vector3 @this) => new(@this.Y, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 YZ(this Vector3 @this) => new(@this.Y, @this.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ZX(this Vector3 @this) => new(@this.Z, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ZY(this Vector3 @this) => new(@this.Z, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ZZ(this Vector3 @this) => new(@this.Z, @this.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void XY(ref this Vector3 @this, Vector2 other)
    {
        @this.X = other.X;
        @this.Y = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void XZ(ref this Vector3 @this, Vector2 other)
    {
        @this.X = other.X;
        @this.Z = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void YX(ref this Vector3 @this, Vector2 other)
    {
        @this.Y = other.X;
        @this.X = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void YZ(ref this Vector3 @this, Vector2 other)
    {
        @this.Y = other.X;
        @this.Z = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ZX(ref this Vector3 @this, Vector2 other)
    {
        @this.Z = other.X;
        @this.X = other.Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ZY(ref this Vector3 @this, Vector2 other)
    {
        @this.Z = other.X;
        @this.Y = other.Y;
    }
}