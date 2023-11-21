using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static class PlaneExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Plane Normalize(this Plane @this) => Plane.Normalize(@this);
}