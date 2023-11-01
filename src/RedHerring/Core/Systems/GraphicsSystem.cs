using RedHerring.Render;
using RedHerring.Render.Features;
using Silk.NET.Maths;

namespace RedHerring.Core.Systems;

public sealed class GraphicsSystem : AnEngineSystem
{
    private Renderer _renderer = null!;
    
    protected override void Init()
    {
        _renderer = new Renderer(Context.View, Context.GraphicsBackend, Context.UseSeparateRenderThread);
    }

    protected override ValueTask<int> Load()
    {
        _renderer.Init();
        _renderer.Resize(Context.View.Size);
        return ValueTask.FromResult(0);
    }

    public bool BeginDraw() => _renderer.BeginDraw();
    public void Draw() => _renderer.Draw();
    public void EndDraw() => _renderer.EndDraw();
    public void Resize(Vector2D<int> size) => _renderer.Resize(size);

    public void RegisterFeature(ARenderFeature feature)
    {
        _renderer.Features.Add(feature);
        feature.Init(_renderer.Device, _renderer.CommandList);
        feature.Resize(Context.View.Size);
    }
}