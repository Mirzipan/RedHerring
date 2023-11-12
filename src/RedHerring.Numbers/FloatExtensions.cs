namespace RedHerring.Numbers;

public static class FloatExtensions
{
    public static byte ClampToByte(this float @this) => ((int)(@this * 255f)).ClampToByte();
}