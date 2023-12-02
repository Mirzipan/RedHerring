using System.Reflection;
using Migration;
using RedHerring.Studio.Commands;
using RedHerring.Studio.Models.Project;
using RedHerring.Studio.Models.ViewModels;
using RedHerring.Studio.Models.ViewModels.Console;
using RedHerring.Studio.TaskProcessing;

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

	private CommandHistory _commandHistory = new();
	public  CommandHistory CommandHistory => _commandHistory;
	
	// view models
	private readonly ConsoleViewModel _console = new();
	public           ConsoleViewModel Console => _console;
	
	private readonly SelectionViewModel _selection = new();
	public SelectionViewModel Selection => _selection;

	private readonly TaskProcessor _taskProcessor = new(_threadsCount);
	public           TaskProcessor TaskProcessor => _taskProcessor;

	public StudioModel()
	{
		_project = new ProjectModel(_migrationManager);
	}

	public void Cancel()
	{
		_taskProcessor.Cancel();
		Project.Close();
	}

	public async Task OpenProject(string path)
	{
		Selection.DeselectAll();
		Project.Close();
		
		try
		{
			ConsoleViewModel.Log($"Opening project from {path}", ConsoleItemType.Info);
			await Project.Open(path);
			ConsoleViewModel.Log($"Project opened", ConsoleItemType.Success);
		}
		catch (Exception e)
		{
			ConsoleViewModel.Log($"Exception: {e}", ConsoleItemType.Exception);
		}
	}

	public void RunTests()
	{
		for(int i=0;i <20;++i)
		{
			_taskProcessor.EnqueueTask(new TestTask(i), 0);
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