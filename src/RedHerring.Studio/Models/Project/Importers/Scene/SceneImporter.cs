namespace RedHerring.Studio.Models.Project.Importers;

[Importer("fbx", "obj")]
public class SceneImporter : AssetImporter<SceneIntermediate>
{
	public override SceneIntermediate Import(Stream stream)
	{
		return new SceneIntermediate();
	}
}