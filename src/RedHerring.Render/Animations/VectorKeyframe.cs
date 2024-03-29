using System.Numerics;

namespace RedHerring.Render.Animations;

public readonly struct VectorKeyframe : IEquatable<VectorKeyframe>, IComparable<VectorKeyframe>
{
    public readonly double Time;
    public readonly Vector3 Value;

    public VectorKeyframe(double time, Vector3 value)
    {
        Time = time;
        Value = value;
    }

    public static bool operator ==(VectorKeyframe left, VectorKeyframe right) => left.Value == right.Value;
    public static bool operator !=(VectorKeyframe left, VectorKeyframe right) => left.Value != right.Value;
    public static bool operator <(VectorKeyframe left, VectorKeyframe right) => left.Time < right.Time;
    public static bool operator >(VectorKeyframe left, VectorKeyframe right) => left.Time > right.Time;
    
    public bool Equals(VectorKeyframe other) => this == other;
    public override bool Equals(object? obj) => obj is VectorKeyframe key && Equals(key);
    
    public int CompareTo(VectorKeyframe other) => Time.CompareTo(other.Time);
    
    public override int GetHashCode() => Value.GetHashCode();
}