namespace RedHerring.Alexandria.Masks;

public struct BitMask32 : IComparable<BitMask32>, IEquatable<BitMask32>
{
    public const int Count = 32;

    public static readonly BitMask32 Empty = new BitMask32(false);
    public static readonly BitMask32 Full = new BitMask32(true);
        
    public uint Bits;

    public bool this[int index]
    {
        get => ((Bits >> index) & 1) != 0;
        set
        {
            if (value)
            {
                Set(index);
            }
            else
            {
                Unset(index);
            }
        } 
    }

    public BitMask32(uint value)
    {
        Bits = value;
    }
        
    public BitMask32(bool fill) => Bits = fill ? uint.MaxValue : uint.MinValue;

    public void Set(bool fill) => Bits = fill ? uint.MaxValue : uint.MinValue;
    public void Set(int index) => Bits |= (uint)(1 << index);
    public void Unset(int index) => Bits &= (uint)~(1 << index);
    public void Toggle(int index) => Bits ^= (uint)(1 << index);

    public override string ToString()
    {
        return Convert.ToString(Bits, 2).PadLeft(32, '0');
    }

    #region Comparison

    public int CompareTo(BitMask32 other) => Bits.CompareTo(other.Bits);

    #endregion Comparison

    #region Equality

    public bool Equals(BitMask32 other) => Bits == other.Bits;
    public override bool Equals(object obj) => obj is BitMask32 other && Equals(other);

    public override int GetHashCode() => Bits.GetHashCode();

    public static bool operator ==(BitMask32 lhs, BitMask32 rhs) => lhs.Equals(rhs);
    public static bool operator !=(BitMask32 lhs, BitMask32 rhs) => !lhs.Equals(rhs);

    #endregion Equality
}