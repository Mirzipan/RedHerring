using Migration;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Import;

[Serializable, SerializedClassId("4a6bcb48-9bf0-4672-b133-824f5180e915")]
public sealed class AssimpSceneImporterMeshSettings
{
    [ReadOnlyInInspector] 
    public string Name;
    [ReadOnlyInInspector] 
    public int MaterialIndex;
    public bool Import = true;

    public AssimpSceneImporterMeshSettings(string name, int materialIndex)
    {
        Name = name;
        MaterialIndex = materialIndex;
    }
}

#region Migration

[MigratableInterface(typeof(AssimpSceneImporterMeshSettings))]
public interface AssimpSceneImporterMeshSettingsMigratable;

[Serializable, LatestVersion(typeof(AssimpSceneImporterMeshSettings))]
public class AssimpSceneImporterMeshSettings_000 : AssimpSceneImporterMeshSettingsMigratable
{
    public string Name;
    public int MaterialIndex;
    public bool Import = true;
}

#endregion