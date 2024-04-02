using Migration;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Import;

[Serializable, SerializedClassId("94b3ed97-377b-4ea0-8f69-b2480d756892")]
public class AssimpSceneImporterAnimationSettings
{
    [ReadOnlyInInspector]
    public string Name;
    public bool Import = true;

    public AssimpSceneImporterAnimationSettings(string name)
    {
        Name = name;
    }
}

#region Migration

[MigratableInterface(typeof(SceneImporterAnimationSettings))]
public interface AssimpSceneImporterAnimationSettingsMigratable;

[Serializable, LatestVersion(typeof(SceneImporterAnimationSettings))]
public class AssimpSceneImporterAnimationSettings_000 : AssimpSceneImporterAnimationSettingsMigratable
{
    public string Name;
    public bool Import = true;
}
#endregion