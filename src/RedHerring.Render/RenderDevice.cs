using RedHerring.Render.Features;
using Silk.NET.Maths;

namespace RedHerring.Render;

public interface RenderDevice
{
    void Init(RenderFeature feature);
    void Init();
    void Close();
    bool BeginDraw();
    void Draw(RenderContext context);
    void EndDraw();
    void Resize(Vector2D<int> size);

    void ReloadShaders(RenderFeatureCollection features);
    void Init(RenderFeatureCollection features);
}