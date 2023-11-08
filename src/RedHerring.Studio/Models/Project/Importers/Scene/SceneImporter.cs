namespace RedHerring.Studio.Models.Project.Importers;

[Importer("fbx", "obj")]
public class SceneImporter : AssetImporter<SceneIntermediate, SceneImporterSettings>
{
	public override SceneIntermediate Import(Stream stream, SceneImporterSettings settings)
	{
		return new SceneIntermediate();
	}
}