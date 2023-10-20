using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Alexandria.Extensions.Numerics;

public static partial class Vector3Extensions
{
    #region Deconstruction

    public static void Deconstruct(this Vector3 @this, out float x, out float y, out float z)
    {
        x = @this.X;
        y = @this.Y;
        z = @this.Z;
    }

    #endregion Deconstruction
        
    #region Validation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNaN(this Vector3 @this)
    {
        return float.IsNaN(@this.X) || float.IsNaN(@this.Y) || float.IsNaN(@this.Z);
    }

    #endregion Validation

    #region With

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WithX(this Vector3 @this, float x) => new(x, @this.Y, @this.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WithY(this Vector3 @this, float y) => new(@this.X, y, @this.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WithZ(this Vector3 @this, float z) => new(@this.X, @this.Y, z);

    #endregion With

    #region Clamp
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Clamp(this Vector3 @this, Vector3 min, Vector3 max) => Vector3.Clamp(@this, min, max);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Clamp01(this Vector3 @this) => Clamp(@this, 0f, 1f);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Clamp(this Vector3 @this, float min, float max)
    {
        @this.X = Math.Clamp(@this.X, min, max);
        @this.Y = Math.Clamp(@this.Y, min, max);
        @this.Z = Math.Clamp(@this.Z, min, max);
        return @this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ClampX(this Vector3 @this, float min, float max)
    {
        return @this.WithX(Math.Clamp(@this.X, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ClampY(this Vector3 @this, float min, float max)
    {
        return @this.WithY(Math.Clamp(@this.Y, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ClampZ(this Vector3 @this, float min, float max)
    {
        return @this.WithZ(Math.Clamp(@this.Z, min, max));
    }

    #endregion Clamp

    #region Equality

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
        
    #region Manipulation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Abs(this Vector3 @this) => Vector3.Abs(@this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Round(this Vector3 @this)
    {
        return new Vector3(MathF.Round(@this.X), MathF.Round(@this.Y), MathF.Round(@this.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Sign(this Vector3 @this)
    {
        return new Vector3(MathF.Sign(@this.X), MathF.Sign(@this.Y), MathF.Sign(@this.Z));
    }

    #endregion
}