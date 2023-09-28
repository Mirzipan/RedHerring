using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Extensions.Numerics;

public static partial class Vector4Extensions
{
    #region Deconstruction

    public static void Deconstruct(this Vector4 @this, out float x, out float y, out float z, out float w)
    {
        x = @this.X;
        y = @this.Y;
        z = @this.Z;
        w = @this.W;
    }

    #endregion Deconstruction
        
    #region Validation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNaN(this Vector4 @this)
    {
        return float.IsNaN(@this.X) || float.IsNaN(@this.Y) || float.IsNaN(@this.Z) || float.IsNaN(@this.W);
    }

    #endregion Validation

    #region With

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 WithX(this Vector4 @this, float x) => new(x, @this.Y, @this.Z, @this.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 WithY(this Vector4 @this, float y) => new(@this.X, y, @this.Z, @this.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 WithZ(this Vector4 @this, float z) => new(@this.X, @this.Y, z, @this.W);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 WithW(this Vector4 @this, float w) => new(@this.X, @this.Y, @this.Z, w);

    #endregion With

    #region Clamp

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Clamp(this Vector4 @this, Vector4 min, Vector4 max) => Vector4.Clamp(@this, min, max);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Clamp01(this Vector4 @this) => Clamp(@this, 0f, 1f);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Clamp(this Vector4 @this, float min, float max)
    {
        @this.X = Math.Clamp(@this.X, min, max);
        @this.Y = Math.Clamp(@this.Y, min, max);
        @this.Z = Math.Clamp(@this.Z, min, max);
        @this.W = Math.Clamp(@this.W, min, max);
        return @this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ClampX(this Vector4 @this, float min, float max)
    {
        return @this.WithX(Math.Clamp(@this.X, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ClampY(this Vector4 @this, float min, float max)
    {
        return @this.WithY(Math.Clamp(@this.Y, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ClampZ(this Vector4 @this, float min, float max)
    {
        return @this.WithZ(Math.Clamp(@this.Z, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ClampW(this Vector4 @this, float min, float max)
    {
        return @this.WithW(Math.Clamp(@this.W, min, max));
    }

    #endregion Clamp

    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Approximately(this Vector4 @this, Vector4 other, float tolerance = float.Epsilon)
    {
        var delta = @this - other;
        return delta.LengthSquared() <= tolerance * tolerance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(this Vector4 @this, float tolerance = float.Epsilon)
    {
        return @this.Approximately(Vector4.Zero, tolerance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotZero(this Vector4 @this, float tolerance = float.Epsilon)
    {
        return !@this.Approximately(Vector4.Zero, tolerance);
    }

    #endregion Equality
}