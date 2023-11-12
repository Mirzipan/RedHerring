using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RedHerring.Numbers;

[Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct Color : IEquatable<Color>
{
    internal const int Count = 4;
    
    [FieldOffset(0)]
    private readonly uint _value;
    
    [NonSerialized]
    [FieldOffset(0)]
    public readonly byte R;
    
    [NonSerialized]
    [FieldOffset(1)]
    public readonly byte G;
    
    [NonSerialized]
    [FieldOffset(2)]
    public readonly byte B;
    
    [NonSerialized]
    [FieldOffset(3)]
    public readonly byte A;

    public uint Value => _value;
    
    public byte this[int index]
    {
        get => GetElement(this, index);
        set => this = WithElement(this, index, value);
    }

    #region Lifecycle

    public Color(uint value)
    {
        Vector4 a;
        
        Unsafe.SkipInit(out this);
        
        _value = value;
    }

    public Color(byte value) : this(value, value, value, value)
    {
    }

    public Color(byte red, byte green, byte blue, byte alpha = 255)
    {
        Unsafe.SkipInit(out this);
        
        R = red;
        G = green;
        B = blue;
        A = alpha;
    }

    public Color(ReadOnlySpan<byte> values)
    {
        if (values.Length < 4)
        {
            throw new ArgumentOutOfRangeException(nameof(values));
        }

        this = Unsafe.ReadUnaligned<Color>(ref Unsafe.As<byte, byte>(ref MemoryMarshal.GetReference(values)));
    }
    
    internal static byte GetElement(Color color, int index)
    {
        if ((uint)index >= Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return GetElementUnsafe(ref color, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte GetElementUnsafe(ref Color vector, int index)
    {
        Debug.Assert(index is >= 0 and < Count);
        return Unsafe.Add(ref Unsafe.As<Color, byte>(ref vector), index);
    }
    
    internal static Color WithElement(Color vector, int index, byte value)
    {
        if ((uint)index >= Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        Color result = vector;
        SetElementUnsafe(ref result, index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void SetElementUnsafe(ref Color vector, int index, byte value)
    {
        Debug.Assert(index is >= 0 and < Count);
        Unsafe.Add(ref Unsafe.As<Color, byte>(ref vector), index) = value;
    }

    #endregion Lifecycle

    #region Equality

    public bool Equals(Color other) => _value == other._value;

    public override bool Equals(object? obj) => obj is Color other && Equals(other);

    public override int GetHashCode() => _value.GetHashCode();

    #endregion Equality

    #region Queries

    public readonly void Deconstruct(out byte red, out byte green, out byte blue, out byte alpha)
    {
        red = R;
        green = G;
        blue = B;
        alpha = A;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color WithRed(byte red) => new(red, G, B, A);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color WithGreen(byte green) => new(R, green, B, A);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color WithBlue(byte blue) => new(R, G, blue, A);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color WithAlpha(byte alpha) => new(R, G, B, alpha);

    public override string ToString() => $"({R}, {G}, {B}, {A})";

    #endregion Queries

    #region Operators

    public static bool operator ==(Color lhs, Color rhs) => lhs._value == rhs._value;

    public static bool operator !=(Color lhs, Color rhs) => lhs._value != rhs._value;
    
    public static implicit operator Color(uint value) => new(value);

    #endregion Operators

}