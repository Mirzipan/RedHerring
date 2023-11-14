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
    
    public static void UnpackRGBA(uint value, out float r, out float g, out float b, out float a)
    {
        r = UnpackUNorm(255, value);
        g = UnpackUNorm(255, value >> 8);
        b = UnpackUNorm(255, value >> 16);
        a = UnpackUNorm(255, value >> 24);
    }
}