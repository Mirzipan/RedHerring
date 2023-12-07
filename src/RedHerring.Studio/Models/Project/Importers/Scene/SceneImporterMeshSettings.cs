using Migration;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models.Project.Importers;

[Serializable, SerializedClassId("scene-importer-mesh-id")]
public sealed class SceneImporterMeshSettings
{
	[ReadOnlyInInspector] public string Name;
	public bool Import = true;

	public SceneImporterMeshSettings(string name)
	{
		Name = name;
	}
}

#region Migration
[MigratableInterface(typeof(SceneImporterSettings))]
public interface ISceneImporterMeshSettingsMigratable
{
}

[Serializable, LatestVersion(typeof(SceneImporterMeshSettings))]
public class SceneImporterMeshSettings_000 : ISceneImporterMeshSettingsMigratable
{
	public string Name;
	public bool Import = true;
}
#endregion