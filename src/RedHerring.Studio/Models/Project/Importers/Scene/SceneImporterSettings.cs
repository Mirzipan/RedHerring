using Migration;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models.Project.Importers;

[Serializable, SerializedClassId("scene-importer-id")]
public sealed class SceneImporterSettings : ImporterSettings
{
	public bool Tangents = false;
	public List<bool> UVs = new();

	[ReadOnlyInInspector] public List<SceneImporterMeshSettings> Meshes = new();
}

#region Migration
[Serializable, LatestVersion(typeof(SceneImporterSettings))]
public class SceneImporterSettings_000 : ImporterSettings_000
{
	public bool                                       Tangents;
	public List<bool>                                 UVs;
	
	public List<ISceneImporterMeshSettingsMigratable> Meshes;
}
#endregion