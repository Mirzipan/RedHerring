// ReSharper disable InconsistentNaming
namespace RedHerring.Numbers;

public static class ColorPacker
{
    public static uint PackUNorm(float bitmask, float value)
    {
        value *= bitmask;
        return (uint)value.ClampAndRound(0.0f, bitmask);
    }

    public static float UnpackUNorm(uint bitmask, uint value)
    {
        value &= bitmask;
        return (float)value / bitmask;
    }

    public static uint PackRGBA(float r, float g, float b, float a)
    {
        uint pr = PackUNorm(255f, r);
        uint pg = PackUNorm(255f, g) << 8;
        uint pb = PackUNorm(255f, b) << 16;
        uint pa = PackUNorm(255f, a) << 24;
        return pr | pg | pb | pa;
    }
    
    public static void UnpackRGBA(uint packedValue, out float r, out float g, out float b, out float a)
    {
        r = UnpackUNorm(255, packedValue);
        g = UnpackUNorm(255, packedValue >> 8);
        b = UnpackUNorm(255, packedValue >> 16);
        a = UnpackUNorm(255, packedValue >> 24);
    }
    
    public static uint PackHSV(HsvColor color)
    {
        int hue = (color.H * 360f).RoundToInt();
        int saturation = (color.S * 100f + 100f).RoundToInt() << 16;
        int value = (color.V * 100f + 100f).RoundToInt() << 24;
        return (uint)(hue | saturation | value);
    }
    
    public static HsvColor UnpackHSV(uint packedValue)
    {
        ushort hue = (ushort)packedValue;
        byte saturation = (byte)(packedValue >> 16);
        byte value = (byte)(packedValue >> 24);
        return new HsvColor(hue / 360f, (saturation - 100) / 100f, (value - 100) / 100f, 1f);
    }
}