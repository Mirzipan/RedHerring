using Migration;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models;

[Serializable, SerializedClassId("studio-settings-class-id")]
public sealed class StudioSettings
{
	[NonSerialized] public static string? HomeDirectory = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
		? Environment.GetEnvironmentVariable("HOME")
		: Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%"); // TODO - not good, application path needed
	
	[NonSerialized] public readonly string SettingsPath = Path.Join(HomeDirectory, "RedHerring", "options.json");
	public          int    WorkerThreadsCount;
}

#region Migration
[MigratableInterface(typeof(StudioSettings))]
public interface IStudioSettingsMigratable
{
}
    
[Serializable, LatestVersion(typeof(StudioSettings))]
public class StudioSettings_000 : IStudioSettingsMigratable
{
}
#endregion