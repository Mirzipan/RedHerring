﻿using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static class Vector3Utils
{
    public static Vector3 NegativeOne
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Vector3(-1f);
    }

    public static Vector3 Half
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Vector3(-0.5f);
    }

    public static Vector3 Up
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Vector3(0, 1, 0);
    }

    public static Vector3 Down
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Vector3(0, -1, 0);
    }

    public static Vector3 Left
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Vector3(-1, 0, 0);
    }

    public static Vector3 Right
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Vector3(1, 0, 0);
    }

    public static Vector3 Forward
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Vector3(0, 0, -1);
    }

    public static Vector3 Backward
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Vector3(0, 0, 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SeparateMinMax(ref Vector3 min, ref Vector3 max)
    {
        if (min.X > max.X)
        {
            (min.X, max.X) = (max.X, min.X);
        }
        
        if (min.Y > max.Y)
        {
            (min.Y, max.Y) = (max.Y, min.Y);
        }
        
        if (min.Z > max.Z)
        {
            (min.Z, max.Z) = (max.Z, min.Z);
        }
    }
}