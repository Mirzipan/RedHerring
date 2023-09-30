using System.Numerics;

namespace RedHerring.Entities.Components;

public sealed class TransformComponent : EntityComponent
{
    private Matrix4x4 _worldMatrix = Matrix4x4.Identity;
    private Matrix4x4 _localMatrix = Matrix4x4.Identity;

    public Matrix4x4 WorldMatrix
    {
        get => _worldMatrix;
        set => _worldMatrix = value;
    }
    public Matrix4x4 LocalMatrix
    {
        get => _localMatrix;
        set => _localMatrix = value;
    }
}