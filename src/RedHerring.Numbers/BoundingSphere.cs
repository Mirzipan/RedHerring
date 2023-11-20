using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RedHerring.Numbers;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct BoundingSphere : IEquatable<BoundingSphere>
{
    public static BoundingSphere Empty
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new BoundingSphere(Vector3.Zero, 0f);
    }
    
    public Vector3 Center;
    public float Radius;

    #region Lifecycle

    public BoundingSphere(Vector3 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    #endregion Lifecycle

    #region Equality

    public bool Equals(BoundingSphere other) => Center.Equals(other.Center) && Radius.Equals(other.Radius);

    public override bool Equals(object? obj) => obj is BoundingSphere other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Center, Radius);

    #endregion Equality

    #region Queries

    public readonly void Deconstruct(out Vector3 center, out float radius)
    {
        center = Center;
        radius = Radius;
    }    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly BoundingSphere WithCenter(Vector3 center) => this with { Center = center};
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly BoundingSphere WithRadius(float radius) => this with { Radius = radius};
    
    public readonly override string ToString() => ToString("G", CultureInfo.CurrentCulture);

    public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format,
        IFormatProvider? formatProvider)
    {
        return 
            $"<Center:{Center.ToString(format, formatProvider)} Radius:{Radius.ToString(format, formatProvider)}>";
    }
    
    #endregion Queries

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(BoundingSphere left, BoundingSphere right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BoundingSphere left, BoundingSphere right)
    {
        return left.Center != right.Center || left.Radius != right.Radius;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingSphere operator +(BoundingSphere left, BoundingSphere right)
    {
        if (left == Empty)
        {
            return right;
        }

        if (right == Empty)
        {
            return left;
        }

        Vector3 delta = right.Center - left.Center;
        float length = delta.Length();
        float radiusLeft = left.Radius;
        float radiusRight = right.Radius;

        if (radiusLeft + radiusRight >= length)
        {
            if (radiusLeft - radiusRight >= length)
            {
                return left;
            }

            if (radiusRight - radiusLeft >= length)
            {
                return right;
            }
        }

        Vector3 vector = delta * (1.0f / length);
        float min = MathF.Min(-radiusLeft, length - radiusRight);
        float max = (MathF.Max(radiusLeft, length + radiusRight) - min) * .5f;

        return new BoundingSphere(left.Center + vector * (max + min), max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingSphere operator *(BoundingSphere sphere, float scale)
    {
        return new BoundingSphere(sphere.Center, sphere.Radius * scale);
    }

    #endregion Operators
}