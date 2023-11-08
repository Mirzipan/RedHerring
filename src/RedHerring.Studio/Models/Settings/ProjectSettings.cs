using Migration;
using RedHerring.Studio.Models.Project;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models;

[Serializable, SerializedClassId("project-settings-class-id")]
public sealed class ProjectSettings
{
	[ReadOnlyInInspector, NonSerialized] public string GameFolderPath = "[here will be path to game folder]";

	public TargetPlatformEnum TargetPlatform = TargetPlatformEnum.PC;

	[NonSerialized] private string? _relativeResourcesPath;
	public                  string  RelativeResourcesPath => _relativeResourcesPath ??= "Resources_" + TargetPlatform;

	[NonSerialized] private string? _absoluteResourcesPath;
	public                  string  AbsoluteResourcesPath => _absoluteResourcesPath ??= Path.Combine(GameFolderPath, RelativeResourcesPath);

}

#region Migration
[MigratableInterface(typeof(ProjectSettings))]
public interface IProjectSettingsMigratable
{
}
    
[Serializable, LatestVersion(typeof(ProjectSettings))]
public class ProjectSettings_000 : IProjectSettingsMigratable
{
	public TargetPlatformEnum TargetPlatform;
}
#endregion