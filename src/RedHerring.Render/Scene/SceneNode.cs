using Silk.NET.Maths;

namespace RedHerring.Render.Models;

[Serializable]
public sealed class SceneNode
{
	public string            Name;
	public Vector3D<float>   Translation;
	public Quaternion<float> Rotation;
	public Vector3D<float>   Scale;

	public List<int>? MeshIndices;

	public List<SceneNode>? Children;
}