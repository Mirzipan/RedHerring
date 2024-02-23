using System.Numerics;
using RedHerring.Alexandria;
using RedHerring.Render.Features;
using Silk.NET.Maths;
using Veldrid;

namespace RedHerring.Render;

public class NullRendererContext : RendererContext
{
    public RenderFeatureCollection Features { get; } = new();
    public void AddFeature(RenderFeature feature)
    {
    }

    public void Init()
    {
    }

    public void Close()
    {
    }

    public void Reset()
    {
    }

    public bool BeginDraw() => false;

    public void Draw()
    {
    }

    public void EndDraw()
    {
    }

    public void Resize(Vector2D<int> size)
    {
    }

    public void SetCameraViewMatrix(Matrix4x4 world, Matrix4x4 view, Matrix4x4 projection, float fieldOfView, float clipPlaneNear,
        float clipPlaneFar)
    {
    }

    public void ReloadShaders()
    {
    }
}