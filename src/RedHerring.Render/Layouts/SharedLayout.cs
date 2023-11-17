using Veldrid;
using Layout = Veldrid.ResourceLayoutDescription;
using Element = Veldrid.ResourceLayoutElementDescription;

namespace RedHerring.Render.Layouts;

internal static class SharedLayout
{
    public static ResourceLayout? CreateProjectionView(ResourceFactory factory)
    {
        var projection = new Element("ProjectionBuffer", ResourceKind.UniformBuffer, ShaderStages.Vertex);
        var view = new Element("ViewBuffer", ResourceKind.UniformBuffer, ShaderStages.Vertex);

        var description = new Layout
        {
            Elements = new[] { projection, view },
        };

        var result = factory.CreateResourceLayout(description);
        return result;
    }

    public static ResourceLayout? CreateWorldTexture(ResourceFactory factory)
    {
        var world = new Element("WorldBuffer", ResourceKind.UniformBuffer, ShaderStages.Vertex);
        var texture = new Element("SurfaceTexture", ResourceKind.TextureReadOnly, ShaderStages.Fragment);
        var sampler = new Element("SurfaceSampler", ResourceKind.Sampler, ShaderStages.Fragment);

        var description = new Layout
        {
            Elements = new[] { world, texture, sampler },
        };
        
        var result = factory.CreateResourceLayout(description);
        return result;
    }
}