using Migration;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Import;

[Serializable, SerializedClassId("7fcc683f-48c4-4acd-a7e1-2ac82b097b7d")]
public sealed class AssimpSceneImporterSettings : ImporterSettings
{
    public float NormalSmoothingAngle = 15f;
    public bool CompensateFBXScale = false;
    public bool ImportAnimations = true;
    public float AnimationDeviation = float.Epsilon;

    [ReadOnlyInInspector]
    public List<AssimpSceneImporterMeshSettings> Meshes = new();
    [ReadOnlyInInspector]
    public List<AssimpSceneImporterMaterialSettings> Materials = new();
    [ReadOnlyInInspector]
    public List<SceneImporterAnimationSettings> Animations = new();
    [ReadOnlyInInspector]
    public AssimpSceneImporterHierarchyNodeSettings Root = new("Root");
}

#region Migration

[MigratableInterface(typeof(AssimpSceneImporterSettings))]
public interface AssimpSceneImporterSettingsMigratable : IImporterSettingsMigratable;

[Serializable, LatestVersion(typeof(AssimpSceneImporterSettings))]
public class AssimpSceneImporterSettings_000 : ImporterSettings_000, AssimpSceneImporterSettingsMigratable
{
    public float SmoothingAngle;
    public bool CompensateFBXScale;
    public bool ImportAnimations;
    public float AnimationDeviation;

    [MigrateField]
    public List<AssimpSceneImporterMeshSettingsMigratable> Meshes;
    [MigrateField]
    public List<AssimpSceneImporterMaterialSettingsMigratable> Materials;
    [MigrateField]
    public List<SceneImporterAnimationSettingsMigratable> Animations;

    public AssimpSceneImporterHierarchyNodeSettingsMigratable Root;
}

#endregion