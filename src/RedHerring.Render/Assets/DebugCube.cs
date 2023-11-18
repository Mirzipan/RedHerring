using System.Numerics;
using Veldrid;
using Veldrid.Utilities;

namespace RedHerring.Render.Assets;

public class DebugCube : IDisposable
{
    private static readonly RgbaFloat FrontColor = RgbaFloat.Blue;
    private static readonly RgbaFloat BackColor = RgbaFloat.Blue;
    
    private static readonly RgbaFloat TopColor = RgbaFloat.Green;
    private static readonly RgbaFloat BottomColor = RgbaFloat.Green;

    private static readonly RgbaFloat RightColor = RgbaFloat.Red;
    private static readonly RgbaFloat LeftColor = RgbaFloat.Red;

    private static VertexPositionColor[] _vertices =
    {
        // Front
        new(new (-0.5f, +0.5f, +0.5f), FrontColor),
        new(new (+0.5f, +0.5f, +0.5f), FrontColor),
        new(new (-0.5f, -0.5f, +0.5f), FrontColor),
        new(new (+0.5f, -0.5f, +0.5f), FrontColor),
        // Top
        new(new (+0.5f, +0.5f, -0.5f), TopColor),
        new(new (+0.5f, +0.5f, +0.5f), TopColor),
        new(new (-0.5f, +0.5f, -0.5f), TopColor),
        new(new (-0.5f, +0.5f, +0.5f), TopColor),
        // Right
        new(new (+0.5f, -0.5f, +0.5f), RightColor),
        new(new (+0.5f, +0.5f, +0.5f), RightColor),
        new(new (+0.5f, -0.5f, -0.5f), RightColor),
        new(new (+0.5f, +0.5f, -0.5f), RightColor),
        // Back
        new(new (+0.5f, -0.5f, -0.5f), BackColor),
        new(new (-0.5f, -0.5f, -0.5f), BackColor),
        new(new (+0.5f, +0.5f, -0.5f), BackColor),
        new(new (-0.5f, +0.5f, -0.5f), BackColor),
        // Bottom
        new(new (-0.5f, -0.5f, +0.5f), BottomColor),
        new(new (-0.5f, -0.5f, -0.5f), BottomColor),
        new(new (+0.5f, -0.5f, +0.5f), BottomColor),
        new(new (+0.5f, -0.5f, -0.5f), BottomColor),
        // Left
        new(new (-0.5f, +0.5f, -0.5f), LeftColor),
        new(new (-0.5f, -0.5f, -0.5f), LeftColor),
        new(new (-0.5f, +0.5f, +0.5f), LeftColor),
        new(new (-0.5f, -0.5f, +0.5f), LeftColor),
    };
    private static ushort[] _indices =
    {
        // Front
        00, 01, 02, 00, 02, 03,
        // Top
        04, 05, 06, 04, 06, 07,
        // Right
        08, 09, 10, 08, 10, 11,
        // Back
        12, 13, 14, 12, 14, 15,
        // Bottom
        16, 17, 18, 16, 18, 19,
        // Left
        20, 21, 22, 20, 22, 23,
    };

    private GraphicsDevice _device;
    private ResourceFactory _factory;

    public DebugCube(GraphicsDevice device, ResourceFactory factory)
    {
        _device = device;
        _factory = factory;
    }

    public ModelResources CreateResources(Vector3 position, float size)
    {
        uint vertexCount = (uint)_vertices.Length;
        var vertexBuffer = _factory.CreateBuffer(new BufferDescription
        {
            SizeInBytes = vertexCount * VertexPositionColor.SizeInBytes,
            Usage = BufferUsage.VertexBuffer,
        });

        var vertices = new VertexPositionColor[_vertices.Length];
        for (int i = 0; i < _vertices.Length; i++)
        {
            var entry = _vertices[i];
            entry.Position = position * entry.Position * size;
            vertices[i] = entry;
        }
        
        _device.UpdateBuffer(vertexBuffer, 0, vertices);
        
        uint indicesCount = (uint)_indices.Length;
        var indexBuffer = _factory.CreateBuffer(new BufferDescription
        {
            SizeInBytes = indicesCount * sizeof(ushort),
            Usage = BufferUsage.IndexBuffer,
        });
        _device.UpdateBuffer(indexBuffer, 0, _indices);
        
        return new ModelResources(vertexBuffer, indexBuffer, IndexFormat.UInt16, vertexCount, indicesCount);
    }

    public void Dispose()
    {
        _device = null;
        _factory = null;
    }
}