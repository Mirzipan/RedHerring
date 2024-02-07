using Migration;
using RedHerring.Studio.Definitions;
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
	[HideInInspector, NonSerialized] private static StudioTheme[] _themes = // TODO - maybe from some attributes? 
		{ 
			new ("Crimson Rivers", Render.ImGui.Theme.CrimsonRivers),
			new ("Embrace the Darkness", Render.ImGui.Theme.EmbraceTheDarkness),
			new ("Bloodsucker", Render.ImGui.Theme.Bloodsucker)
		};
	
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
		Clues.Definitions.Default<ThemeDefinition>()?.Apply();
		//Array.Find(_themes, theme => theme.Name == Theme)?.Apply();
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