using Migration;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio;

[Serializable, SerializedClassId("fe818723-a461-4db4-9b0f-52f29b90139d")]
public class SceneImporterAnimationSettings
{
    [ReadOnlyInInspector]
    public string Name;
    public bool Import = true;

    public SceneImporterAnimationSettings(string name)
    {
        Name = name;
    }
}

#region Migration

[MigratableInterface(typeof(SceneImporterAnimationSettings))]
public interface SceneImporterAnimationSettingsMigratable;

[Serializable, LatestVersion(typeof(SceneImporterAnimationSettings))]
public class SceneImporterAnimationSettings_000 : SceneImporterAnimationSettingsMigratable
{
    public string Name;
    public bool Import = true;
}
#endregion