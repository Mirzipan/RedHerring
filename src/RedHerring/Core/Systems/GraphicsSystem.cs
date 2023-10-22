using RedHerring.Render;
using Silk.NET.Maths;
using Veldrid;

namespace RedHerring.Core.Systems;

public sealed class GraphicsSystem : AnEngineSystem
{
    private Renderer _renderer = null!;

    public GraphicsDevice Device => _renderer.Device;
    public CommandList CommandList => _renderer.CommandList;
    
    protected override void Init()
    {
        _renderer = new Renderer(Context.View, Context.GraphicsBackend, Context.UseSeparateRenderThread);
    }

    public bool BeginDraw() => _renderer.BeginDraw();
    public void Draw() => _renderer.Draw();
    public void EndDraw() => _renderer.EndDraw();
    public void Resize(Vector2D<int> size) => _renderer.Resize(size);
}