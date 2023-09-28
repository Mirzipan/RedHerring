namespace RedHerring.Alexandria.Masks;

public struct BitMask8 : IComparable<BitMask8>, IEquatable<BitMask8>
{
    public const int Count = 8;

    public static readonly BitMask8 Empty = new BitMask8(false);
    public static readonly BitMask8 Full = new BitMask8(true);
        
    public byte Bits;

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

    public BitMask8(bool fill)
    {
        Bits = fill ? byte.MaxValue : byte.MinValue;
    }

    public void Set(bool fill) => Bits = fill ? byte.MaxValue : byte.MinValue;
    public void Set(int index) => Bits |= (byte)(1 << index);
    public void Unset(int index) => Bits &= (byte)~(1 << index);
    public void Toggle(int index) => Bits ^= (byte)(1 << index);

    public override string ToString()
    {
        return Convert.ToString(Bits, 2).PadLeft(8, '0');
    }

    #region Comparison

    public int CompareTo(BitMask8 other) => Bits.CompareTo(other.Bits);

    #endregion Comparison

    #region Equality

    public bool Equals(BitMask8 other) => Bits == other.Bits;
    public override bool Equals(object obj) => obj is BitMask8 other && Equals(other);

    public override int GetHashCode() => Bits.GetHashCode();

    public static bool operator ==(BitMask8 lhs, BitMask8 rhs) => lhs.Equals(rhs);
    public static bool operator !=(BitMask8 lhs, BitMask8 rhs) => !lhs.Equals(rhs);

    #endregion Equality
}