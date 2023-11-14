using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static class IntExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ClampToByte(this int @this)
    {
        return @this switch
        {
            < 0 => 0,
            > 255 => 255,
            _ => (byte)@this,
        };
    }
}