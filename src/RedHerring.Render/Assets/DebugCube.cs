using RedHerring.Numbers;
using Veldrid;

namespace RedHerring.Render.Assets;

public class DebugCube : IDisposable
{
    // Colors were chosen, so that faces on the positive side of coordinate axis is always lighter than the negative side
    
    private static readonly Color4 FrontColor = Color4.LightBlue;
    private static readonly Color4 BackColor = Color4.DarkBlue;
    
    private static readonly Color4 TopColor = Color4.LightGreen;
    private static readonly Color4 BottomColor = Color4.DarkGreen;

    private static readonly Color4 RightColor = Color4.LightCoral;
    private static readonly Color4 LeftColor = Color4.DarkRed;

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
        00, 01, 02, 01, 03, 02, 
        // Top
        04, 05, 06, 05, 07, 06, 
        // Right
        08, 09, 10, 09, 11, 10, 
        // Back
        12, 14, 13, 13, 14, 15, 
        // Bottom
        16, 18, 17, 17, 18, 19, 
        // Left
        20, 22, 21, 21, 22, 23, 
    };

    private GraphicsDevice _device;
    private ResourceFactory _factory;

    public DebugCube(GraphicsDevice device, ResourceFactory factory)
    {
        _device = device;
        _factory = factory;
    }

    public ModelResources CreateResources(float size)
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
            entry.Position = entry.Position * size;
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