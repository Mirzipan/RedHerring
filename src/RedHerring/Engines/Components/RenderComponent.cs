using RedHerring.Render;
using Silk.NET.Maths;

namespace RedHerring.Engines.Components;

public class RenderComponent : AnEngineComponent
{
    private Renderer _renderer = null!;
    
    protected override void Init()
    {
        _renderer = new Renderer(Context.View, Context.GraphicsBackend);
    }

    public bool BeginDraw() => _renderer.BeginDraw();
    public void Draw() => _renderer.Draw();
    public void EndDraw() => _renderer.EndDraw();
    public void Resize(Vector2D<int> size) => _renderer.Resize(size);
}