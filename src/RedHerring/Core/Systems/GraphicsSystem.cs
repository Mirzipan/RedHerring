using RedHerring.Infusion.Attributes;
using RedHerring.Render;
using RedHerring.Render.Features;

namespace RedHerring.Core.Systems;

public sealed class GraphicsSystem : EngineSystem
{
    [Infuse]
    private Renderer _renderer = null!;
    
    protected override void Init()
    {
    }

    protected override ValueTask<int> Load()
    {
        _renderer.Init();
        _renderer.Resize(Context.Window.Size);
        return ValueTask.FromResult(0);
    }

    protected override ValueTask<int> Unload()
    {
        _renderer.Close();
        return ValueTask.FromResult(0);
    }

    public void RegisterFeature(RenderFeature feature)
    {
        _renderer.AddFeature(feature);
    }
}