using System.Runtime.InteropServices;

namespace RedHerring.Alexandria.Identifiers;

[Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct CompositeId: IEquatable<CompositeId>, IComparable<CompositeId>
{
    [FieldOffset(0)]   
    public ulong Value;
    [NonSerialized]
    [FieldOffset(0)]   
    public OctoByte Combined;
    [NonSerialized]
    [FieldOffset(0)]   
    public QuadByte Primary;
    [NonSerialized]
    [FieldOffset(4)]   
    public QuadByte Secondary;

    public bool IsEmpty => Value == 0u;
    public string String => ToString();

    #region Lifecycle

    public CompositeId(ulong value) : this()
    {
        Value = value;
    }

    public CompositeId(OctoByte value) : this()
    {
        Combined = value;
    }

    public CompositeId(QuadByte primary, QuadByte secondary) : this()
    {
        Primary = primary;
        Secondary = secondary;
    }

    #endregion Lifecycle

    #region Public

    public override string ToString()
    {
        return $"{Primary}/{Secondary}";
    }

    #endregion Public

    #region Equality

    public bool Equals(CompositeId other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object obj)
    {
        return obj is CompositeId other && Equals(other);
    }

    public int CompareTo(CompositeId other)
    {
        return Value.CompareTo(other.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    #endregion Equality

    #region Operators
        
    public static bool operator ==(CompositeId lhs, CompositeId rhs)
    {
        return lhs.Value == rhs.Value;
    }

    public static bool operator !=(CompositeId lhs, CompositeId rhs)
    {
        return lhs.Value != rhs.Value;
    }
        
    public static implicit operator ulong(CompositeId @this)
    {
        return @this.Value;
    }

    #endregion Operators
}