using System.Numerics;
using Assimp;
using RedHerring.Render.Animations;
using Animation = RedHerring.Render.Animations.Animation;
using Quaternion = System.Numerics.Quaternion;

namespace RedHerring.Studio.Import;

internal static class NodeAnimation
{
    public static void Copy(Assimp.Animation source, Animation destination)
    {
        destination.BoneAnimationChannels.Clear();
        
        for (int i = 0; i < source.NodeAnimationChannelCount; i++)
        {
            var sourceChannel = source.NodeAnimationChannels[i];
            var destinationChannel = new BoneAnimationChannel();
            CopyChannel(sourceChannel, destinationChannel);
            destination.BoneAnimationChannels.Add(destinationChannel);
        }
    }

    private static void CopyChannel(NodeAnimationChannel source, BoneAnimationChannel destination)
    {
        destination.NodeName = source.NodeName;
        destination.PreState = source.PreState.ToExtrapolationKind();
        destination.PostState = source.PostState.ToExtrapolationKind();

        destination.Positions.Clear();
        for (int i = 0; i < source.PositionKeyCount; i++)
        {
            var key = source.PositionKeys[i];
            var keyframe = new VectorKeyframe(key.Time, key.Value.ToVector3());
            destination.Positions.Add(keyframe);
        }

        destination.Rotations.Clear();
        for (int i = 0; i < source.RotationKeyCount; i++)
        {
            var key = source.RotationKeys[i];
            var keyframe = new QuaternionKeyframe(key.Time, key.Value.ToQuaternion());
            destination.Rotations.Add(keyframe);
        }

        destination.Scalings.Clear();
        for (int i = 0; i < source.ScalingKeyCount; i++)
        {
            var key = source.ScalingKeys[i];
            var keyframe = new VectorKeyframe(key.Time, key.Value.ToVector3());
            destination.Scalings.Add(keyframe);
        }
    }

    private static Vector3 ToVector3(this Vector3D @this) => new(@this.X, @this.Y, @this.Z);

    private static Quaternion ToQuaternion(this Assimp.Quaternion @this) => new(@this.X, @this.Y, @this.Z, @this.W);

    private static ExtrapolationKind ToExtrapolationKind(this AnimationBehaviour @this)
    {
        return @this switch
        {
            AnimationBehaviour.Default => ExtrapolationKind.Default,
            AnimationBehaviour.Constant => ExtrapolationKind.Constant,
            AnimationBehaviour.Linear => ExtrapolationKind.Linear,
            AnimationBehaviour.Repeat => ExtrapolationKind.Repeat,
            _ => ExtrapolationKind.Default
        };
    }
}