using System;
using Migration;

namespace RedHerring.Studio.Models;

[Serializable, SerializedClassId("project-settings-class-id")]
public class ProjectSettings
{
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
	public TargetPlatformEnum TargetPlatform;
}
#endregion