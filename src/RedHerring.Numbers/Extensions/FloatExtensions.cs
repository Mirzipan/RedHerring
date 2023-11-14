using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static class FloatExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ClampToByte(this float @this) => ((int)(@this * 255f)).ClampToByte();

    public static float ClampAndRound(this float @this, float min, float max)
    {
        if (float.IsNaN(@this))
        {
            return 0f;
        }

        if (float.IsInfinity(@this))
        {
            return float.IsNegativeInfinity(@this) ? min : max;
        }

        @this = float.Clamp(@this, min, max);
        return MathF.Round(@this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int RoundToInt(this float @this)
    {
        return (int)MathF.Round(@this);
    }
}