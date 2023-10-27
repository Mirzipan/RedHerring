using RedHerring.Studio.Models.Project;
using RedHerring.Studio.Models.ViewModels;
using RedHerring.Studio.Models.ViewModels.Console;
using RedHerring.Studio.TaskProcessor;

namespace RedHerring.Studio.Models;

// main model
public class StudioModel
{
	private const int _threadsCount = 4;
	
	private readonly ProjectModel _project = new();
	public           ProjectModel Project => _project;
	
	// view models
	private readonly ConsoleViewModel _console = new();
	public           ConsoleViewModel Console => _console;
	
	private readonly SelectionViewModel _selection = new();
	public SelectionViewModel Selection => _selection;

	private readonly TaskProcessor.TaskProcessor _taskProcessor = new(_threadsCount);
	public           TaskProcessor.TaskProcessor TaskProcessor => _taskProcessor;

	public void Exit()
	{
		_taskProcessor.Cancel();
	}

	public async Task OpenProject(string path)
	{
		Selection.DeselectAll();
		Project.Close();
		
		try
		{
			Console.Log($"Opening project from {path}", ConsoleItemType.Info);
			await Project.Open(path);
			Console.Log($"Project opened", ConsoleItemType.Success);
		}
		catch (Exception e)
		{
			Console.Log($"Exception: {e}", ConsoleItemType.Exception);
		}
	}
	
	public void RunTests()
	{
		for(int i=0;i <20;++i)
		{
			_taskProcessor.EnqueueTask(new TestTask(i), 0);
		}
	}
}