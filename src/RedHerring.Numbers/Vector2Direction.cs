using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static class Vector2Direction
{
    public static Vector2 Up 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Vector2(0, 1);
    }
    public static Vector2 Down 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Vector2(0, -1);
    }
    public static Vector2 Left 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Vector2(-1, 0);
    }
    public static Vector2 Right 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Vector2(1, 0);
    }
}