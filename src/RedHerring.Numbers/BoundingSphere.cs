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
        get => new(Vector3.Zero, 0f);
    }
    
    public Vector3 Center;
    public float Radius;

    #region Lifecycle

    public BoundingSphere(Vector3 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public BoundingSphere(ref BoundingBox box)
    {
        Center = (box.Minimum + box.Maximum) * 0.5f;
        Radius = Vector3.Distance(Center, box.Maximum);
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingSphere Include(BoundingSphere sphere, BoundingSphere other) => sphere + other;

    public static BoundingSphere Transform(BoundingSphere sphere, Matrix4x4 matrix)
    {
        Vector3 center = Vector3.Transform(sphere.Center, matrix);
        float majorAxisLengthSquared = MathF.Max(
            (matrix.M11 * matrix.M11) + (matrix.M12 * matrix.M12) + (matrix.M13 * matrix.M13), MathF.Max(
                (matrix.M21 * matrix.M21) + (matrix.M22 * matrix.M22) + (matrix.M23 * matrix.M23),
                (matrix.M31 * matrix.M31) + (matrix.M32 * matrix.M32) + (matrix.M33 * matrix.M33)));
        
        return new BoundingSphere(center, sphere.Radius * MathF.Sqrt(majorAxisLengthSquared));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Intersects(BoundingSphere sphere, BoundingBox box)
    {
        Vector3 vector = Vector3.Clamp(sphere.Center, box.Minimum, box.Maximum);
        float distanceSquared = Vector3.DistanceSquared(sphere.Center, vector);
        return distanceSquared <= sphere.Radius * sphere.Radius;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Intersects(BoundingSphere sphere, BoundingSphere other)
    {
        float radii = sphere.Radius + other.Radius;
        return Vector3.DistanceSquared(sphere.Center, other.Center) <= radii * radii;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ContainmentKind Contains(BoundingSphere sphere, Vector3 point)
    {
        if (Vector3.DistanceSquared(point, sphere.Center) <= sphere.Radius * sphere.Radius)
        {
            return ContainmentKind.Contains;
        }

        return ContainmentKind.Disjoint;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ContainmentKind Contains(BoundingSphere sphere, BoundingBox box)
    {
        if (!Intersects(sphere, box))
        {
            return ContainmentKind.Disjoint;
        }
        
        Vector3 vector;
        float radiusSquared = sphere.Radius * sphere.Radius;
        
        vector.X = sphere.Center.X - box.Minimum.X;
        vector.Y = sphere.Center.Y - box.Maximum.Y;
        vector.Z = sphere.Center.Z - box.Maximum.Z;

        if (vector.LengthSquared() > radiusSquared)
        {
            return ContainmentKind.Intersects;
        }
        
        vector.X = sphere.Center.X - box.Maximum.X;
        vector.Y = sphere.Center.Y - box.Maximum.Y;
        vector.Z = sphere.Center.Z - box.Maximum.Z;

        if (vector.LengthSquared() > radiusSquared)
        {
            return ContainmentKind.Intersects;
        }
        
        vector.X = sphere.Center.X - box.Maximum.X;
        vector.Y = sphere.Center.Y - box.Minimum.Y;
        vector.Z = sphere.Center.Z - box.Maximum.Z;

        if (vector.LengthSquared() > radiusSquared)
        {
            return ContainmentKind.Intersects;
        }
        
        vector.X = sphere.Center.X - box.Minimum.X;
        vector.Y = sphere.Center.Y - box.Minimum.Y;
        vector.Z = sphere.Center.Z - box.Maximum.Z;

        if (vector.LengthSquared() > radiusSquared)
        {
            return ContainmentKind.Intersects;
        }
        
        vector.X = sphere.Center.X - box.Minimum.X;
        vector.Y = sphere.Center.Y - box.Maximum.Y;
        vector.Z = sphere.Center.Z - box.Minimum.Z;

        if (vector.LengthSquared() > radiusSquared)
        {
            return ContainmentKind.Intersects;
        }
        
        vector.X = sphere.Center.X - box.Maximum.X;
        vector.Y = sphere.Center.Y - box.Maximum.Y;
        vector.Z = sphere.Center.Z - box.Minimum.Z;

        if (vector.LengthSquared() > radiusSquared)
        {
            return ContainmentKind.Intersects;
        }
        
        vector.X = sphere.Center.X - box.Maximum.X;
        vector.Y = sphere.Center.Y - box.Minimum.Y;
        vector.Z = sphere.Center.Z - box.Minimum.Z;

        if (vector.LengthSquared() > radiusSquared)
        {
            return ContainmentKind.Intersects;
        }
        
        vector.X = sphere.Center.X - box.Minimum.X;
        vector.Y = sphere.Center.Y - box.Minimum.Y;
        vector.Z = sphere.Center.Z - box.Minimum.Z;

        if (vector.LengthSquared() > radiusSquared)
        {
            return ContainmentKind.Intersects;
        }
        
        return ContainmentKind.Contains;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ContainmentKind Contains(BoundingSphere sphere, BoundingSphere other)
    {
        float distance = Vector3.Distance(sphere.Center, other.Center);
        if (sphere.Radius + other.Radius < distance)
        {
            return ContainmentKind.Disjoint;
        }

        if (sphere.Radius - other.Radius < distance)
        {
            return ContainmentKind.Intersects;
        }
        
        return ContainmentKind.Contains;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingSphere FromBox(BoundingBox box) => new(ref box);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingSphere FromBox(ref BoundingBox box) => new(ref box);

    #endregion Operators
}