using RedHerring.Studio.Models.Project;
using RedHerring.Studio.Models.ViewModels.Console;
using RedHerring.Studio.TaskProcessor;

namespace RedHerring.Studio.Models;

// main model
public class StudioModel
{
	private const int _threadsCount = 4;
	
	private readonly ProjectModel _project = new();
	public           ProjectModel Project => _project;
	
	private readonly ConsoleViewModel _console = new();
	public           ConsoleViewModel Console => _console;

	private readonly TaskProcessor.TaskProcessor _taskProcessor = new(_threadsCount);
	public           TaskProcessor.TaskProcessor TaskProcessor => _taskProcessor;

	public void Exit()
	{
		_taskProcessor.Cancel();
	}

	public void RunTests()
	{
		for(int i=0;i <20;++i)
		{
			_taskProcessor.EnqueueTask(new TestTask(i), 0);
		}
	}
}