using System.Text;
using Veldrid;
using Veldrid.SPIRV;

namespace RedHerring.Render.Shaders;

public static class ShaderFactory
{
    private const string VertexCode = @"
#version 450

layout(set = 0, binding = 0) uniform ProjectionBuffer
{
    mat4 Projection;
};
layout(set = 0, binding = 1) uniform ViewBuffer
{
    mat4 View;
};

layout(location = 0) in vec3 Position;
layout(location = 1) in vec4 Color;

layout(location = 0) out vec4 fsin_Color;

void main()
{
    mat4 vp = Projection * View;
    gl_Position = vp * vec4(Position, 1);
    //gl_Position = vec4(Position, 1);
    fsin_Color = Color;
}";

    private const string FragmentCode = @"
#version 450

layout(location = 0) in vec4 fsin_Color;
layout(location = 0) out vec4 fsout_Color;

void main()
{
    fsout_Color = fsin_Color;
}";

    public static VertexLayoutDescription[] DefaultVertexLayouts()
    {
        return new[]
        {
            new VertexLayoutDescription
            (
                new VertexElementDescription
                    ("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3),
                new VertexElementDescription
                    ("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4)
            ),
        };
    }

    public static Shader[] DefaultShaders(GraphicsDevice graphicsDevice)
    {
        var vertexShaderDesc = new ShaderDescription
        (
            ShaderStages.Vertex,
            Encoding.UTF8.GetBytes(VertexCode),
            "main"
        );
        var fragmentShaderDesc = new ShaderDescription
        (
            ShaderStages.Fragment,
            Encoding.UTF8.GetBytes(FragmentCode),
            "main"
        );

        return graphicsDevice.ResourceFactory.CreateFromSpirv(vertexShaderDesc, fragmentShaderDesc);
    }

    public static ShaderSetDescription DefaultShaderSet(GraphicsDevice graphicsDevice)
    {
        return new ShaderSetDescription
        {
            VertexLayouts = DefaultVertexLayouts(),
            Shaders = DefaultShaders(graphicsDevice),
        };
    }
}