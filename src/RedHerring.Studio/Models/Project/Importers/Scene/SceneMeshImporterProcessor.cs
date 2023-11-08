namespace RedHerring.Studio.Models.Project.Importers;

[ImporterProcessor(typeof(SceneIntermediate))]
public sealed class SceneMeshImporterProcessor : AssetImporterProcessor<SceneIntermediate, SceneImporterSettings>
{
	protected override void Process(SceneIntermediate? intermediate, SceneImporterSettings settings, string resourcePath)
	{
		if (intermediate == null)
		{
			return;
		}

		
		
		//Directory.CreateDirectory(resourcePath);
		//File.WriteAllBytes(resourcePath, intermediate.Data);
	}
}