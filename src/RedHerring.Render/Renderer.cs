using RedHerring.Render.Features;
using Silk.NET.Maths;

namespace RedHerring.Render;

public interface Renderer
{
    RenderFeatureCollection Features { get; }
    void AddFeature(ARenderFeature feature);
    void Init();
    bool BeginDraw();
    void Draw();
    void EndDraw();
    void Resize(Vector2D<int> size);
}