namespace RedHerring.Render.Animations;

public sealed class BoneAnimationChannel
{
    public string NodeName = string.Empty;
    public readonly List<VectorKeyframe> Positions = new();
    public readonly List<QuaternionKeyframe> Rotations = new();
    public readonly List<VectorKeyframe> Scalings = new();
    public ExtrapolationKind PreState = ExtrapolationKind.Default;
    public ExtrapolationKind PostState = ExtrapolationKind.Default;
}