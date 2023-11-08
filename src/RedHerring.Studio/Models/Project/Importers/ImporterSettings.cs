using Migration;

namespace RedHerring.Studio.Models.Project.Importers;

[Serializable]
public abstract class ImporterSettings
{
}

#region Migration
[MigratableInterface(typeof(ImporterSettings))]
public interface IImporterSettingsMigratable
{
}
    
[Serializable, LatestVersion(typeof(ImporterSettings))]
public abstract class ImporterSettings_000 : IImporterSettingsMigratable
{
}
#endregion