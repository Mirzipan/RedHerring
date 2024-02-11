using Silk.NET.Maths;

namespace RedHerring.Render.Models;

[Serializable]
public sealed class MeshTextureCoordinateChannel
{
	// one of these
	public List<Vector2D<float>>? UV;
	public List<Vector3D<float>>? UVW;
}