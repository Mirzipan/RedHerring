﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace RedHerring.Numbers;

[Serializable]
[StructLayout(LayoutKind.Explicit)]
public partial struct Color : IEquatable<Color>, IFormattable
{
    internal const int Count = 4;

    [FieldOffset(0)]
    private uint _value;

    [NonSerialized]
    [FieldOffset(0)]
    public byte R;

    [NonSerialized]
    [FieldOffset(1)]
    public byte G;

    [NonSerialized]
    [FieldOffset(2)]
    public byte B;

    [NonSerialized]
    [FieldOffset(3)]
    public byte A;

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

    public Color(byte rgba) : this(rgba, rgba, rgba, rgba)
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

    public Color(int red, int green, int blue, int alpha = 255) : this()
    {
        R = red.ClampToByte();
        G = green.ClampToByte();
        B = blue.ClampToByte();
        A = alpha.ClampToByte();
    }

    public Color(float rgb) : this()
    {
        _value = ColorPacker.PackRGBA(rgb, rgb, rgb, 1f);
    }

    public Color(float red, float green, float blue, float alpha = 1.0f) : this()
    {
        _value = ColorPacker.PackRGBA(red, green, blue, alpha);
    }

    public Color(Vector3 rgb, float alpha = 1.0f) : this()
    {
        _value = ColorPacker.PackRGBA(rgb.X, rgb.Y, rgb.Z, alpha);
    }

    public Color(Vector4 rgba) : this()
    {
        _value = ColorPacker.PackRGBA(rgba.X, rgba.Y, rgba.Z, rgba.W);
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
    public readonly Color WithRed(byte red) => new(red, G, B, A);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Color WithGreen(byte green) => new(R, green, B, A);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Color WithBlue(byte blue) => new(R, G, blue, A);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Color WithAlpha(byte alpha) => new(R, G, B, alpha);

    public int RGBA()
    {
        int value = R;
        value |= G << 8;
        value |= B << 16;
        value |= A << 24;
        return value;
    }

    public int ARGB()
    {
        int value = A;
        value |= R << 8;
        value |= G << 16;
        value |= B << 24;
        return value;
    }

    public int BGRA()
    {
        int value = B;
        value |= G << 8;
        value |= R << 16;
        value |= A << 24;
        return value;
    }

    public Vector3 ToVector3()
    {
        ColorPacker.UnpackRGBA(_value, out float x, out float y, out float z, out float _);
        return new Vector3(x, y, z);
    }

    public Vector4 ToVector4()
    {
        ColorPacker.UnpackRGBA(_value, out float x, out float y, out float z, out float w);
        return new Vector4(x, y, z, w);
    }

    public Color3 ToColor3()
    {
        ColorPacker.UnpackRGBA(_value, out float x, out float y, out float z, out float _);
        return new Color3(x, y, z);
    }

    public Color4 ToColor4()
    {
        ColorPacker.UnpackRGBA(_value, out float x, out float y, out float z, out float w);
        return new Color4(x, y, z, w);
    }

    public readonly string ToHex() => _value.ToString("x8");
    public readonly override string ToString() => ToString("G", CultureInfo.CurrentCulture);

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Color left, Color right) => left._value == right._value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Color left, Color right) => left._value != right._value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator +(Color left, Color right)
    {
        return new Color(left.R + right.R, left.G + right.G, left.B + right.B, left.A + right.A);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator -(Color left, Color right)
    {
        return new Color(left.R - right.R, left.G - right.G, left.B - right.B, left.A - right.A);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator -(Color value)
    {
        return new Color(255 - value.R, 255 - value.G, 255 - value.B, value.A);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator *(Color value, float factor)
    {
        byte r = (value.R * factor).ClampToByte();
        byte g = (value.G * factor).ClampToByte();
        byte b = (value.B * factor).ClampToByte();
        byte a = (value.A * factor).ClampToByte();

        return new Color(r, g, b, a);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator *(float left, Color right) => right * left;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Add(Color left, Color right) => left + right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Subtract(Color left, Color right) => left - right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Multiply(Color left, float right) => left * right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Multiply(float left, Color right) => left * right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Color Negate()
    {
        return -this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Negate(Color value) => -value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Color Clamp(Color min, Color max)
    {
        return new Color(
            byte.Clamp(R, min.R, max.R),
            byte.Clamp(G, min.G, max.G),
            byte.Clamp(B, min.B, max.B),
            byte.Clamp(A, min.A, max.A));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Clamp(Color value, Color min, Color max)
    {
        // We must follow HLSL behavior in the case user specified min value is bigger than max value.
        return Min(Max(value, min), max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Lerp(Color value1, Color value2, float amount)
    {
        return (value1 * (1.0f - amount)) + (value2 * amount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Min(Color left, Color right)
    {
        return new Color(
            left.R < right.R ? left.R : right.R,
            left.G < right.G ? left.G : right.G,
            left.B < right.B ? left.B : right.B,
            left.A < right.A ? left.A : right.A);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Max(Color left, Color right)
    {
        return new Color(
            left.R > right.R ? left.R : right.R,
            left.G > right.G ? left.G : right.G,
            left.B > right.B ? left.B : right.B,
            left.A > right.A ? left.A : right.A);
    }

    public static implicit operator Color(uint value) => new(value);

    public static explicit operator int(Color value) => value.RGBA();

    public static implicit operator Color4(Color value) => value.ToColor4();

    public static explicit operator Color(Vector3 value) => new(value.X, value.Y, value.Z);

    public static explicit operator Color(Vector4 value) => new(value.X, value.Y, value.Z, value.W);

    public static explicit operator Color(Color4 value) => new(value.R, value.G, value.B, value.A);

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