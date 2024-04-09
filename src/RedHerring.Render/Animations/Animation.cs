namespace RedHerring.Render.Animations;

[Serializable]
public sealed class Animation
{
    public string Name = string.Empty;
    public TimeSpan Duration;
    public double DurationInTicks = 0.00d;
    public double TicksPerSecond = 0.00d;
    public readonly List<BoneAnimationChannel> BoneAnimationChannels = new();
}