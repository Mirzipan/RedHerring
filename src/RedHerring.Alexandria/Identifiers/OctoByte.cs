using System.Runtime.InteropServices;

namespace RedHerring.Alexandria.Identifiers;

[Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct OctoByte : IEquatable<OctoByte>, IComparable<OctoByte>
{
    [FieldOffset(0)]
    public ulong Value;
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
    [NonSerialized]
    [FieldOffset(4)]
    public byte B5;
    [NonSerialized]
    [FieldOffset(5)]
    public byte B6;
    [NonSerialized]
    [FieldOffset(6)]
    public byte B7;
    [NonSerialized]
    [FieldOffset(7)]
    public byte B8;

    public bool IsEmpty => Value == 0;
    public string String => ToString();
        
    public byte this[int index] => (byte)(Value << 8 * index);

    #region Lifecycle

    public OctoByte(ulong value) : this()
    {
        Value = value;
    }

    public OctoByte(uint value) : this()
    {
        Value = value;
    }

    public OctoByte(QuadByte value) : this()
    {
        Value = value;
    }

    public OctoByte(byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7, byte b8) : this()
    {
        B1 = b1;
        B2 = b2;
        B3 = b3;
        B4 = b4;
        B5 = b5;
        B6 = b6;
        B7 = b7;
        B8 = b8;
    }

    public OctoByte(params byte[] value) : this()
    {
        B1 = value.Length > 0 ? value[0] : (byte)0;
        B2 = value.Length > 1 ? value[1] : (byte)0;
        B3 = value.Length > 2 ? value[2] : (byte)0;
        B4 = value.Length > 3 ? value[3] : (byte)0;
        B5 = value.Length > 4 ? value[4] : (byte)0;
        B6 = value.Length > 5 ? value[5] : (byte)0;
        B7 = value.Length > 6 ? value[6] : (byte)0;
        B8 = value.Length > 7 ? value[7] : (byte)0;
    }

    public OctoByte(string value) : this()
    {
        B1 = value.Length > 0 ? (byte)value[0] : (byte)0;
        B2 = value.Length > 1 ? (byte)value[1] : (byte)0;
        B3 = value.Length > 2 ? (byte)value[2] : (byte)0;
        B4 = value.Length > 3 ? (byte)value[3] : (byte)0;
        B5 = value.Length > 4 ? (byte)value[4] : (byte)0;
        B6 = value.Length > 5 ? (byte)value[5] : (byte)0;
        B7 = value.Length > 6 ? (byte)value[6] : (byte)0;
        B8 = value.Length > 7 ? (byte)value[7] : (byte)0;
    }

    #endregion Lifecycle

    #region Public

    public override string ToString()
    {
        return $"{(char)B1}{(char)B2}{(char)B3}{(char)B4}{(char)B5}{(char)B6}{(char)B7}{(char)B8}";
    }

    #endregion Public

    #region Equality

    public bool Equals(OctoByte other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        return obj is OctoByte other && Equals(other);
    }

    public int CompareTo(OctoByte other)
    {
        return Value.CompareTo(other.Value);
    }

    public override int GetHashCode()
    {
        return (int)Value;
    }

    #endregion Equality
        
    #region Operators
        
    public static bool operator ==(OctoByte lhs, OctoByte rhs)
    {
        return lhs.Value == rhs.Value;
    }

    public static bool operator !=(OctoByte lhs, OctoByte rhs)
    {
        return lhs.Value != rhs.Value;
    }
        
    public static implicit operator ulong(OctoByte @this)
    {
        return @this.Value;
    }

    public static implicit operator string(OctoByte @this)
    {
        return new[] {@this.B1, @this.B2, @this.B3, @this.B4, @this.B5, @this.B6, @this.B7, @this.B8}.ToString();
    }

    public static implicit operator OctoByte(uint value)
    {
        return new OctoByte(value);
    }

    public static implicit operator OctoByte(ulong value)
    {
        return new OctoByte(value);
    }

    public static implicit operator OctoByte(string value)
    {
        return new OctoByte(value);
    }

    public static implicit operator OctoByte(QuadByte value)
    {
        return new OctoByte(value);
    }
        
    #endregion Operators
}