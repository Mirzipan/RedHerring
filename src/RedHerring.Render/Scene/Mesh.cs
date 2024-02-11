using Silk.NET.Maths;
using Vortice.Mathematics;

namespace RedHerring.Render.Models;

[Serializable]
public sealed class Mesh
{
	public string Name;
	public int    MaterialIndex; // TODO

	public BoundingBox    BoundingBox;
	public BoundingSphere BoundingSphere;

	public List<Vector3D<float>>?              Positions;
	public List<Vector3D<float>>?              Normals;
	public List<Vector3D<float>>?              Tangents;
	public List<Vector3D<float>>?              BiTangents;
	public List<ushort>?                       UShortIndices;
	public List<uint>?                         UIntIndices;
	public List<MeshTextureCoordinateChannel>? TextureCoordinateChannels;
	public List<MeshVertexColorChannel>?       VertexColorChannels;
}