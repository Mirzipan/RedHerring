using RedHerring.Numbers;

namespace RedHerring.Render.Models;

[Serializable]
public sealed class SceneMeshVertexColorChannel
{
	public const int ItemSizeInBytes = Color4.SizeInBytes;

	public List<Color4> Colors = new();
}