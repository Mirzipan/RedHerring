using RedHerring.Infusion.Attributes;
using RedHerring.Render;
using RedHerring.Render.Features;

namespace RedHerring.Core.Systems;

public sealed class GraphicsSystem : EngineSystem
{
    [Infuse]
    private RendererContext _rendererContext = null!;
    
    protected override void Init()
    {
    }

    protected override ValueTask<int> Load()
    {
        _rendererContext.Init();
        _rendererContext.Resize(Context.Window.Size);
        return ValueTask.FromResult(0);
    }

    protected override ValueTask<int> Unload()
    {
        _rendererContext.Close();
        return ValueTask.FromResult(0);
    }

    public void RegisterFeature(RenderFeature feature)
    {
        _rendererContext.AddFeature(feature);
    }
}