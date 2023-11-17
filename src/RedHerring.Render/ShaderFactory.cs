using System.Text;
using Veldrid;
using Veldrid.SPIRV;

namespace RedHerring.Render.Shaders;

public static class ShaderFactory
{
    private const string VertexCode = @"
#version 450

layout(binding = 0) uniform MatrixBlock
{
    mat4 ProjectionMatrix;
    mat4 ViewMatrix;
} matrices;

layout(location = 0) in vec2 Position;
layout(location = 1) in vec4 Color;

layout(location = 0) out vec4 fsin_Color;

void main()
{
    mat4 vp = matrices.ProjectionMatrix * matrices.ViewMatrix;
    gl_Position = vp * vec4(Position, 0, 1);
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
                    ("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
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