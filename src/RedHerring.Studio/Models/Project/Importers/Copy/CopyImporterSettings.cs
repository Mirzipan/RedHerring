using Migration;

namespace RedHerring.Studio;

[Serializable, SerializedClassId("d6112cef-1810-416d-b26b-9e9b75529b64")]
public sealed class CopyImporterSettings : ImporterSettings
{
}

#region Migration
[MigratableInterface(typeof(CopyImporterSettings))]
public interface ICopyImporterSettingsMigratable : IImporterSettingsMigratable;

[Serializable, LatestVersion(typeof(CopyImporterSettings))]
public class CopyImporterSettings_000 : ImporterSettings_000, ICopyImporterSettingsMigratable
{
}
#endregion