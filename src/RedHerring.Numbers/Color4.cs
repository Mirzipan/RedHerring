using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace RedHerring.Numbers;

[Serializable]
[StructLayout(LayoutKind.Explicit)]
public struct Color4 : IEquatable<Color4>, IFormattable
{
    internal const int Count = 4;
    
    [FieldOffset(0)]
    public float R;
    
    [FieldOffset(4)]
    public float G;
    
    [FieldOffset(8)]
    public float B;
    
    [FieldOffset(12)]
    public float A;
    
    public float this[int index]
    {
        get => GetElement(this, index);
        set => this = WithElement(this, index, value);
    }

    #region Lifecycle

    public Color4(float value) : this(value, value, value, value)
    {
    }
    
    public Color4(Vector3 color, float alpha) : this(color.X, color.Y, color.Z, alpha)
    {
    }
    
    public Color4(float red, float green, float blue, float alpha)
    {
        R = red;
        G = green;
        B = blue;
        A = alpha;
    }
    
    public Color4(ReadOnlySpan<float> values)
    {
        if (values.Length < Count)
        {
            throw new ArgumentOutOfRangeException(nameof(values));
        }

        this = Unsafe.ReadUnaligned<Color4>(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(values)));
    }
    
    internal static float GetElement(Color4 color, int index)
    {
        if ((uint)index >= Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return GetElementUnsafe(ref color, index);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float GetElementUnsafe(ref Color4 color, int index)
    {
        Debug.Assert(index is >= 0 and < Count);
        return Unsafe.Add(ref Unsafe.As<Color4, float>(ref color), index);
    }
    
    internal static Color4 WithElement(Color4 color, int index, float value)
    {
        if ((uint)index >= Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        Color4 result = color;
        SetElementUnsafe(ref result, index, value);
        return result;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void SetElementUnsafe(ref Color4 color, int index, float value)
    {
        Debug.Assert(index is >= 0 and < Count);
        Unsafe.Add(ref Unsafe.As<Color4, float>(ref color), index) = value;
    }

    #endregion Lifecycle

    #region Equality
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Color4 other)
    {
        if (Vector128.IsHardwareAccelerated)
        {
            return AsVector128().Equals(other.AsVector128());
        }

        return SoftwareFallback(in this, other);

        static bool SoftwareFallback(in Color4 self, Color4 other)
        {
            return self.R.Equals(other.R)
                   && self.G.Equals(other.G)
                   && self.B.Equals(other.B)
                   && self.A.Equals(other.A);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Color4 other && Equals(other);

    public readonly override int GetHashCode() => HashCode.Combine(R, G, B, A);

    #endregion Equality
    
    #region Queries

    public readonly void Deconstruct(out float red, out float green, out float blue, out float alpha)
    {
        red = R;
        green = G;
        blue = B;
        alpha = A;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Color4 WithRed(float red) => new(red, G, B, A);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Color4 WithGreen(float green) => new(R, green, B, A);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Color4 WithBlue(float blue) => new(R, G, blue, A);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Color4 WithAlpha(float alpha) => new(R, G, B, alpha);
    
    public readonly override string ToString()
    {
        return ToString("G", CultureInfo.CurrentCulture);
    }
    public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }
    public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
    {
        string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        return $"<{R.ToString(format, formatProvider)}{separator} {G.ToString(format, formatProvider)}{separator} {B.ToString(format, formatProvider)}{separator} {A.ToString(format, formatProvider)}>";
    }
    
    #endregion Queries

    #region Operators
    
    public Vector128<float> AsVector128() => Unsafe.As<Color4, Vector128<float>>(ref this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Color4 lhs, Color4 rhs)
    {
        return lhs.R == rhs.R && lhs.G == rhs.G && lhs.B == rhs.B && lhs.A == rhs.A;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Color4 lhs, Color4 rhs)
    {
        return !(lhs == rhs);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 operator +(Color4 lhs, Color4 rhs)
    {
        return new Color4(lhs.R + rhs.R, lhs.G + rhs.G, lhs.B + rhs.B, lhs.A + rhs.A);
    }

    #endregion Operators

    #region Copy

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void CopyTo(float[] destination)
    {
        CopyTo(destination, 0);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void CopyTo(float[] destination, int index)
    {
        if (destination is null)
        {
            throw new NullReferenceException();
        }

        if ((index < 0) || (index >= destination.Length))
        {
            throw new IndexOutOfRangeException();
        }

        if ((destination.Length - index) < 4)
        {
            throw new ArgumentOutOfRangeException(nameof(destination));
        }

        destination[index] = R;
        destination[index + 1] = G;
        destination[index + 2] = B;
        destination[index + 3] = A;
    }
    
    public readonly void CopyTo(Span<float> destination)
    {
        if (destination.Length < 4)
        {
            throw new ArgumentOutOfRangeException(nameof(destination));
        }

        Unsafe.WriteUnaligned(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(destination)), this);
    }
    
    public readonly bool TryCopyTo(Span<float> destination)
    {
        if (destination.Length < 4)
        {
            return false;
        }

        Unsafe.WriteUnaligned(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(destination)), this);

        return true;
    }

    #endregion Copy
}