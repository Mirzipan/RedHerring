namespace RedHerring.Numbers;

public static class FloatExtensions
{
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
}