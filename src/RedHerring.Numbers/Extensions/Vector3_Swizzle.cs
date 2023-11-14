using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static partial class Vector3Extensions
{
    #region X

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XXX(this Vector3 @this) => new(@this.X, @this.X, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XXY(this Vector3 @this) => new(@this.X, @this.X, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XXZ(this Vector3 @this) => new(@this.X, @this.X, @this.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XYX(this Vector3 @this) => new(@this.X, @this.Y, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XYY(this Vector3 @this) => new(@this.X, @this.Y, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XYZ(this Vector3 @this) => new(@this.X, @this.Y, @this.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XZX(this Vector3 @this) => new(@this.X, @this.Z, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XZY(this Vector3 @this) => new(@this.X, @this.Z, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XZZ(this Vector3 @this) => new(@this.X, @this.Z, @this.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void XYZ(ref this Vector3 @this, Vector3 other)
    {
        @this.X = other.X;
        @this.Y = other.Y;
        @this.Z = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void XZY(ref this Vector3 @this, Vector3 other)
    {
        @this.X = other.X;
        @this.Z = other.Y;
        @this.Y = other.Z;
    }

    #endregion X

    #region Y

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YXX(this Vector3 @this) => new(@this.Y, @this.X, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YXY(this Vector3 @this) => new(@this.Y, @this.X, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YXZ(this Vector3 @this) => new(@this.Y, @this.X, @this.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YYX(this Vector3 @this) => new(@this.Y, @this.Y, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YYY(this Vector3 @this) => new(@this.Y, @this.Y, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YYZ(this Vector3 @this) => new(@this.Y, @this.Y, @this.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YZX(this Vector3 @this) => new(@this.Y, @this.Z, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YZY(this Vector3 @this) => new(@this.Y, @this.Z, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YZZ(this Vector3 @this) => new(@this.Y, @this.Z, @this.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void YXZ(ref this Vector3 @this, Vector3 other)
    {
        @this.Y = other.X;
        @this.X = other.Y;
        @this.Z = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void YZX(ref this Vector3 @this, Vector3 other)
    {
        @this.Y = other.X;
        @this.Z = other.Y;
        @this.X = other.Z;
    }

    #endregion Y

    #region Z

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZXX(this Vector3 @this) => new(@this.Z, @this.X, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZXY(this Vector3 @this) => new(@this.Z, @this.X, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZXZ(this Vector3 @this) => new(@this.Z, @this.X, @this.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZYX(this Vector3 @this) => new(@this.Z, @this.Y, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZYY(this Vector3 @this) => new(@this.Z, @this.Y, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZYZ(this Vector3 @this) => new(@this.Z, @this.Y, @this.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZZX(this Vector3 @this) => new(@this.Z, @this.Z, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZZY(this Vector3 @this) => new(@this.Z, @this.Z, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZZZ(this Vector3 @this) => new(@this.Z, @this.Z, @this.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ZXY(ref this Vector3 @this, Vector3 other)
    {
        @this.Z = other.X;
        @this.X = other.Y;
        @this.Y = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ZYX(ref this Vector3 @this, Vector3 other)
    {
        @this.Z = other.X;
        @this.Y = other.Y;
        @this.X = other.Z;
    }

    #endregion Z
}