using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static class Vector2Extensions
{
    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNan(this in Vector2 @this) => float.IsNaN(@this.X) || float.IsNaN(@this.Y);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Approximately(this in Vector2 @this, in Vector2 other, in float tolerance = float.Epsilon)
    {
        var delta = @this - other;
        return delta.LengthSquared() <= tolerance * tolerance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(this in Vector2 @this, in float tolerance = float.Epsilon)
    {
        return @this.Approximately(Vector2.Zero, tolerance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotZero(this in Vector2 @this, in float tolerance = float.Epsilon)
    {
        return !@this.Approximately(Vector2.Zero, tolerance);
    }

    #endregion Equality

    #region With

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 WithX(this in Vector2 @this, in float x) => @this with { X = x };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 WithY(this in Vector2 @this, in float y) => @this with { Y = y };

    #endregion With

    #region Clamp
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Saturate(this in Vector2 @this) => Vector2.Clamp(@this, Vector2.Zero, Vector2.One);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Clamp(this in Vector2 @this, in float min, in float max)
    {
        return new Vector2(float.Clamp(@this.X, min, max), float.Clamp(@this.Y, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Clamp(this in Vector2 @this, in Vector2 min, in Vector2 max)
    {
        return Vector2.Clamp(@this, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ClampX(this in Vector2 @this, in float min, in float max)
    {
        return @this.WithX(float.Clamp(@this.X, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ClampY(this in Vector2 @this, in float min, in float max)
    {
        return @this.WithY(float.Clamp(@this.Y, min, max));
    }

    #endregion Clamp

    #region Manipulation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Add(this in Vector2 @this, Vector2 other) => Vector2.Add(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Subtract(this in Vector2 @this, Vector2 other) => Vector2.Subtract(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Multiply(this in Vector2 @this, Vector2 other) => Vector2.Multiply(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Multiply(this in Vector2 @this, float other) => Vector2.Multiply(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Divide(this in Vector2 @this, Vector2 other) => Vector2.Divide(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Divide(this in Vector2 @this, float divisor) => Vector2.Divide(@this, divisor);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Negate(this in Vector2 @this) => Vector2.Negate(@this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Normalize(this in Vector2 @this) => Vector2.Normalize(@this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Truncate(this in Vector2 @this)
    {
        return new Vector2(MathF.Truncate(@this.X), MathF.Truncate(@this.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Round(this in Vector2 @this)
    {
        return new Vector2(MathF.Round(@this.X), MathF.Round(@this.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Abs(this in Vector2 @this) => Vector2.Abs(@this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Sign(this in Vector2 @this)
    {
        return new Vector2(MathF.Sign(@this.X), MathF.Sign(@this.Y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 SquareRoot(this in Vector2 @this) => Vector2.SquareRoot(@this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ReflectOff(this in Vector2 @this, Vector2 normal) => Vector2.Reflect(@this, normal);

    #endregion Manipulation

    #region Distance

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceFrom(this in Vector2 @this, Vector2 other) => Vector2.Distance(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquaredFrom(this in Vector2 @this, Vector2 other)
    {
        return Vector2.DistanceSquared(@this, other);
    }

    #endregion Distance

    #region Rotation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 RotateBy90CW(this in Vector2 @this) => new(@this.Y, -@this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 RotateBy90CCW(this in Vector2 @this) => new(-@this.Y, @this.X);

    #endregion Rotation
}