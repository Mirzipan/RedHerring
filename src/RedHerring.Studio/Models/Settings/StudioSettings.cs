using Migration;
using RedHerring.Studio.Tools;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models;

[Serializable, SerializedClassId("studio-settings-class-id")]
public sealed class StudioSettings
{
	[NonSerialized] public static string? HomeDirectory = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
		? (Environment.GetEnvironmentVariable("XDG_CONFIG_HOME") ?? "~/Library/Application Support")
		: Environment.ExpandEnvironmentVariables("%APPDATA%");
	
	public string SettingsPath => Path.Join(HomeDirectory, "RedHerring", "options.json");

	public                   int     WorkerThreadsCount = 4;
	[HideInInspector] public string? UiLayout;
	
	[HideInInspector] public int          ToolUniqueIdGeneratorState;
	[HideInInspector] public List<ToolId> ActiveToolWindows = new();

	public void StoreToolWindows(int uniqueIdGeneratorState, List<ToolId> openedToolWindows)
	{
		ToolUniqueIdGeneratorState = uniqueIdGeneratorState;
		ActiveToolWindows          = openedToolWindows;
	}
}

#region Migration
[MigratableInterface(typeof(StudioSettings))]
public interface IStudioSettingsMigratable
{
}
    
[Serializable, LatestVersion(typeof(StudioSettings))]
public class StudioSettings_000 : IStudioSettingsMigratable
{
	public int          WorkerThreadsCount;
	public string?      UiLayout;
	public int          ToolUniqueIdGeneratorState;
	public List<ToolId> ActiveToolWindows;
}
#endregion