using Migration;
using RedHerring.Studio.Definition;
using RedHerring.Studio.Tools;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models;

[Serializable, SerializedClassId("studio-settings-class-id")]
public sealed class StudioSettings
{
	public const string DefaultTheme     = "Crimson Rivers";
	public const string SettingsFileName = "studio_settings.json";

	public int WorkerThreadsCount = 4;

	[ValueDropdown(nameof(_themes)), OnCommitValue(nameof(ApplyTheme))] public string Theme = DefaultTheme;

	[HideInInspector, NonSerialized]
	private static StudioTheme[] _themes = Clues.Definitions.ByType<ThemeDefinition>().Select(e => new StudioTheme(e.Name, e.Apply)).ToArray();
	
	#region Data storage
	[HideInInspector] public string?       UiLayout;
	[HideInInspector] public int           ToolUniqueIdGeneratorState;
	[HideInInspector] public List<ToolId>  ActiveToolWindows = new();
	#endregion

	public void StoreToolWindows(int uniqueIdGeneratorState, List<ToolId> openedToolWindows)
	{
		ToolUniqueIdGeneratorState = uniqueIdGeneratorState;
		ActiveToolWindows          = openedToolWindows;
	}

	public void ApplyTheme()
	{
		Array.Find(_themes, e => e.Name == Theme)?.Apply();
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