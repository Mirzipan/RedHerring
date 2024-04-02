using Migration;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Import;

[Serializable, SerializedClassId("cc1cd360-ca9f-442b-9f7c-555e1fc42d62")]
public sealed class AssimpSceneImporterHierarchyNodeSettings
{
    [ReadOnlyInInspector] 
    public string Name;
    [ReadOnlyInInspector] 
    public List<AssimpSceneImporterHierarchyNodeSettings>? Children = null;
    [ReadOnlyInInspector] 
    public List<int> Meshes = new();
    [ReadOnlyInInspector] 
    public List<int> Materials = new();

    public AssimpSceneImporterHierarchyNodeSettings(string name)
    {
        Name = name;
    }
}

#region Migration

[MigratableInterface(typeof(AssimpSceneImporterHierarchyNodeSettings))]
public interface AssimpSceneImporterHierarchyNodeSettingsMigratable;

[Serializable, LatestVersion(typeof(AssimpSceneImporterHierarchyNodeSettings))]
public class AssimpSceneImporterHierarchyNodeSettings_000 : AssimpSceneImporterHierarchyNodeSettingsMigratable
{
    public string Name;

    [MigrateField] 
    public List<AssimpSceneImporterHierarchyNodeSettingsMigratable>? Children;
    public List<int> Meshes;
    public List<int> Materials;
}

#endregion