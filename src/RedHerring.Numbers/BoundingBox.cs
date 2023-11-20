using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RedHerring.Numbers;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct BoundingBox : IEquatable<BoundingBox>
{
    public const int CornerCount = 8;

    public static BoundingBox Empty
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Vector3.Zero, Vector3.Zero);
    }

    public Vector3 Minimum;
    public Vector3 Maximum;

    public Vector3 Center
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (Minimum + Maximum) * .5f;
    }
    
    public Vector3 Extent
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (Maximum - Minimum) * .5f;
    }
    
    public Vector3 Size
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Maximum - Minimum;
    }

    public float Width
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Maximum.X - Minimum.X;
    }
    
    public float Height
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Maximum.Y - Minimum.Y;
    }
    
    public float Depth
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Maximum.Z - Minimum.Z;
    }
    
    public float Volume
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var size = Maximum - Minimum;
            return size.X * size.Y * size.Z;
        }
    }
    
    public float Perimeter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var size = Maximum - Minimum;
            return 4 * (size.X + size.Y + size.Z);
        }
    }
    
    public float SurfaceArea
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            var size = Maximum - Minimum;
            return 2 * (size.X * size.Y + size.Y * size.Z + size.Z * size.X);
        }
    }

    #region Lifecycle

    public BoundingBox(Vector3 minimum, Vector3 maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    #endregion Lifecycle

    #region Equality

    public bool Equals(BoundingBox other) => Minimum.Equals(other.Minimum) && Maximum.Equals(other.Maximum);

    public override bool Equals(object? obj) => obj is BoundingBox other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Minimum, Maximum);

    #endregion Equality

    #region Queries

    public readonly void Deconstruct(out Vector3 minimum, out Vector3 maximum)
    {
        minimum = Minimum;
        maximum = Maximum;
    }    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly BoundingBox WithMinimum(Vector3 minimum) => this with { Minimum = minimum};
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly BoundingBox WithMaximum(Vector3 maximum) => this with { Maximum = maximum};

    public Vector3[] Corners()
    {
        var result = new Vector3[CornerCount];
        result[0] = new Vector3(Minimum.X, Maximum.Y, Maximum.Z);
        result[1] = new Vector3(Maximum.X, Maximum.Y, Maximum.Z);
        result[2] = new Vector3(Maximum.X, Minimum.Y, Maximum.Z);
        result[3] = new Vector3(Minimum.X, Minimum.Y, Maximum.Z);
        result[4] = new Vector3(Minimum.X, Maximum.Y, Minimum.Z);
        result[5] = new Vector3(Maximum.X, Maximum.Y, Minimum.Z);
        result[6] = new Vector3(Maximum.X, Minimum.Y, Minimum.Z);
        result[7] = new Vector3(Minimum.X, Minimum.Y, Minimum.Z);
        return result;
    }

    public void Corners(Vector3[] corners)
    {
        if (corners.Length < CornerCount)
        {
            throw new ArgumentOutOfRangeException(nameof(corners));
        }
        
        corners[0] = new Vector3(Minimum.X, Maximum.Y, Maximum.Z);
        corners[1] = new Vector3(Maximum.X, Maximum.Y, Maximum.Z);
        corners[2] = new Vector3(Maximum.X, Minimum.Y, Maximum.Z);
        corners[3] = new Vector3(Minimum.X, Minimum.Y, Maximum.Z);
        corners[4] = new Vector3(Minimum.X, Maximum.Y, Minimum.Z);
        corners[5] = new Vector3(Maximum.X, Maximum.Y, Minimum.Z);
        corners[6] = new Vector3(Maximum.X, Minimum.Y, Minimum.Z);
        corners[7] = new Vector3(Minimum.X, Minimum.Y, Minimum.Z);
    }
    
    public readonly override string ToString() => ToString("G", CultureInfo.CurrentCulture);

    public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format,
        IFormatProvider? formatProvider)
    {
        return 
            $"<Minimum:{Minimum.ToString(format, formatProvider)} Maximum:{Maximum.ToString(format, formatProvider)}>";
    }

    #endregion Queries

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(BoundingBox left, BoundingBox right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BoundingBox left, BoundingBox right)
    {
        return left.Minimum != right.Minimum || left.Maximum != right.Maximum;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox operator +(BoundingBox left, BoundingBox right)
    {
        return new BoundingBox(Vector3.Min(left.Minimum, right.Minimum), Vector3.Max(left.Maximum, right.Maximum));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox operator *(BoundingBox box, float scale)
    {
        Vector3 center = box.Center;
        Vector3 extent = box.Extent * scale;
        return new BoundingBox(center - extent, center + extent);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox operator *(BoundingBox box, Vector3 scale)
    {
        Vector3 center = box.Center;
        Vector3 extent = box.Extent * scale;
        return new BoundingBox(center - extent, center + extent);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox Add(BoundingBox left, BoundingBox right) => left + right;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox Multiply(BoundingBox box, float scale) => box * scale;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox Multiply(BoundingBox box, Vector3 scale) => box * scale;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Intersect(BoundingBox left, BoundingBox right)
    {
        return left.Maximum.X >= right.Minimum.X && left.Minimum.X <= right.Maximum.X
            && left.Maximum.Y >= right.Minimum.Y && left.Minimum.Y <= right.Maximum.Y
            && left.Maximum.Z >= right.Minimum.Z && left.Minimum.Z <= right.Maximum.Z;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox? Intersection(BoundingBox left, BoundingBox right)
    {
        Vector3 min = Vector3.Max(left.Minimum, right.Minimum);
        Vector3 max = Vector3.Min(left.Maximum, right.Maximum);
        var result = new BoundingBox(min, max);

        if (min.X > max.X || min.Y > max.Y || min.Z > max.Z)
        {
            return null;
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ContainmentKind Contains(BoundingBox box, Vector3 point)
    {
        if (box.Minimum.X <= point.X && box.Maximum.X >= point.X
            && box.Minimum.Y <= point.Y && box.Maximum.Y >= point.Y
            && box.Minimum.Z <= point.Z && box.Maximum.Z >= point.Z)
        {
            return ContainmentKind.Contains;
        }

        return ContainmentKind.Disjoint;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ContainmentKind Contains(BoundingBox box1, BoundingBox box2)
    {
        if (box1.Maximum.X < box2.Minimum.X || box1.Minimum.X > box2.Maximum.X)
        {
            return ContainmentKind.Disjoint;
        }
        
        if (box1.Maximum.Y < box2.Minimum.Y || box1.Minimum.Y > box2.Maximum.Y)
        {
            return ContainmentKind.Disjoint;
        }
        
        if (box1.Maximum.Z < box2.Minimum.Z || box1.Minimum.Z > box2.Maximum.Z)
        {
            return ContainmentKind.Disjoint;
        }
        
        if (box1.Minimum.X <= box2.Minimum.X && box1.Maximum.X >= box2.Maximum.X
            && box1.Minimum.Y <= box2.Minimum.Y && box1.Maximum.Y >= box2.Maximum.Y
            && box1.Minimum.Z <= box2.Minimum.Z && box1.Maximum.Z >= box2.Maximum.Z)
        {
            return ContainmentKind.Contains;
        }

        return ContainmentKind.Intersects;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox Translate(BoundingBox box, Vector3 translation)
    {
        box.Minimum += translation;
        box.Maximum += translation;
        return box;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox Inflate(BoundingBox box, float size)
    {
        box.Minimum -= new Vector3(size);
        box.Maximum += new Vector3(size);
        return box;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox Inflate(BoundingBox box, Vector3 size)
    {
        box.Minimum -= size;
        box.Maximum += size;
        return box;
    }

    public static BoundingBox FromPoints(Vector3[]? points)
    {
        if (points is null)
        {
            throw new ArgumentNullException(nameof(points));
        }

        Vector3 min = new Vector3(float.MaxValue);
        Vector3 max = new Vector3(float.MinValue);

        for (int i = 0; i < points.Length; i++)
        {
            min = Vector3.Min(min, points[i]);
            max = Vector3.Max(max, points[i]);
        }

        return new BoundingBox(min, max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox FromExtent(Vector3 center, float extent) => FromExtent(center, new Vector3(extent));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BoundingBox FromExtent(Vector3 center, Vector3 extent)
    {
        return new BoundingBox(center - extent, center + extent);
    }

    #endregion Operators
}