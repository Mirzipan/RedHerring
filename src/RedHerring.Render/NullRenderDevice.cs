using System.Numerics;
using RedHerring.Render.Features;
using Silk.NET.Maths;

namespace RedHerring.Render;

public class NullRenderDevice : RenderDevice
{
    public RenderFeatureCollection Features { get; } = new();
    public Shared Shared => throw new NullReferenceException("Shared resources are not available in NullRendererContext!");

    public void Init(RenderFeatureCollection features)
    {
        
    }

    public void Init(RenderFeature feature)
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

    public void Draw(RenderContext context)
    {
    }

    public void EndDraw()
    {
    }

    public void Resize(Vector2D<int> size)
    {
    }
    
    public void ReloadShaders(RenderFeatureCollection features)
    {
    }
}