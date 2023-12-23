﻿using Migration;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models.Project.Importers;

[Serializable, SerializedClassId("scene-importer-id")]
public sealed class SceneImporterSettings : ImporterSettings
{
	[ReadOnlyInInspector] public List<SceneImporterMeshSettings> Meshes = new();
}

#region Migration
[MigratableInterface(typeof(SceneImporterSettings))]
public interface ISceneImporterSettingsMigratable : IImporterSettingsMigratable;

[Serializable, LatestVersion(typeof(SceneImporterSettings))]
public class SceneImporterSettings_000 : ImporterSettings_000, ISceneImporterSettingsMigratable
{
	public List<ISceneImporterMeshSettingsMigratable> Meshes;
}
#endregion