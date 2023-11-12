﻿using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static class Vector4Extensions
{
    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNaN(this in Vector4 @this)
    {
        return float.IsNaN(@this.X) || float.IsNaN(@this.Y) || float.IsNaN(@this.Z) || float.IsNaN(@this.W);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Approximately(this in Vector4 @this, in Vector4 other, in float tolerance = float.Epsilon)
    {
        var delta = @this - other;
        return delta.LengthSquared() <= tolerance * tolerance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(this in Vector4 @this, in float tolerance = float.Epsilon)
    {
        return @this.Approximately(Vector4.Zero, tolerance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotZero(this in Vector4 @this, in float tolerance = float.Epsilon)
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
    public static Vector4 Saturate(this in Vector4 @this) => Clamp(@this, Vector4.Zero, Vector4.One);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Clamp(this in Vector4 @this, in float min, in float max)
    {
        return new Vector4(
            float.Clamp(@this.X, min, max),
            float.Clamp(@this.Y, min, max),
            float.Clamp(@this.Z, min, max),
            float.Clamp(@this.W, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Clamp(this in Vector4 @this, in Vector4 min, in Vector4 max)
    {
        return new Vector4(
            float.Clamp(@this.X, min.X, max.X),
            float.Clamp(@this.Y, min.Y, max.Y),
            float.Clamp(@this.Z, min.Z, max.Z),
            float.Clamp(@this.W, min.W, max.W));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ClampX(this in Vector4 @this, in float min, in float max)
    {
        return @this.WithX(float.Clamp(@this.X, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ClampY(this in Vector4 @this, in float min, in float max)
    {
        return @this.WithY(float.Clamp(@this.Y, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ClampZ(this in Vector4 @this, in float min, in float max)
    {
        return @this.WithZ(float.Clamp(@this.Z, min, max));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ClampW(this in Vector4 @this, in float min, in float max)
    {
        return @this.WithW(float.Clamp(@this.W, min, max));
    }

    #endregion Clamp

    #region Manipulation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Truncate(this in Vector4 @this)
    {
        return new Vector4(
            MathF.Truncate(@this.X),
            MathF.Truncate(@this.Y),
            MathF.Truncate(@this.Z),
            MathF.Truncate(@this.W));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Round(this in Vector4 @this)
    {
        return new Vector4(
            MathF.Round(@this.X),
            MathF.Round(@this.Y),
            MathF.Round(@this.Z), 
            MathF.Round(@this.W));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Abs(this in Vector4 @this)
    {
        return new Vector4(
            MathF.Abs(@this.X), 
            MathF.Abs(@this.Y), 
            MathF.Abs(@this.Z), 
            MathF.Abs(@this.W));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Sign(this in Vector4 @this)
    {
        return new Vector4(
            MathF.Sign(@this.X), 
            MathF.Sign(@this.Y), 
            MathF.Sign(@this.Z), 
            MathF.Sign(@this.W));
    }

    #endregion Manipulation
}