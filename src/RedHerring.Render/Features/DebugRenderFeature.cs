using System.Numerics;
using RedHerring.Alexandria.Disposables;
using RedHerring.Numbers;
using RedHerring.Render.Assets;
using RedHerring.Render.Layouts;
using RedHerring.Render.Passes;
using RedHerring.Render.Shaders;
using Silk.NET.Maths;
using Veldrid;

namespace RedHerring.Render.Features;

public class DebugRenderFeature : RenderFeature, IDisposable
{
    private DebugCube _cube = null!;
    private ModelResources _modelResources;

    private Pipeline _pipeline = null!;

    private DeviceBuffer _projectionBuffer;
    private DeviceBuffer _viewBuffer;
    private DeviceBuffer _worldBuffer;

    private ResourceSet _projectionViewSet;
    private ResourceSet _worldSet;
    
    private Vector3 _position = Vector3.Zero;

    public override int Priority { get; } = -1000;

    #region Lifecycle
    
    public override void Init(GraphicsDevice device, CommandList commandList)
    {
        var factory = device.ResourceFactory;
        
        var projectionView = SharedLayout.CreateProjectionView(factory);
        var world = SharedLayout.CreateWorld(factory);
        var layouts = new[] { projectionView, world };
        
        var description = new GraphicsPipelineDescription
        {
            BlendState = BlendStateDescription.SingleOverrideBlend,
            DepthStencilState = new(
                true,
                true,
                ComparisonKind.LessEqual
            ),
            RasterizerState = new(
                FaceCullMode.Back,
                PolygonFillMode.Solid,
                FrontFace.Clockwise,
                true,
                false
            ),
            PrimitiveTopology = PrimitiveTopology.TriangleList,
            ResourceLayouts = layouts,
            ShaderSet = ShaderFactory.DefaultShaderSet(device),
            Outputs = device.SwapchainFramebuffer.OutputDescription,
        };

        _pipeline = factory.CreateGraphicsPipeline(description);
        _pipeline.DisposeWith(this);

        _position = Vector3Utils.Forward * 30;
        _cube = new DebugCube(device, factory);
        _modelResources = _cube.CreateResources(10f);
        _modelResources.DisposeWith(this);

        _projectionBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
        _viewBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));

        _worldBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));

        _projectionViewSet = factory.CreateResourceSet(new ResourceSetDescription(projectionView, _projectionBuffer, _viewBuffer));
        _worldSet = factory.CreateResourceSet(new ResourceSetDescription(world, _worldBuffer));
    }

    public override void Render(GraphicsDevice device, CommandList commandList, RenderEnvironment environment, RenderPass pass)
    {
        commandList.UpdateBuffer(_projectionBuffer, 0, environment.ProjectiomMatrix);
        commandList.UpdateBuffer(_viewBuffer, 0, environment.ViewMatrix);

        var world = Matrix4x4.CreateTranslation(_position);
        commandList.UpdateBuffer(_worldBuffer, 0, world);
        
        commandList.SetVertexBuffer(0, _modelResources.VertexBuffer);
        commandList.SetIndexBuffer(_modelResources.IndexBuffer, _modelResources.IndexFormat);
        commandList.SetPipeline(_pipeline);
        
        commandList.SetGraphicsResourceSet(0, _projectionViewSet);
        commandList.SetGraphicsResourceSet(1, _worldSet);
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