using RedHerring.Render.Shaders;
using Veldrid;

namespace RedHerring.Render;

public static class PipelineFactory
{
    public static Pipeline Default(GraphicsDevice graphicsDevice)
    {
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
            PrimitiveTopology = PrimitiveTopology.TriangleStrip,
            ResourceLayouts = Array.Empty<ResourceLayout>(),
            ShaderSet = ShaderFactory.DefaultShaderSet(graphicsDevice),
            Outputs = graphicsDevice.SwapchainFramebuffer.OutputDescription,
        };

        return graphicsDevice.ResourceFactory.CreateGraphicsPipeline(description);
    }
}