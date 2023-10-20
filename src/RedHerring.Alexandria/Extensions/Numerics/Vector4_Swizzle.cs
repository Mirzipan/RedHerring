using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Alexandria.Extensions.Numerics;

public static partial class Vector4Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 XXXX(this Vector4 @this) => new(@this.X, @this.X, @this.X, @this.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 YYYY(this Vector4 @this) => new(@this.Y, @this.Y, @this.Y, @this.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ZZZZ(this Vector4 @this) => new(@this.Z, @this.Z, @this.Z, @this.Z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 WWWW(this Vector4 @this) => new(@this.W, @this.W, @this.W, @this.W);
}