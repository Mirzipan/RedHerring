using RedHerring.Infusion.Attributes;
using RedHerring.Render;
using RedHerring.Render.Features;

namespace RedHerring.Core.Systems;

public sealed class GraphicsSystem : AnEngineSystem
{
    [Inject]
    private Renderer _renderer = null!;
    
    protected override void Init()
    {
    }

    protected override ValueTask<int> Load()
    {
        _renderer.Init();
        _renderer.Resize(Context.View.Size);
        return ValueTask.FromResult(0);
    }

    public void RegisterFeature(ARenderFeature feature)
    {
        _renderer.AddFeature(feature);
    }
}