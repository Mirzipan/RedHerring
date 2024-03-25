using Migration;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio;

[Serializable, SerializedClassId("8589a74d-43fc-4c5d-8537-87681debc89e")]
public sealed class SceneImporterMaterialSettings
{
	[ReadOnlyInInspector] public string Name;

	public SceneImporterMaterialSettings(string name)
	{
		Name = name;
	}
}

#region Migration

[MigratableInterface(typeof(SceneImporterMaterialSettings))]
public interface ISceneImporterMaterialSettingsMigratable;

[Serializable, LatestVersion(typeof(SceneImporterMaterialSettings))]
public class SceneImporterMaterialSettings_000 : ISceneImporterMaterialSettingsMigratable
{
	public string Name;
}
#endregion