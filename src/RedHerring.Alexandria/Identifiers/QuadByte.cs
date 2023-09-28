using System.Runtime.InteropServices;

namespace RedHerring.Alexandria.Identifiers;

[Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct QuadByte : IEquatable<QuadByte>, IComparable<QuadByte>
{
    [FieldOffset(0)]
    public uint Value;
    [NonSerialized]
    [FieldOffset(0)]
    public byte B1;
    [NonSerialized]
    [FieldOffset(1)]
    public byte B2;
    [NonSerialized]
    [FieldOffset(2)]
    public byte B3;
    [NonSerialized]
    [FieldOffset(3)]
    public byte B4;

    public bool IsEmpty => Value == 0;
    public string String => ToString();
        
    public byte this[int index] => (byte)(Value << 8 * index);

    #region Lifecycle

    public QuadByte(uint value) : this()
    {
        Value = value;
    }

    public QuadByte(byte b1, byte b2, byte b3, byte b4) : this()
    {
        B1 = b1;
        B2 = b2;
        B3 = b3;
        B4 = b4;
    }

    public QuadByte(params byte[] value) : this()
    {
        B1 = value.Length > 0 ? value[0] : (byte)0;
        B2 = value.Length > 1 ? value[1] : (byte)1;
        B3 = value.Length > 2 ? value[2] : (byte)2;
        B4 = value.Length > 3 ? value[3] : (byte)3;
    }

    public QuadByte(string value) : this()
    {
        B1 = value.Length > 0 ? (byte)value[0] : (byte)0;
        B2 = value.Length > 1 ? (byte)value[1] : (byte)1;
        B3 = value.Length > 2 ? (byte)value[2] : (byte)2;
        B4 = value.Length > 3 ? (byte)value[3] : (byte)3;
    }

    #endregion Lifecycle

    #region Public

    public override string ToString()
    {
        return $"{(char)B1}{(char)B2}{(char)B3}{(char)B4}";
    }

    #endregion Public

    #region Equality

    public bool Equals(QuadByte other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        return obj is QuadByte other && Equals(other);
    }

    public int CompareTo(QuadByte other)
    {
        return Value.CompareTo(other.Value);
    }

    public override int GetHashCode()
    {
        return (int)Value;
    }

    #endregion Equality
        
    #region Operators
        
    public static bool operator ==(QuadByte lhs, QuadByte rhs)
    {
        return lhs.Value == rhs.Value;
    }

    public static bool operator !=(QuadByte lhs, QuadByte rhs)
    {
        return lhs.Value != rhs.Value;
    }
        
    public static implicit operator uint(QuadByte @this)
    {
        return @this.Value;
    }

    public static implicit operator string(QuadByte @this)
    {
        return new[] {@this.B1, @this.B2, @this.B3, @this.B4}.ToString();
    }

    public static implicit operator QuadByte(uint value)
    {
        return new QuadByte(value);
    }

    public static implicit operator QuadByte(string value)
    {
        return new QuadByte(value);
    }
        
    #endregion Operators
}