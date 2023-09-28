namespace RedHerring.Alexandria.Masks;

public struct BitMask16 : IComparable<BitMask16>, IEquatable<BitMask16>
{
    public const int Count = 16;

    public static readonly BitMask16 Empty = new BitMask16(false);
    public static readonly BitMask16 Full = new BitMask16(true);
        
    public ushort Bits;

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

    public BitMask16(ushort value)
    {
        Bits = value;
    }
        
    public BitMask16(bool fill) => Bits = fill ? ushort.MaxValue : ushort.MinValue;

    public void Set(bool fill) => Bits = fill ? ushort.MaxValue : ushort.MinValue;
    public void Set(int index) => Bits |= (ushort)(1 << index);
    public void Unset(int index) => Bits &= (ushort)~(1 << index);
    public void Toggle(int index) => Bits ^= (ushort)(1 << index);

    public override string ToString()
    {
        return Convert.ToString(Bits, 2).PadLeft(16, '0');
    }

    #region Comparison

    public int CompareTo(BitMask16 other) => Bits.CompareTo(other.Bits);

    #endregion Comparison

    #region Equality

    public bool Equals(BitMask16 other) => Bits == other.Bits;
    public override bool Equals(object obj) => obj is BitMask16 other && Equals(other);

    public override int GetHashCode() => Bits.GetHashCode();

    public static bool operator ==(BitMask16 lhs, BitMask16 rhs) => lhs.Equals(rhs);
    public static bool operator !=(BitMask16 lhs, BitMask16 rhs) => !lhs.Equals(rhs);

    #endregion Equality
}