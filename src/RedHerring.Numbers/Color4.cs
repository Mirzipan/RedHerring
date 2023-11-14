using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

// ReSharper disable InconsistentNaming

namespace RedHerring.Numbers;

[Serializable]
[StructLayout(LayoutKind.Explicit)]
public partial struct Color4 : IEquatable<Color4>, IFormattable
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

    public Color4(Vector3 color, float alpha = 1f) : this(color.X, color.Y, color.Z, alpha)
    {
    }

    public Color4(Color3 color, float alpha = 1f) : this(color.R, color.G, color.B, alpha)
    {
    }

    public Color4(Vector4 color) : this(color.X, color.Y, color.Z, color.W)
    {
    }

    public Color4(float red, float green, float blue, float alpha = 1f)
    {
        R = red;
        G = green;
        B = blue;
        A = alpha;
    }

    public Color4(byte red, byte green, byte blue, byte alpha = 255)
    {
        R = red / 255f;
        G = green / 255f;
        B = blue / 255f;
        A = alpha / 255f;
    }

    public Color4(Color color)
    {
        R = color.R / 255f;
        G = color.G / 255f;
        B = color.B / 255f;
        A = color.A / 255f;
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

    public Vector4 BGRA() => new Vector4(B, G, R, A);

    public Vector3 ToVector3() => new Vector3(R, G, B);

    public Vector4 ToVector4() => new Vector4(R, G, B, A);

    public readonly override string ToString()
    {
        return ToString("G", CultureInfo.CurrentCulture);
    }

    public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format,
        IFormatProvider? formatProvider)
    {
        string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        return
            $"<{R.ToString(format, formatProvider)}{separator} {G.ToString(format, formatProvider)}{separator} {B.ToString(format, formatProvider)}{separator} {A.ToString(format, formatProvider)}>";
    }

    #endregion Queries

    #region Operators

    public Vector128<float> AsVector128() => Unsafe.As<Color4, Vector128<float>>(ref this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Color4 left, Color4 right)
    {
        return left.R == right.R && left.G == right.G && left.B == right.B && left.A == right.A;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Color4 left, Color4 right)
    {
        return !(left == right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 operator +(Color4 left, Color4 right)
    {
        return new Color4(left.R + right.R, left.G + right.G, left.B + right.B, left.A + right.A);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 operator -(Color4 left, Color4 right)
    {
        return new Color4(left.R - right.R, left.G - right.G, left.B - right.B, left.A - right.A);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 operator -(Color4 value)
    {
        return new Color4(-value.R, -value.G, -value.B, -value.A);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 operator *(Color4 left, Color4 right)
    {
        return new Color4(left.R * right.R, left.G * right.G, left.B * right.B, left.A * right.A);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 operator *(Color4 left, float right)
    {
        return new Color4(left.R * right, left.G * right, left.B * right, left.A * right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 operator *(float left, Color4 right) => right * left;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Add(Color4 left, Color4 right) => left + right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Subtract(Color4 left, Color4 right) => left - right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Multiply(Color4 left, Color4 right) => left * right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Multiply(Color4 left, float right) => left * right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Multiply(float left, Color4 right) => left * right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Negate(Color4 value) => -value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Clamp(Color4 value, Color4 min, Color4 max)
    {
        // We must follow HLSL behavior in the case user specified min value is bigger than max value.
        return Min(Max(value, min), max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Lerp(Color4 value1, Color4 value2, float amount)
    {
        return (value1 * (1.0f - amount)) + (value2 * amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Max(Color4 value1, Color4 value2)
    {
        return new Color4(
            (value1.R > value2.R) ? value1.R : value2.R,
            (value1.G > value2.G) ? value1.G : value2.G,
            (value1.B > value2.B) ? value1.B : value2.B,
            (value1.A > value2.A) ? value1.A : value2.A
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Min(Color4 value1, Color4 value2)
    {
        return new Color4(
            (value1.R < value2.R) ? value1.R : value2.R,
            (value1.G < value2.G) ? value1.G : value2.G,
            (value1.B < value2.B) ? value1.B : value2.B,
            (value1.A < value2.A) ? value1.A : value2.A
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color4 Saturate(Color4 value)
    {
        return new Color4(
            float.Clamp(value.R, 0f, 1f),
            float.Clamp(value.G, 0f, 1f),
            float.Clamp(value.B, 0f, 1f),
            float.Clamp(value.A, 0f, 1f));
    }
    
    public static implicit operator Vector3(Color4 value) => value.ToVector3();

    public static implicit operator Vector4(Color4 value) => value.ToVector4();

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

        if ((destination.Length - index) < Count)
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
        if (destination.Length < Count)
        {
            throw new ArgumentOutOfRangeException(nameof(destination));
        }

        Unsafe.WriteUnaligned(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(destination)), this);
    }

    public readonly bool TryCopyTo(Span<float> destination)
    {
        if (destination.Length < Count)
        {
            return false;
        }

        Unsafe.WriteUnaligned(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(destination)), this);

        return true;
    }

    #endregion Copy
}