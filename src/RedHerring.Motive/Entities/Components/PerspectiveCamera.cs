using System.Numerics;
using RedHerring.Infusion.Attributes;
using RedHerring.Render;
using Veldrid;

namespace RedHerring.Motive.Entities.Components;

public class PerspectiveCamera : EntityComponent
{
    [Infuse]
    private RendererContext _rendererContext = null!;
    
    private Matrix4x4 _viewMatrix;
    private Matrix4x4 _projectionMatrix;
    private float _fieldOfView;
    private float _clipPlaneNear;
    private float _clipPlaneFar;

    private bool _isDirty;

    public Matrix4x4 ViewMatrix
    {
        get 
        {
            if (_isDirty)
            {
                CalculateMatrices();
            }

            return _viewMatrix;
        }
    }
    
    public Matrix4x4 ProjectionMatrix
    {
        get
        {
            if (_isDirty)
            {
                CalculateMatrices();
            }

            return _projectionMatrix;
        }
    }
    
    public float FieldOfView => _fieldOfView;
    public float ClipPlaneNear => _clipPlaneNear;
    public float ClipPlaneFar => _clipPlaneFar;

    public void SetFieldOfView(float fov)
    {
        _fieldOfView = float.Clamp(fov, MathF.PI / 4, MathF.PI);
    }

    public void SetClipPlanes(float near, float far)
    {
        _clipPlaneNear = MathF.Max(near, 0.01f);
        _clipPlaneFar = MathF.Max(far, near + 0.01f);
    }
    
    public void Submit()
    {
        if (Entity?.Transform is null)
        {
            return;
        }

        if (_isDirty)
        {
            CalculateMatrices();
        }
        
        var world = Entity.Transform.WorldMatrix;
        _rendererContext.SetCameraViewMatrix(world, _viewMatrix, _projectionMatrix, FieldOfView, ClipPlaneNear, ClipPlaneFar);
    }

    private void CalculateMatrices()
    {
        if (Entity?.Transform is null)
        {
            return;
        }
        
        Viewport viewport = new Viewport(0, 0, 100, 100, 1, 100);
        float aspectRatio = viewport.Width / viewport.Height;
        
        var world = Entity.Transform.WorldMatrix;
        Matrix4x4.Invert(world, out _viewMatrix);
        _projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(FieldOfView, aspectRatio, ClipPlaneNear, ClipPlaneFar);
        
        _isDirty = false;
    }
}