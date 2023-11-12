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

    public Color(int red, int green, int blue, int alpha = 255)
    {
        Unsafe.SkipInit(out this);
        
        R = red.ClampToByte();
        G = green.ClampToByte();
        B = blue.ClampToByte();
        A = alpha.ClampToByte();
    }

    public Color(ReadOnlySpan<byte> values)
    {
        if (values.Length < Count)
        {
            throw new ArgumentOutOfRangeException(nameof(values));
        }

        this = Unsafe.ReadUnaligned<Color>(ref MemoryMarshal.GetReference(values));
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color Clamp(Color min, Color max)
    {
        return new Color(
            byte.Clamp(R, min.R, max.R), 
            byte.Clamp(G, min.G, max.G), 
            byte.Clamp(B, min.B, max.B), 
            byte.Clamp(A, min.A, max.A));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color Negate()
    {
        return new Color(255 - R, 255 - G, 255 - B, A);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Clamp(Color value, Color min, Color max) => value.Clamp(min, max);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Min(Color lhs, Color rhs)
    {
        return new Color(
            lhs.R < rhs.R ? lhs.R : rhs.R,
            lhs.G < rhs.G ? lhs.G : rhs.G,
            lhs.B < rhs.B ? lhs.B : rhs.B,
            lhs.A < rhs.A ? lhs.A : rhs.A);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Max(Color lhs, Color rhs)
    {
        return new Color(
            lhs.R > rhs.R ? lhs.R : rhs.R,
            lhs.G > rhs.G ? lhs.G : rhs.G,
            lhs.B > rhs.B ? lhs.B : rhs.B,
            lhs.A > rhs.A ? lhs.A : rhs.A);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Negate(Color value) => value.Negate();

    public override string ToString() => $"({R}, {G}, {B}, {A})";

    #endregion Queries

    #region Operators

    public static bool operator ==(Color lhs, Color rhs) => lhs._value == rhs._value;

    public static bool operator !=(Color lhs, Color rhs) => lhs._value != rhs._value;

    public static Color operator +(Color lhs, Color rhs)
    {
        return new Color(lhs.R + rhs.R, lhs.G + rhs.G, lhs.B + rhs.B, lhs.A + rhs.A);
    }

    public static implicit operator Color(uint value) => new(value);

    #endregion Operators

    #region Copy

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void CopyTo(byte[]? destination) => CopyTo(destination, 0);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void CopyTo(byte[]? destination, int index)
    {
        if (destination is null)
        {
            throw new NullReferenceException(nameof(destination));
        }

        if (index < 0 || index >= destination.Length)
        {
            throw new IndexOutOfRangeException();
        }

        if (destination.Length - index < Count)
        {
            throw new ArgumentOutOfRangeException(nameof(destination));
        }

        destination[index] = R;
        destination[index + 1] = G;
        destination[index + 2] = B;
        destination[index + 3] = A;
    }
    
    public readonly void CopyTo(Span<byte> destination)
    {
        if (destination.Length < Count)
        {
            throw new ArgumentOutOfRangeException(nameof(destination));
        }

        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), this);
    }

    public readonly bool TryCopyTo(Span<byte> destination)
    {
        if (destination.Length < Count)
        {
            return false;
        }

        Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), this);

        return true;
    }

    #endregion Copy

}