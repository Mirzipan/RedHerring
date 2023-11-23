using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static class Vector2Utils
{
    public static Vector2 NegativeOne 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Vector2(-1f);
    }
    
    public static Vector2 Half 
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Vector2(-0.5f);
    }
}