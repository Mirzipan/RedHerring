using Migration;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio;

[Serializable, SerializedClassId("d86709f5-c520-4c47-b4da-8278fff56eb6")]
public sealed class SceneImporterMeshSettings
{
	[ReadOnlyInInspector] public string Name;
	[ReadOnlyInInspector] public int    MaterialIndex;
	public                       bool   Import = true;

	public SceneImporterMeshSettings(string name, int materialIndex)
	{
		Name          = name;
		MaterialIndex = materialIndex;
	}
}

#region Migration

[MigratableInterface(typeof(SceneImporterMeshSettings))]
public interface ISceneImporterMeshSettingsMigratable;

[Serializable, LatestVersion(typeof(SceneImporterMeshSettings))]
public class SceneImporterMeshSettings_000 : ISceneImporterMeshSettingsMigratable
{
	public string Name;
	public int    MaterialIndex;
	public bool   Import = true;
}
#endregion