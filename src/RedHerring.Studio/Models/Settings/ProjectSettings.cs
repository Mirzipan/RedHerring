using Migration;
using RedHerring.Studio.Models.Project;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models;

[Serializable, SerializedClassId("project-settings-class-id")]
public sealed class ProjectSettings
{
	[ReadOnlyInInspector] public string GameFolderPath = "here will be readonly path";

	public TargetPlatformEnum TargetPlatform = TargetPlatformEnum.PC;
}

#region Migration
[MigratableInterface(typeof(ProjectSettings))]
public interface IProjectSettingsMigratable
{
}
    
[Serializable, LatestVersion(typeof(ProjectSettings))]
public class ProjectSettings_000 : IProjectSettingsMigratable
{
}
#endregion