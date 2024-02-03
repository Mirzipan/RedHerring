using Migration;
using RedHerring.Studio.Models.Project.FileSystem;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models.Project.Importers;

[Serializable, SerializedClassId("scene-importer-id")]
public sealed class SceneImporterSettings : ImporterSettings
{
	public override ProjectNodeType NodeType => ProjectNodeType.AssetMesh;

	public float NormalSmoothingAngle = 15f;
	
	[ReadOnlyInInspector] public List<SceneImporterMeshSettings> Meshes = new();
}

#region Migration
[MigratableInterface(typeof(SceneImporterSettings))]
public interface ISceneImporterSettingsMigratable : IImporterSettingsMigratable;

[Serializable, LatestVersion(typeof(SceneImporterSettings))]
public class SceneImporterSettings_000 : ImporterSettings_000, ISceneImporterSettingsMigratable
{
	public float NormalSmoothingAngle;
	
	public List<ISceneImporterMeshSettingsMigratable> Meshes;
}
#endregion