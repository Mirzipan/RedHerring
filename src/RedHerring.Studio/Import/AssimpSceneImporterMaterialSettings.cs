using Migration;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Import;

[Serializable, SerializedClassId("12341789-f065-4a01-acad-6306fb2ad730")]
public sealed class AssimpSceneImporterMaterialSettings
{
	[ReadOnlyInInspector] 
	public string Name;

	public AssimpSceneImporterMaterialSettings(string name)
	{
		Name = name;
	}
}

#region Migration

[MigratableInterface(typeof(AssimpSceneImporterMaterialSettings))]
public interface AssimpSceneImporterMaterialSettingsMigratable;

[Serializable, LatestVersion(typeof(AssimpSceneImporterMaterialSettings))]
public class AssimpSceneImporterMaterialSettings_000 : AssimpSceneImporterMaterialSettingsMigratable
{
	public string Name;
}
#endregion