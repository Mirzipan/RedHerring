using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static class Vector3Extensions
{
    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNaN(this Vector3 @this)
    {
        return float.IsNaN(@this.X) || float.IsNaN(@this.Y) || float.IsNaN(@this.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Approximately(this Vector3 @this, Vector3 other, float tolerance = float.Epsilon)
    {
        var delta = @this - other;
        return delta.LengthSquared() <= tolerance * tolerance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(this Vector3 @this, float tolerance = float.Epsilon)
    {
        return @this.Approximately(Vector3.Zero, tolerance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotZero(this Vector3 @this, float tolerance = float.Epsilon)
    {
        return !@this.Approximately(Vector3.Zero, tolerance);
    }

    #endregion Equality

    #region With

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WithX(this Vector3 @this, float x) => @this with { X = x };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WithY(this Vector3 @this, float y) => @this with { Y = y };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WithZ(this Vector3 @this, float z) => @this with { Z = z };

    #endregion With
    
    #region Clamp
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Saturate(this Vector3 @this) => Vector3.Clamp(@this, Vector3.Zero, Vector3.One);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Clamp(this Vector3 @this, float min, float max)
    {
        return new Vector3(
            float.Clamp(@this.X, min, max),
            float.Clamp(@this.Y, min, max),
            float.Clamp(@this.Z, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Clamp(this Vector3 @this, Vector3 min, Vector3 max)
    {
        return Vector3.Clamp(@this, min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ClampX(this Vector3 @this, float min, float max)
    {
        return @this.WithX(float.Clamp(@this.X, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ClampY(this Vector3 @this, float min, float max)
    {
        return @this.WithY(float.Clamp(@this.Y, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ClampZ(this Vector3 @this, float min, float max)
    {
        return @this.WithZ(float.Clamp(@this.Z, min, max));
    }

    #endregion Clamp
    
    #region Manipulation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Add(this Vector3 @this, Vector3 other) => Vector3.Add(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Subtract(this Vector3 @this, Vector3 other) => Vector3.Subtract(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Multiply(this Vector3 @this, Vector3 other) => Vector3.Multiply(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Multiply(this Vector3 @this, float other) => Vector3.Multiply(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Divide(this Vector3 @this, Vector3 other) => Vector3.Divide(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Divide(this Vector3 @this, float divisor) => Vector3.Divide(@this, divisor);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Negate(this Vector3 @this) => Vector3.Negate(@this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Truncate(this Vector3 @this)
    {
        return new Vector3(MathF.Truncate(@this.X), MathF.Truncate(@this.Y), MathF.Truncate(@this.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Round(this Vector3 @this)
    {
        return new Vector3(MathF.Round(@this.X), MathF.Round(@this.Y), MathF.Round(@this.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Abs(this Vector3 @this) => Vector3.Abs(@this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Sign(this Vector3 @this)
    {
        return new Vector3(MathF.Sign(@this.X), MathF.Sign(@this.Y), MathF.Sign(@this.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 SquareRoot(this Vector3 @this) => Vector3.SquareRoot(@this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ReflectOff(this Vector3 @this, Vector3 normal) => Vector3.Reflect(@this, normal);

    #endregion Manipulation

    #region Distance

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceFrom(this Vector3 @this, Vector3 other) => Vector3.Distance(@this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSquaredFrom(this Vector3 @this, Vector3 other)
    {
        return Vector3.DistanceSquared(@this, other);
    }

    #endregion Distance
}