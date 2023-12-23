﻿using Migration;

namespace RedHerring.Studio.Models.Project.Importers;

[Serializable, SerializedClassId("copy-importer-id")]
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