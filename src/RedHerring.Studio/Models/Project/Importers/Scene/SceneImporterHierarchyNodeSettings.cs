using Migration;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio;

[Serializable, SerializedClassId("4c7e4c73-e41f-4044-a733-ef7d6df38f41")]
public sealed class SceneImporterHierarchyNodeSettings
{
	[ReadOnlyInInspector] public string                                  Name;
	[ReadOnlyInInspector] public List<SceneImporterHierarchyNodeSettings>? Children  = null;
	[ReadOnlyInInspector] public List<int>                               Meshes    = new();
	[ReadOnlyInInspector] public List<int>                               Materials = new();

	public SceneImporterHierarchyNodeSettings(string name)
	{
		Name = name;
	}
}

#region Migration

[MigratableInterface(typeof(SceneImporterHierarchyNodeSettings))]
public interface ISceneImporterHierarchyNodeSettingsMigratable;

[Serializable, LatestVersion(typeof(SceneImporterHierarchyNodeSettings))]
public class SceneImporterHierarchyNodeSettings_000 : ISceneImporterHierarchyNodeSettingsMigratable
{
	public string Name;
	
	[MigrateField] public List<ISceneImporterHierarchyNodeSettingsMigratable>? Children;
	public                List<int>                                          Meshes;
	public                List<int>                                          Materials;
}
#endregion