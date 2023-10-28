using Migration;

namespace RedHerring.Studio.Models.Project.Importers;

[Serializable]
public abstract class AnImporterSettings
{
}

#region Migration
[MigratableInterface(typeof(AnImporterSettings))]
public interface IImporterSettingsMigratable
{
}
    
[Serializable, LatestVersion(typeof(AnImporterSettings))]
public abstract class AnImporterSettings_000 : IImporterSettingsMigratable
{
}
#endregion