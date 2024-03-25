using System.Numerics;

namespace RedHerring.Render.Models;

[Serializable]
public sealed class SceneNode
{
	public string     Name;
	public Vector3    Translation;
	public Quaternion Rotation;
	public Vector3    Scale;

	public List<int>? MeshIndices;

	public List<SceneNode>? Children;
}