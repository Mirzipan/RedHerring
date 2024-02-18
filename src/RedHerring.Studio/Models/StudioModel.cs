using System.Reflection;
using Migration;
using RedHerring.Studio.Commands;
using RedHerring.Studio.Models.Project;
using RedHerring.Studio.Models.Project.Importers;
using RedHerring.Studio.Models.ViewModels;
using RedHerring.Studio.Models.ViewModels.Console;

namespace RedHerring.Studio.Models;

// main studio model
public class StudioModel
{
	private const int _threadsCount = 4;
	
	public static    Assembly         Assembly => typeof(StudioModel).Assembly; 
	private readonly MigrationManager _migrationManager = new(Assembly);
	
	private readonly ProjectModel _project;
	public           ProjectModel Project => _project;

	private StudioSettings  _studioSettings = new();
	public  StudioSettings  StudioSettings => _studioSettings;

	private CommandHistoryWithChange _commandHistory = new();
	public  CommandHistoryWithChange CommandHistory => _commandHistory;
	
	// view models
	private readonly ConsoleViewModel _console = new();
	public           ConsoleViewModel Console => _console;
	
	private readonly SelectionViewModel _selection = new();
	public SelectionViewModel Selection => _selection;

	// events
	private readonly StudioModelEventAggregator          _eventAggregator = new();
	public           IStudioModelEventAggregatorReadOnly EventAggregator => _eventAggregator;
	
	public StudioModel(ImporterRegistry importerRegistry)
	{
		_project = new ProjectModel(_migrationManager, importerRegistry, _eventAggregator);
	}

	public void Cancel()
	{
		Project.Close();
		Project.Cancel();
	}

	public void OpenProject(string path)
	{
		Selection.DeselectAll();
		Project.Close();
		
		try
		{
			ConsoleViewModel.Log($"Opening project from {path}", ConsoleItemType.Info);
			Project.Open(path);
			ConsoleViewModel.Log($"Project opened", ConsoleItemType.Success);
		}
		catch (Exception e)
		{
			ConsoleViewModel.Log($"Exception: {e}", ConsoleItemType.Exception);
		}
	}

	public void SaveStudioSettings(string applicationDataPath)
	{
		byte[] json = MigrationSerializer.SerializeAsync(StudioSettings, SerializedDataFormat.JSON, Assembly).GetAwaiter().GetResult();
		Directory.CreateDirectory(applicationDataPath);
		File.WriteAllBytes(Path.Join(applicationDataPath, StudioSettings.SettingsFileName), json);
	}

	public void LoadStudioSettings(string applicationDataPath)
	{
		string settingsFilePath = Path.Join(applicationDataPath, StudioSettings.SettingsFileName);
		if(!File.Exists(settingsFilePath))
		{
			return;
		}
		
		byte[] json = File.ReadAllBytes(settingsFilePath);
		StudioSettings settings = MigrationSerializer.DeserializeAsync<StudioSettings, IStudioSettingsMigratable>(_migrationManager.TypesHash, json, SerializedDataFormat.JSON, _migrationManager, false, Assembly).GetAwaiter().GetResult();
		_studioSettings = settings;
	}
}