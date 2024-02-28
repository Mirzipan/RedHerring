using RedHerring.Render.Models;
using Veldrid;

namespace RedHerring.Render;

public sealed class SharedMesh : IDisposable
{
	// geometry
	private DeviceBuffer?           _vertexBuffer = null;
	private DeviceBuffer?           _indexBuffer  = null;
	private IndexFormat             _indexFormat;
	private uint                     _indexCount;
	private VertexLayoutDescription _vertextLayoutDescription;

	public DeviceBuffer?           VertexBuffer            => _vertexBuffer;
	public DeviceBuffer?           IndexBuffer             => _indexBuffer;
	public IndexFormat             IndexFormat             => _indexFormat;
	public uint                     IndexCount              => _indexCount;
	public VertexLayoutDescription VertexLayoutDescription => _vertextLayoutDescription;
    
	public void Init(GraphicsDevice graphicsDevice, SceneMesh sceneMesh)
	{
		byte[] vertexData = sceneMesh.BuildVertexBufferData(out _vertextLayoutDescription);
		
		// vertex buffer
		BufferDescription vertexBufferDescription = new
		(
			(uint)(sceneMesh.VertexCount * sceneMesh.VertexSize),
			BufferUsage.VertexBuffer
		);
		_vertexBuffer = graphicsDevice.ResourceFactory.CreateBuffer(vertexBufferDescription);

		graphicsDevice.UpdateBuffer(_vertexBuffer, 0, vertexData);
		
		// index buffer
		if (sceneMesh.UShortIndices != null && sceneMesh.UShortIndices.Length > 0)
		{
			BufferDescription indexBufferDescription = new
			(
				(uint)(sizeof(ushort) * sceneMesh.UShortIndices.Length),
				BufferUsage.IndexBuffer
			);

			_indexBuffer = graphicsDevice.ResourceFactory.CreateBuffer(indexBufferDescription);
			_indexFormat = IndexFormat.UInt16;
			_indexCount  = (uint)sceneMesh.UShortIndices.Length;

			graphicsDevice.UpdateBuffer(_indexBuffer, 0, sceneMesh.UShortIndices);
		}
		else if (sceneMesh.UIntIndices != null && sceneMesh.UIntIndices.Length > 0)
		{
			BufferDescription indexBufferDescription = new
			(
				(uint)(sizeof(ushort) * sceneMesh.UIntIndices.Length),
				BufferUsage.IndexBuffer
			);

			_indexBuffer = graphicsDevice.ResourceFactory.CreateBuffer(indexBufferDescription);
			_indexFormat = IndexFormat.UInt32;
			_indexCount  = (uint)sceneMesh.UIntIndices.Length;

			graphicsDevice.UpdateBuffer(_indexBuffer, 0, sceneMesh.UIntIndices);
		}
	}
	
	public void Dispose()
	{
		_vertexBuffer?.Dispose();
		_indexBuffer?.Dispose();
	}
}