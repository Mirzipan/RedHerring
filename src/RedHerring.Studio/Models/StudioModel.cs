using RedHerring.Studio.Models.Project;
using RedHerring.Studio.Models.Project.Importers;
using RedHerring.Studio.Models.ViewModels;
using RedHerring.Studio.Models.ViewModels.Console;
using RedHerring.Studio.TaskProcessing;

namespace RedHerring.Studio.Models;

// main studio model
public class StudioModel
{
	private const int _threadsCount = 4;
	
	private readonly ProjectModel _project = new();
	public           ProjectModel Project => _project;

	public readonly StudioSettings  StudioSettings = new();
	public readonly ProjectSettings ProjectSettings = new();
	
	// view models
	private readonly ConsoleViewModel _console = new();
	public           ConsoleViewModel Console => _console;
	
	private readonly SelectionViewModel _selection = new();
	public SelectionViewModel Selection => _selection;

	private readonly TaskProcessor _taskProcessor = new(_threadsCount);
	public           TaskProcessor TaskProcessor => _taskProcessor;
	
	private readonly Importer _importer = new();

	public void Cancel()
	{
		_importer.Cancel();
		_taskProcessor.Cancel();
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

	public void ReimportAll()
	{
		
	}

	public void RunTests()
	{
		for(int i=0;i <20;++i)
		{
			_taskProcessor.EnqueueTask(new TestTask(i), 0);
		}
	}
}