using RedHerring.Render.Animations;
using Animation = RedHerring.Render.Animations.Animation;
using AssimpAnimationBehaviour = Silk.NET.Assimp.AnimBehaviour;
using AssimpAnimation = Silk.NET.Assimp.Animation;
using AssimpAnimNode = Silk.NET.Assimp.NodeAnim;

namespace RedHerring.Studio.Import;

internal static class AssimpNodeAnimation
{
    public static unsafe void Copy(AssimpAnimation* source, Animation destination)
    {
        destination.BoneAnimationChannels.Clear();
        
        for (int i = 0; i < source->MNumChannels; i++)
        {
            var sourceChannel = source->MChannels[i];
            var destinationChannel = new BoneAnimationChannel();
            CopyChannel(sourceChannel, destinationChannel);
            destination.BoneAnimationChannels.Add(destinationChannel);
        }
    }

    private static unsafe void CopyChannel(AssimpAnimNode* source, BoneAnimationChannel destination)
    {
        destination.NodeName = source->MNodeName;
        destination.PreState = source->MPreState.ToExtrapolationKind();
        destination.PostState = source->MPostState.ToExtrapolationKind();

        destination.Positions.Clear();
        for (int i = 0; i < source->MNumPositionKeys; i++)
        {
            var key = source->MPositionKeys[i];
            var keyframe = new VectorKeyframe(key.MTime, key.MValue);
            destination.Positions.Add(keyframe);
        }

        destination.Rotations.Clear();
        for (int i = 0; i < source->MNumRotationKeys; i++)
        {
            var key = source->MRotationKeys[i];
            var keyframe = new QuaternionKeyframe(key.MTime, key.MValue);
            destination.Rotations.Add(keyframe);
        }

        destination.Scalings.Clear();
        for (int i = 0; i < source->MNumScalingKeys; i++)
        {
            var key = source->MScalingKeys[i];
            var keyframe = new VectorKeyframe(key.MTime, key.MValue);
            destination.Scalings.Add(keyframe);
        }
    }
    
    private static ExtrapolationKind ToExtrapolationKind(this AssimpAnimationBehaviour @this)
    {
        return @this switch
        {
            AssimpAnimationBehaviour.Default => ExtrapolationKind.Default,
            AssimpAnimationBehaviour.Constant => ExtrapolationKind.Constant,
            AssimpAnimationBehaviour.Linear => ExtrapolationKind.Linear,
            AssimpAnimationBehaviour.Repeat => ExtrapolationKind.Repeat,
            _ => ExtrapolationKind.Default
        };
    }
}