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

	public async Task SaveStudioSettingsAsync()
	{
		byte[] json = await MigrationSerializer.SerializeAsync(StudioSettings, SerializedDataFormat.JSON, Assembly);
		Directory.CreateDirectory(Path.GetDirectoryName(StudioSettings.SettingsPath)!);
		await File.WriteAllBytesAsync(StudioSettings.SettingsPath, json);
	}

	public async Task LoadStudioSettingsAsync()
	{
		if(!File.Exists(StudioSettings.SettingsPath))
		{
			return;
		}
		
		byte[] json = await File.ReadAllBytesAsync(StudioSettings.SettingsPath);
		StudioSettings settings = await MigrationSerializer.DeserializeAsync<StudioSettings, IStudioSettingsMigratable>(_migrationManager.TypesHash, json, SerializedDataFormat.JSON, _migrationManager, false, Assembly);
		_studioSettings = settings;
	}
}