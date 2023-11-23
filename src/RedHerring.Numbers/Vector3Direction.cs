using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static class Vector3Direction
{
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
}