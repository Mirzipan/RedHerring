using RedHerring.Render.Assets;
using Veldrid;

namespace RedHerring.Render.Components;

public class DebugRenderComponent : ARendererComponent, IDisposable
{
    private DebugQuad _quad;
    private ModelResources _modelResources;

    private Pipeline _pipeline;
    
    public override int Priority { get; set; } = 0;
    
    #region Lifecycle

    public DebugRenderComponent(GraphicsDevice device, ResourceFactory factory)
    {
        _pipeline = PipelineFactory.Default(device);
        
        _quad = new DebugQuad(device, factory);
        _modelResources = _quad.CreateResources();
    }

    public override void Draw(CommandList commandList)
    {
        commandList.SetVertexBuffer(0, _modelResources.VertexBuffer);
        commandList.SetIndexBuffer(_modelResources.IndexBuffer, _modelResources.IndexFormat);
        commandList.SetPipeline(_pipeline);
        commandList.DrawIndexed(_modelResources.IndexCount, 1, 0, 0, 0);
    }

    public void Dispose()
    {
        _pipeline.Dispose();
        _modelResources.Dispose();
    }

    #endregion Lifecycle
}