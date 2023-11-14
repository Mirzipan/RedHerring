using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static partial class Vector4Extensions
{
    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNaN(this Vector4 @this)
    {
        return float.IsNaN(@this.X) || float.IsNaN(@this.Y) || float.IsNaN(@this.Z) || float.IsNaN(@this.W);
    }

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

    #region With

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 WithX(this Vector4 @this, float x) => @this with { X = x };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 WithY(this Vector4 @this, float y) => @this with { Y = y };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 WithZ(this Vector4 @this, float z) => @this with { Z = z };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 WithW(this Vector4 @this, float w) => @this with { W = w };

    #endregion With

    #region Clamp

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Saturate(this Vector4 @this) => Vector4.Clamp(@this, Vector4.Zero, Vector4.One);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Clamp(this Vector4 @this, float min, float max)
    {
        return new Vector4(
            float.Clamp(@this.X, min, max),
            float.Clamp(@this.Y, min, max),
            float.Clamp(@this.Z, min, max),
            float.Clamp(@this.W, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Clamp(this Vector4 @this, Vector4 min, Vector4 max)
    {
        return Vector4.Clamp(@this, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ClampX(this Vector4 @this, float min, float max)
    {
        return @this.WithX(float.Clamp(@this.X, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ClampY(this Vector4 @this, float min, float max)
    {
        return @this.WithY(float.Clamp(@this.Y, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ClampZ(this Vector4 @this, float min, float max)
    {
        return @this.WithZ(float.Clamp(@this.Z, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ClampW(this Vector4 @this, float min, float max)
    {
        return @this.WithW(float.Clamp(@this.W, min, max));
    }

    #endregion Clamp
    
    #region Manipulation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Add(this Vector4 @this, Vector4 other) => Vector4.Add(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Subtract(this Vector4 @this, Vector4 other) => Vector4.Subtract(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Multiply(this Vector4 @this, Vector4 other) => Vector4.Multiply(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Multiply(this Vector4 @this, float other) => Vector4.Multiply(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Divide(this Vector4 @this, Vector4 other) => Vector4.Divide(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Divide(this Vector4 @this, float divisor) => Vector4.Divide(@this, divisor);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Negate(this Vector4 @this) => Vector4.Negate(@this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Truncate(this Vector4 @this)
    {
        return new Vector4(
            MathF.Truncate(@this.X), 
            MathF.Truncate(@this.Y), 
            MathF.Truncate(@this.Z), 
            MathF.Truncate(@this.W));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Round(this Vector4 @this)
    {
        return new Vector4(
            MathF.Round(@this.X), 
            MathF.Round(@this.Y), 
            MathF.Round(@this.Z), 
            MathF.Round(@this.W));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Abs(this Vector4 @this) => Vector4.Abs(@this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Sign(this Vector4 @this)
    {
        return new Vector4(
            MathF.Sign(@this.X), 
            MathF.Sign(@this.Y), 
            MathF.Sign(@this.Z), 
            MathF.Sign(@this.W));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 SquareRoot(this Vector4 @this) => Vector4.SquareRoot(@this);

    #endregion Manipulation

    #region Distance

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceFrom(this Vector4 @this, Vector4 other) => Vector4.Distance(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquaredFrom(this Vector4 @this, Vector4 other)
    {
        return Vector4.DistanceSquared(@this, other);
    }

    #endregion Distance
}