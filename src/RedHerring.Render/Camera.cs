using System.Numerics;

namespace RedHerring.Render;

public sealed class Camera
{
    public Matrix4x4 ViewMatrix;
    public Matrix4x4 ProjectiomMatrix;
    public Matrix4x4 ViewProjectionMatrix;

    public Vector3 Position;
    
    public float FieldOfView;
    public float ClipPlaneNear;
    public float ClipPlaneFar;
    public float AspectRatio;
}