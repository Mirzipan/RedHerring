using Silk.NET.Maths;

namespace RedHerring.Render.Models;

[Serializable]
public sealed class SceneMeshTextureCoordinateChannel
{
	// one of these
	public List<Vector2D<float>>? UV;
	public List<Vector3D<float>>? UVW;

	public int ItemSizeInBytes => UV is not null ? 2 * sizeof(float) : 3 * sizeof(float);
}