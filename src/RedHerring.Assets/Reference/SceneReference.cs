namespace RedHerring.Assets;

[Serializable]
public sealed class SceneReference : Reference
{
	//public          Mesh          ReferencedMesh;

	public SceneReference(string path) : base(path)
	{
	}
}