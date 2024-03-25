using Migration;
namespace RedHerring.Studio;

[Serializable, SerializedClassId("1fbf8638-5ae8-49cd-be7c-846bbadd6951")]
public class ImporterSettings
{
}

#region Migration
[MigratableInterface(typeof(ImporterSettings))]
public interface IImporterSettingsMigratable
{
}
    
[Serializable, LatestVersion(typeof(ImporterSettings))]
public class ImporterSettings_000 : IImporterSettingsMigratable
{
}
#endregion