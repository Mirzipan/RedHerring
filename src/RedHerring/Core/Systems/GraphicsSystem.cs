using RedHerring.Infusion.Attributes;
using RedHerring.Render;
using RedHerring.Render.Features;

namespace RedHerring.Core.Systems;

public sealed class GraphicsSystem : EngineSystem
{
    [Infuse]
    private RenderContext _context = null!;
    
    protected override void Init()
    {
    }

    protected override ValueTask<int> Load()
    {
        Renderer.Resize(Context.Window.Size);
        return ValueTask.FromResult(0);
    }

    protected override ValueTask<int> Unload()
    {
        return ValueTask.FromResult(0);
    }

    public void RegisterFeature(RenderFeature feature)
    {
        _context.AddFeature(feature);
    }
}