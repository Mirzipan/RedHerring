using Migration;

namespace RedHerring.Studio.Models.Project.Importers;

[Serializable, SerializedClassId("scene-importer-id")]
public sealed class SceneImporterSettings : AnImporterSettings
{
	public bool Tangents = false; // just for debug
	public List<bool> UVs = new();
}

#region Migration
[Serializable, LatestVersion(typeof(SceneImporterSettings))]
public class SceneImporterSettings_000 : AnImporterSettings_000
{
	public bool       Tangents;
	public List<bool> UVs;
}
#endregion