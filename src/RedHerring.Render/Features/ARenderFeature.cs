using System.Reactive.Disposables;
using RedHerring.Alexandria.Components;
using RedHerring.Alexandria.Disposables;
using RedHerring.Render.Passes;
using Silk.NET.Maths;
using Veldrid;

namespace RedHerring.Render.Features;

public abstract class ARenderFeature : AComponent<RenderFeatureCollection>, IDisposerContainer, IDisposable
{
    private RenderFeatureCollection? _container;
    public override RenderFeatureCollection? Container => _container;
    CompositeDisposable IDisposerContainer.Disposer { get; set; } = new();
    public abstract int Priority { get; }

    public abstract void Init(GraphicsDevice device, CommandList commandList);
    
    public abstract void Update(GraphicsDevice device, CommandList commandList);
    
    public abstract void Render(GraphicsDevice device, CommandList commandList, RenderPass pass);

    public abstract void Resize(Vector2D<int> size);
    public abstract void Destroy();

    public void Dispose()
    {
        Destroy();
    }

    internal void SetContainer(RenderFeatureCollection container)
    {
        _container = container;
    }
}