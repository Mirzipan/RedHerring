using RedHerring.Numbers;

namespace RedHerring.Render.Models;

[Serializable]
public sealed class MeshVertexColorChannel
{
	public List<Color4> Colors = new();
}