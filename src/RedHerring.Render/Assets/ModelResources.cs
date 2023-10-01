using Veldrid;

namespace RedHerring.Render.Assets;

public struct ModelResources : IDisposable
{
    public readonly DeviceBuffer VertexBuffer;
    public readonly DeviceBuffer IndexBuffer;
    public readonly IndexFormat IndexFormat;
    public readonly uint VertexCount;
    public readonly uint IndexCount;

    public ModelResources(DeviceBuffer vertexBuffer, DeviceBuffer indexBuffer, IndexFormat indexFormat, uint vertexCount, uint indexCount)
    {
        VertexBuffer = vertexBuffer;
        IndexBuffer = indexBuffer;
        IndexFormat = indexFormat;
        VertexCount = vertexCount;
        IndexCount = indexCount;
    }

    public void Dispose()
    {
        VertexBuffer.Dispose();
        IndexBuffer.Dispose();
    }
}