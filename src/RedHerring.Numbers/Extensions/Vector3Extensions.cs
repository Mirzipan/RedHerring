using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static class Vector3Extensions
{
    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNaN(this in Vector3 @this)
    {
        return float.IsNaN(@this.X) || float.IsNaN(@this.Y) || float.IsNaN(@this.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Approximately(this in Vector3 @this, in Vector3 other, in float tolerance = float.Epsilon)
    {
        var delta = @this - other;
        return delta.LengthSquared() <= tolerance * tolerance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(this in Vector3 @this, in float tolerance = float.Epsilon)
    {
        return @this.Approximately(Vector3.Zero, tolerance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotZero(this in Vector3 @this, in float tolerance = float.Epsilon)
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
    public static Vector3 Saturate(this in Vector3 @this) => Clamp(@this, Vector3.Zero, Vector3.One);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Clamp(this in Vector3 @this, in float min, in float max)
    {
        return new Vector3(
            float.Clamp(@this.X, min, max),
            float.Clamp(@this.Y, min, max),
            float.Clamp(@this.Z, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Clamp(this in Vector3 @this, in Vector3 min, in Vector3 max)
    {
        return new Vector3(
            float.Clamp(@this.X, min.X, max.X),
            float.Clamp(@this.Y, min.Y, max.Y),
            float.Clamp(@this.Z, min.Z, max.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ClampX(this in Vector3 @this, in float min, in float max)
    {
        return @this.WithX(float.Clamp(@this.X, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ClampY(this in Vector3 @this, in float min, in float max)
    {
        return @this.WithY(float.Clamp(@this.Y, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ClampZ(this in Vector3 @this, in float min, in float max)
    {
        return @this.WithZ(float.Clamp(@this.Y, min, max));
    }

    #endregion Clamp    
    
    #region Manipulation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Truncate(this in Vector3 @this)
    {
        return new Vector3(MathF.Truncate(@this.X), MathF.Truncate(@this.Y), MathF.Truncate(@this.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Round(this in Vector3 @this)
    {
        return new Vector3(MathF.Round(@this.X), MathF.Round(@this.Y), MathF.Round(@this.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Abs(this in Vector3 @this)
    {
        return new Vector3(MathF.Abs(@this.X), MathF.Abs(@this.Y), MathF.Abs(@this.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Sign(this in Vector3 @this)
    {
        return new Vector3(MathF.Sign(@this.X), MathF.Sign(@this.Y), MathF.Sign(@this.Z));
    }

    #endregion Manipulation
}