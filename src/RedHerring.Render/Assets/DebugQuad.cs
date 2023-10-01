using Veldrid;

namespace RedHerring.Render.Assets;

public class DebugQuad : IDisposable
{
    private static VertexPositionColor[] _vertices =
    {
        new(new (-0.75f, 0.75f), RgbaFloat.Red),
        new(new (0.75f, 0.75f), RgbaFloat.Green),
        new(new (-0.75f, -0.75f), RgbaFloat.Blue),
        new(new (0.75f, -0.75f), RgbaFloat.Yellow),
    };
    private static ushort[] _indices = { 0, 1, 2, 3 };

    private GraphicsDevice _device;
    private ResourceFactory _factory;

    public DebugQuad(GraphicsDevice device, ResourceFactory factory)
    {
        _device = device;
        _factory = factory;
    }

    public ModelResources CreateResources()
    {
        uint vertexCount = (uint)_vertices.Length;
        var vertexBuffer = _factory.CreateBuffer(new BufferDescription
        {
            SizeInBytes = vertexCount * VertexPositionColor.SizeInBytes,
            Usage = BufferUsage.VertexBuffer,
        });
        _device.UpdateBuffer(vertexBuffer, 0, _vertices);
        
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