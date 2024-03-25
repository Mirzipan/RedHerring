using System.Numerics;

namespace RedHerring.Render.Models;

[Serializable]
public sealed class SceneMeshTextureCoordinateChannel
{
	// one of these
	public List<Vector2>? UV;
	public List<Vector3>? UVW;

	public int ItemSizeInBytes => UV is not null ? 2 * sizeof(float) : 3 * sizeof(float);
}