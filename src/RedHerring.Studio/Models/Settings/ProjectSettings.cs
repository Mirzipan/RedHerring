using Migration;
using RedHerring.Core.Systems;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models;

[Serializable, SerializedClassId("7dc45fb0-41e7-45da-91ae-cd213e2400b5")]
public sealed class ProjectSettings
{
	public string AssetDatabaseSourcePath = "AssetDatabase.cs";
	public string AssetDatabaseNamespace  = "MyGame";
	public string AssetDatabaseClass      = "AssetDatabase";

	[ReadOnlyInInspector, NonSerialized] public string ProjectFolderPath = "[here will be path to game folder]";

	[ReadOnlyInInspector, ShowInInspector, NonSerialized] private string? _relativeResourcesPath;
	public                  string  RelativeResourcesPath => _relativeResourcesPath ??= PathsSystem.ResourcesFolderName;

	[ReadOnlyInInspector, ShowInInspector, NonSerialized] private string? _absoluteResourcesPath;
	public                  string  AbsoluteResourcesPath => _absoluteResourcesPath ??= Path.Combine(ProjectFolderPath, RelativeResourcesPath);

	[ReadOnlyInInspector, ShowInInspector, NonSerialized] private string? _absoluteScriptsPath;
	public                  string  AbsoluteScriptsPath => _absoluteScriptsPath ??= Path.Combine(ProjectFolderPath, "GameLibrary");

	[ReadOnlyInInspector, ShowInInspector, NonSerialized] private string? _absoluteAssetsPath;
	public                  string  AbsoluteAssetsPath => _absoluteAssetsPath ??= Path.Combine(ProjectFolderPath, "Assets");
}

#region Migration
[MigratableInterface(typeof(ProjectSettings))]
public interface IProjectSettingsMigratable;
    
[Serializable, ObsoleteVersion(typeof(ProjectSettings))]
public class ProjectSettings_000 : IProjectSettingsMigratable
{
}

[Serializable, LatestVersion(typeof(ProjectSettings))]
public class ProjectSettings_001 : IProjectSettingsMigratable
{
	public string AssetDatabaseSourcePath = null!;
	public string AssetDatabaseNamespace  = null!;
	public string AssetDatabaseClass      = null!;

	public void Migrate(ProjectSettings_000 prev)
	{
		AssetDatabaseSourcePath = "AssetDatabase.cs";
		AssetDatabaseNamespace  = "MyGame";
		AssetDatabaseClass      = "AssetDatabase";
	}
}
#endregion