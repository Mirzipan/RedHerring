using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Extensions.Numerics;

public static partial class Vector2Extensions
{
    #region Deconstruction

    public static void Deconstruct(this Vector2 @this, out float x, out float y)
    {
        x = @this.X;
        y = @this.Y;
    }

    #endregion Deconstruction
        
    #region Validation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNaN(this Vector2 @this) => float.IsNaN(@this.X) || float.IsNaN(@this.Y);

    #endregion Validation

    #region With

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 WithX(this Vector2 @this, float X) => new(X, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 WithY(this Vector2 @this, float y) => new(@this.X, y);

    #endregion With

    #region Clamp

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Clamp(this Vector2 @this, Vector2 min, Vector2 max) => Vector2.Clamp(@this, min, max);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Clamp01(this Vector2 @this) => Clamp(@this, 0f, 1f);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Clamp(this Vector2 @this, float min, float maX)
    {
        @this.X = Math.Clamp(@this.X, min, maX);
        @this.Y = Math.Clamp(@this.Y, min, maX);
        return @this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ClampX(this Vector2 @this, float min, float maX)
    {
        return @this.WithX(Math.Clamp(@this.X, min, maX));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ClampY(this Vector2 @this, float min, float maX)
    {
        return @this.WithY(Math.Clamp(@this.Y, min, maX));
    }

    #endregion Clamp

    #region Rotation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 RotateBy90CW(this Vector2 @this) => new(-@this.Y, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 RotateBy90CCW(this Vector2 @this) => new(@this.Y, -@this.X);

    #endregion Rotation

    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Approximately(this Vector2 @this, Vector2 other, float tolerance = float.Epsilon)
    {
        var delta = @this - other;
        return delta.LengthSquared() <= tolerance * tolerance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(this Vector2 @this, float tolerance = float.Epsilon)
    {
        return @this.Approximately(Vector2.Zero, tolerance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotZero(this Vector2 @this, float tolerance = float.Epsilon)
    {
        return !@this.Approximately(Vector2.Zero, tolerance);
    }

    #endregion Equality
}