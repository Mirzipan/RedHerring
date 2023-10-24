using RedHerring.Studio.Models.Project;
using RedHerring.Studio.Models.TaskProcessor;

namespace RedHerring.Studio.Models;

// main model, singleton (not sure if this is ok)
public class Model
{
	private const int _threadsCount = 4;
	
	private static Model _instance = new();
	public static  Model Instance => _instance;

	private       ProjectModel  _project = new();
	public static ProjectModel Project => _instance._project;

	private readonly TaskProcessor.TaskProcessor _taskProcessor = new(_threadsCount);
	public           TaskProcessor.TaskProcessor TaskProcessor => _taskProcessor;

	public void Exit()
	{
		_taskProcessor.Cancel();
	}

	public void RunTests()
	{
		for(int i=0;i<20;++i)
		{
			_taskProcessor.EnqueueTask(new TestTask(i), 0);
		}
	}
}