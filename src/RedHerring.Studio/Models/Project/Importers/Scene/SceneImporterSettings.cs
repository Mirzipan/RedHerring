using Migration;

namespace RedHerring.Studio.Models.Project.Importers;

[Serializable, SerializedClassId("scene-importer-id")]
public sealed class SceneImporterSettings : ImporterSettings
{
	public bool Tangents = false;
	public List<bool> UVs = new();
}

#region Migration
[Serializable, LatestVersion(typeof(SceneImporterSettings))]
public class SceneImporterSettings_000 : ImporterSettings_000
{
	public bool       Tangents;
	public List<bool> UVs;
}
#endregion