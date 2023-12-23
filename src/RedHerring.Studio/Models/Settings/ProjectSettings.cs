using Migration;
using RedHerring.Core.Systems;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models;

[Serializable, SerializedClassId("project-settings-class-id")]
public sealed class ProjectSettings
{
	[ReadOnlyInInspector, NonSerialized] public string ProjectFolderPath = "[here will be path to game folder]";

	[NonSerialized] private string? _relativeResourcesPath;
	public                  string  RelativeResourcesPath => _relativeResourcesPath ??= PathsSystem.ResourcesFolderName;

	[NonSerialized] private string? _absoluteResourcesPath;
	public                  string  AbsoluteResourcesPath => _absoluteResourcesPath ??= Path.Combine(ProjectFolderPath, RelativeResourcesPath);

	[NonSerialized] private string? _absoluteScriptsPath;
	public                  string  AbsoluteScriptsPath => _absoluteScriptsPath ??= Path.Combine(ProjectFolderPath, "GameLibrary");

	[NonSerialized] private string? _absoluteAssetsPath;
	public                  string  AbsoluteAssetsPath => _absoluteAssetsPath ??= Path.Combine(ProjectFolderPath, "Assets");
}

#region Migration
[MigratableInterface(typeof(ProjectSettings))]
public interface IProjectSettingsMigratable;
    
[Serializable, LatestVersion(typeof(ProjectSettings))]
public class ProjectSettings_000 : IProjectSettingsMigratable
{
}
#endregion