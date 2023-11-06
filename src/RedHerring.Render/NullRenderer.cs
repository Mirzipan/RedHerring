using RedHerring.Render.Features;
using Silk.NET.Maths;

namespace RedHerring.Render;

public class NullRenderer : Renderer
{
    public RenderFeatureCollection Features { get; } = new();
    public void AddFeature(ARenderFeature feature)
    {
    }

    public void Init()
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
}