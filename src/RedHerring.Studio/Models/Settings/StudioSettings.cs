using Migration;
using RedHerring.Studio.Tools;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models;

[Serializable, SerializedClassId("studio-settings-class-id")]
public sealed class StudioSettings
{
	public const string DefaultTheme = "Crimson Rivers";
	
	[NonSerialized] public static string? HomeDirectory = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
		? (Environment.GetEnvironmentVariable("XDG_CONFIG_HOME") ?? "~/Library/Application Support")
		: Environment.ExpandEnvironmentVariables("%APPDATA%");
	
	public string SettingsPath => Path.Join(HomeDirectory, "RedHerring", "options.json");
	
	public int WorkerThreadsCount = 4;

	[ValueDropdown(nameof(_themes)), OnCommitValue(nameof(ApplyTheme))] public string Theme = DefaultTheme;
	[HideInInspector, NonSerialized] private static StudioTheme[] _themes = // TODO - maybe from some attributes? 
		{ 
			new ("Crimson Rivers", ImGui.Theme.CrimsonRivers),
			new ("Embrace the Darkness", ImGui.Theme.EmbraceTheDarkness),
			new ("Bloodsucker", ImGui.Theme.Bloodsucker)
		};
	
	#region Data storage
	[HideInInspector] public string?      UiLayout;
	[HideInInspector] public int          ToolUniqueIdGeneratorState;
	[HideInInspector] public List<ToolId> ActiveToolWindows = new();
	#endregion

	public void StoreToolWindows(int uniqueIdGeneratorState, List<ToolId> openedToolWindows)
	{
		ToolUniqueIdGeneratorState = uniqueIdGeneratorState;
		ActiveToolWindows          = openedToolWindows;
	}

	public void ApplyTheme()
	{
		Array.Find(_themes, theme => theme.Name == Theme)?.Apply();
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
	public int    WorkerThreadsCount;
	public string Theme;
	
	#region Data storage
	public string?      UiLayout;
	public int          ToolUniqueIdGeneratorState;
	public List<ToolId> ActiveToolWindows;
	#endregion
}
#endregion