using System.Numerics;

namespace RedHerring.Render.Animations;

public readonly struct QuaternionKeyframe : IEquatable<QuaternionKeyframe>, IComparable<QuaternionKeyframe>
{
    public readonly double Time;
    public readonly Quaternion Value;

    public QuaternionKeyframe(double time, Quaternion value)
    {
        Time = time;
        Value = value;
    }

    public static bool operator ==(QuaternionKeyframe left, QuaternionKeyframe right) => left.Value == right.Value;
    public static bool operator !=(QuaternionKeyframe left, QuaternionKeyframe right) => left.Value != right.Value;
    public static bool operator <(QuaternionKeyframe left, QuaternionKeyframe right) => left.Time < right.Time;
    public static bool operator >(QuaternionKeyframe left, QuaternionKeyframe right) => left.Time > right.Time;
    
    public bool Equals(QuaternionKeyframe other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is QuaternionKeyframe key && Equals(key);
    
    public int CompareTo(QuaternionKeyframe other) => Time.CompareTo(other.Time);
    
    public override int GetHashCode() => Value.GetHashCode();
}