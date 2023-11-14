using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace RedHerring.Numbers;

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public partial struct Color3 : IEquatable<Color3>, IFormattable
{
    internal const int Count = 3;

    public float R;
    public float G;
    public float B;
    
    public float this[int index]
    {
        get => GetElement(this, index);
        set => this = WithElement(this, index, value);
    }

    #region Lifecycle

    public Color3(float value) : this(value, value, value)
    {
    }

    public Color3(Vector3 color) : this(color.X, color.Y, color.Z)
    {
    }

    public Color3(Vector4 color) : this(color.X, color.Y, color.Z)
    {
    }

    public Color3(float red, float green, float blue)
    {
        R = red;
        G = green;
        B = blue;
    }

    public Color3(byte red, byte green, byte blue)
    {
        R = red / 255f;
        G = green / 255f;
        B = blue / 255f;
    }

    public Color3(Color color)
    {
        R = color.R / 255f;
        G = color.G / 255f;
        B = color.B / 255f;
    }

    public Color3(Color4 color)
    {
        R = color.R;
        G = color.G;
        B = color.B;
    }

    public Color3(ReadOnlySpan<float> values)
    {
        if (values.Length < Count)
        {
            throw new ArgumentOutOfRangeException(nameof(values));
        }

        this = Unsafe.ReadUnaligned<Color3>(ref Unsafe.As<float, byte>(ref MemoryMarshal.GetReference(values)));
    }

    internal static float GetElement(Color3 color, int index)
    {
        if ((uint)index >= Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return GetElementUnsafe(ref color, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float GetElementUnsafe(ref Color3 color, int index)
    {
        Debug.Assert(index is >= 0 and < Count);
        return Unsafe.Add(ref Unsafe.As<Color3, float>(ref color), index);
    }

    internal static Color3 WithElement(Color3 color, int index, float value)
    {
        if ((uint)index >= Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        Color3 result = color;
        SetElementUnsafe(ref result, index, value);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void SetElementUnsafe(ref Color3 color, int index, float value)
    {
        Debug.Assert(index is >= 0 and < Count);
        Unsafe.Add(ref Unsafe.As<Color3, float>(ref color), index) = value;
    }

    #endregion Lifecycle

    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Color3 other)
    {
        return R.Equals(other.R)
               && G.Equals(other.G)
               && B.Equals(other.B);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Color3 other && Equals(other);

    public readonly override int GetHashCode() => HashCode.Combine(R, G, B);

    #endregion Equality

    #region Queries

    public readonly void Deconstruct(out float red, out float green, out float blue)
    {
        red = R;
        green = G;
        blue = B;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Color3 WithRed(float red) => new(red, G, B);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Color3 WithGreen(float green) => new(R, green, B);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Color3 WithBlue(float blue) => new(R, G, blue);

    public Vector3 ToVector3() => new Vector3(R, G, B);

    public Vector4 ToVector4() => new Vector4(R, G, B, 1f);

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
            $"<{R.ToString(format, formatProvider)}{separator} {G.ToString(format, formatProvider)}{separator} {B.ToString(format, formatProvider)}>";
    }

    #endregion Queries

    #region Operators

    public static explicit operator Color3(Color value) => new(value);

    public static explicit operator Color3(Color4 value) => new(value);

    public static explicit operator Color3(Vector3 value) => new(value);

    public static explicit operator Color3(Vector4 value) => new(value);

    public static explicit operator Color(Color3 value) => new(value.R, value.G, value.B);

    public static explicit operator Color4(Color3 value) => new(value.R, value.G, value.B, 1f);

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