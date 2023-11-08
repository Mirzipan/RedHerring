using Assimp;
using Assimp.Configs;

namespace RedHerring.Studio.Models.Project.Importers;

[Importer(".fbx", ".obj")]
public class SceneImporter : AssetImporter<SceneIntermediate, SceneImporterSettings>
{
	protected override SceneIntermediate Import(Stream stream, SceneImporterSettings settings)
	{
		AssimpContext context = new();
		context.SetConfig(new NormalSmoothingAngleConfig(66.0f)); // just for testing

		Scene scene = context.ImportFileFromStream(stream,
			PostProcessSteps.Triangulate
		);
		
		return new SceneIntermediate(scene);
	}
}