using RedHerring.Alexandria.Disposables;
using RedHerring.Render.Assets;
using RedHerring.Render.Passes;
using Silk.NET.Maths;
using Veldrid;

namespace RedHerring.Render.Features;

public class DebugRenderFeature : ARenderFeature, IDisposable
{
    private DebugQuad _quad = null!;
    private ModelResources _modelResources;

    private Pipeline _pipeline = null!;
    
    public override int Priority { get; } = -1000;

    #region Lifecycle
    
    public override void Init(GraphicsDevice device, CommandList commandList)
    {
        _pipeline = PipelineFactory.Default(device);
        _pipeline.DisposeWith(this);

        _quad = new DebugQuad(device, device.ResourceFactory);
        _modelResources = _quad.CreateResources();
        _modelResources.DisposeWith(this);
    }

    public override void Render(GraphicsDevice device, CommandList commandList, RenderPass pass)
    {
        commandList.SetVertexBuffer(0, _modelResources.VertexBuffer);
        commandList.SetIndexBuffer(_modelResources.IndexBuffer, _modelResources.IndexFormat);
        commandList.SetPipeline(_pipeline);
        commandList.DrawIndexed(_modelResources.IndexCount, 1, 0, 0, 0);
    }

    public override void Resize(Vector2D<int> size)
    {
    }

    public override void Update(GraphicsDevice device, CommandList commandList)
    {
    }

    public override void Destroy()
    {
    }

    #endregion Lifecycle
}