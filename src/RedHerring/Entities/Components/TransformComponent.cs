using System.Numerics;

namespace RedHerring.Entities.Components;

public sealed class TransformComponent : AnEntityComponent
{
    private TransformComponent? _parent;
    private TransformChildrenCollection _children;
    
    private Matrix4x4 _worldMatrix;
    private Matrix4x4 _localMatrix;

    public TransformComponent? Parent
    {
        get => _parent;
        set => _parent = value; // TODO: this might need extra update stuff
    }

    public TransformChildrenCollection Children => _children;

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

    public Vector3 Right => new Vector3(_worldMatrix.M11, _worldMatrix.M21, _worldMatrix.M31);
    public Vector3 Up => new Vector3(_worldMatrix.M12, _worldMatrix.M22, _worldMatrix.M32);
    public Vector3 Forward => new Vector3(_worldMatrix.M13, _worldMatrix.M23, _worldMatrix.M33);

    #region Lifecycle

    public TransformComponent()
    {
        _children = new TransformChildrenCollection(this);
        
        _worldMatrix = Matrix4x4.Identity;
        _localMatrix = Matrix4x4.Identity;
    }

    #endregion Lifecycle
}