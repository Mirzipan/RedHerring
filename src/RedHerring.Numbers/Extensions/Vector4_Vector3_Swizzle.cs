using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static partial class Vector4Extensions
{
    #region X

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XXX(this Vector4 @this) => new(@this.X, @this.X, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XXY(this Vector4 @this) => new(@this.X, @this.X, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XXZ(this Vector4 @this) => new(@this.X, @this.X, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XXW(this Vector4 @this) => new(@this.X, @this.X, @this.W);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XYX(this Vector4 @this) => new(@this.X, @this.Y, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XYY(this Vector4 @this) => new(@this.X, @this.Y, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XYZ(this Vector4 @this) => new(@this.X, @this.Y, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XYW(this Vector4 @this) => new(@this.X, @this.Y, @this.W);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XZX(this Vector4 @this) => new(@this.X, @this.Z, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XZY(this Vector4 @this) => new(@this.X, @this.Z, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XZZ(this Vector4 @this) => new(@this.X, @this.Z, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XZW(this Vector4 @this) => new(@this.X, @this.Z, @this.W);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XWX(this Vector4 @this) => new(@this.X, @this.W, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XWY(this Vector4 @this) => new(@this.X, @this.W, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XWZ(this Vector4 @this) => new(@this.X, @this.W, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XWW(this Vector4 @this) => new(@this.X, @this.W, @this.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]    
    public static void XYZ(ref this Vector4 @this, Vector3 other)
    {
        @this.X = other.X;
        @this.Y = other.Y;
        @this.Z = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void XYW(ref this Vector4 @this, Vector3 other)
    {
        @this.X = other.X;
        @this.Y = other.Y;
        @this.W = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void XZY(ref this Vector4 @this, Vector3 other)
    {
        @this.X = other.X;
        @this.Z = other.Y;
        @this.Y = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void XZW(ref this Vector4 @this, Vector3 other)
    {
        @this.X = other.X;
        @this.Z = other.Y;
        @this.W = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void XWY(ref this Vector4 @this, Vector3 other)
    {
        @this.X = other.X;
        @this.W = other.Y;
        @this.Y = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void XWZ(ref this Vector4 @this, Vector3 other)
    {
        @this.X = other.X;
        @this.W = other.Y;
        @this.Z = other.Z;
    }

    #endregion X

    #region Y


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YXX(this Vector4 @this) => new(@this.Y, @this.X, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YXY(this Vector4 @this) => new(@this.Y, @this.X, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YXZ(this Vector4 @this) => new(@this.Y, @this.X, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YXW(this Vector4 @this) => new(@this.Y, @this.X, @this.W);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YYX(this Vector4 @this) => new(@this.Y, @this.Y, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YYY(this Vector4 @this) => new(@this.Y, @this.Y, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YYZ(this Vector4 @this) => new(@this.Y, @this.Y, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YYW(this Vector4 @this) => new(@this.Y, @this.Y, @this.W);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YZX(this Vector4 @this) => new(@this.Y, @this.Z, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YZY(this Vector4 @this) => new(@this.Y, @this.Z, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YZZ(this Vector4 @this) => new(@this.Y, @this.Z, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YZW(this Vector4 @this) => new(@this.Y, @this.Z, @this.W);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YWX(this Vector4 @this) => new(@this.Y, @this.W, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YWY(this Vector4 @this) => new(@this.Y, @this.W, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YWZ(this Vector4 @this) => new(@this.Y, @this.W, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 YWW(this Vector4 @this) => new(@this.Y, @this.W, @this.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]    
    public static void YXZ(ref this Vector4 @this, Vector3 other)
    {
        @this.Y = other.X;
        @this.X = other.Y;
        @this.Z = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void YXW(ref this Vector4 @this, Vector3 other)
    {
        @this.Y = other.X;
        @this.X = other.Y;
        @this.W = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void YZX(ref this Vector4 @this, Vector3 other)
    {
        @this.Y = other.X;
        @this.Z = other.Y;
        @this.X = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void YZW(ref this Vector4 @this, Vector3 other)
    {
        @this.Y = other.X;
        @this.Z = other.Y;
        @this.W = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void YWX(ref this Vector4 @this, Vector3 other)
    {
        @this.Y = other.X;
        @this.W = other.Y;
        @this.X = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void YWZ(ref this Vector4 @this, Vector3 other)
    {
        @this.Y = other.X;
        @this.W = other.Y;
        @this.Z = other.Z;
    }

    #endregion Y

    #region Z

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZXX(this Vector4 @this) => new(@this.Z, @this.X, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZXY(this Vector4 @this) => new(@this.Z, @this.X, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZXZ(this Vector4 @this) => new(@this.Z, @this.X, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZXW(this Vector4 @this) => new(@this.Z, @this.X, @this.W);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZYX(this Vector4 @this) => new(@this.Z, @this.Y, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZYY(this Vector4 @this) => new(@this.Z, @this.Y, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZYZ(this Vector4 @this) => new(@this.Z, @this.Y, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZYW(this Vector4 @this) => new(@this.Z, @this.Y, @this.W);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZZX(this Vector4 @this) => new(@this.Z, @this.Z, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZZY(this Vector4 @this) => new(@this.Z, @this.Z, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZZZ(this Vector4 @this) => new(@this.Z, @this.Z, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZZW(this Vector4 @this) => new(@this.Z, @this.Z, @this.W);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZWX(this Vector4 @this) => new(@this.Z, @this.W, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZWY(this Vector4 @this) => new(@this.Z, @this.W, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZWZ(this Vector4 @this) => new(@this.Z, @this.W, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ZWW(this Vector4 @this) => new(@this.Z, @this.W, @this.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]    
    public static void ZXY(ref this Vector4 @this, Vector3 other)
    {
        @this.Z = other.X;
        @this.X = other.Y;
        @this.Y = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ZXW(ref this Vector4 @this, Vector3 other)
    {
        @this.Z = other.X;
        @this.X = other.Y;
        @this.W = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ZYX(ref this Vector4 @this, Vector3 other)
    {
        @this.Z = other.X;
        @this.Y = other.Y;
        @this.X = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ZYW(ref this Vector4 @this, Vector3 other)
    {
        @this.Z = other.X;
        @this.Y = other.Y;
        @this.W = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ZWX(ref this Vector4 @this, Vector3 other)
    {
        @this.Z = other.X;
        @this.W = other.Y;
        @this.X = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ZWY(ref this Vector4 @this, Vector3 other)
    {
        @this.Z = other.X;
        @this.W = other.Y;
        @this.Y = other.Z;
    }

    #endregion Z

    #region W


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WXX(this Vector4 @this) => new(@this.W, @this.X, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WXY(this Vector4 @this) => new(@this.W, @this.X, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WXZ(this Vector4 @this) => new(@this.W, @this.X, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WXW(this Vector4 @this) => new(@this.W, @this.X, @this.W);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WYX(this Vector4 @this) => new(@this.W, @this.Y, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WYY(this Vector4 @this) => new(@this.W, @this.Y, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WYZ(this Vector4 @this) => new(@this.W, @this.Y, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WYW(this Vector4 @this) => new(@this.W, @this.Y, @this.W);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WZX(this Vector4 @this) => new(@this.W, @this.Z, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WZY(this Vector4 @this) => new(@this.W, @this.Z, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WZZ(this Vector4 @this) => new(@this.W, @this.Z, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WZW(this Vector4 @this) => new(@this.W, @this.Z, @this.W);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WWX(this Vector4 @this) => new(@this.W, @this.W, @this.X);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WWY(this Vector4 @this) => new(@this.W, @this.W, @this.Y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WWZ(this Vector4 @this) => new(@this.W, @this.W, @this.Z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WWW(this Vector4 @this) => new(@this.W, @this.W, @this.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]    
    public static void WXY(ref this Vector4 @this, Vector3 other)
    {
        @this.W = other.X;
        @this.X = other.Y;
        @this.Y = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WXZ(ref this Vector4 @this, Vector3 other)
    {
        @this.W = other.X;
        @this.X = other.Y;
        @this.Z = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WYX(ref this Vector4 @this, Vector3 other)
    {
        @this.W = other.X;
        @this.Y = other.Y;
        @this.X = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WYZ(ref this Vector4 @this, Vector3 other)
    {
        @this.W = other.X;
        @this.Y = other.Y;
        @this.Z = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WZX(ref this Vector4 @this, Vector3 other)
    {
        @this.W = other.X;
        @this.Z = other.Y;
        @this.X = other.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WZY(ref this Vector4 @this, Vector3 other)
    {
        @this.W = other.X;
        @this.Z = other.Y;
        @this.Y = other.Z;
    }

    #endregion W
}