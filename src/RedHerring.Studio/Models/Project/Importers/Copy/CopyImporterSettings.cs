using Migration;
using RedHerring.Studio.Models.Project.FileSystem;

namespace RedHerring.Studio.Models.Project.Importers;

[Serializable, SerializedClassId("copy-importer-id")]
public sealed class CopyImporterSettings : ImporterSettings
{
	public override ProjectNodeType NodeType => ProjectNodeType.AssetBinary;
}

#region Migration
[MigratableInterface(typeof(CopyImporterSettings))]
public interface ICopyImporterSettingsMigratable : IImporterSettingsMigratable;

[Serializable, LatestVersion(typeof(CopyImporterSettings))]
public class CopyImporterSettings_000 : ImporterSettings_000, ICopyImporterSettingsMigratable
{
}
#endregion