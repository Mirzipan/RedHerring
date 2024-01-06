using System.Text;
using RedHerring.Studio.Models;

namespace RedHerring.Studio.UserInterface;

public sealed class StatusBarMessageHandler
{
	private readonly StatusBar     _statusBar;
	private readonly StudioModel   _studioModel;
	private readonly StringBuilder _message;
	
	private int  _projectTasksCount = 0;
	private bool _needRefresh       = true;
	
	public StatusBarMessageHandler(StatusBar statusBar, StudioModel studioModel)
	{
		_statusBar   = statusBar;
		_studioModel = studioModel;
		_message     = new StringBuilder();
	}

	public void Update()
	{
		UpdateProjectTasks();
		
		if (_needRefresh)
		{
			Refresh();
			_needRefresh = false;
		}
	}

	private void UpdateProjectTasks()
	{
		int count = _studioModel.Project.TasksCount;
		if (count != _projectTasksCount)
		{
			_projectTasksCount = count;
			_needRefresh       = true;
		}
	}

	private void Refresh()
	{
		if (_projectTasksCount == 0)
		{
			_statusBar.Message      = "Ready";
			_statusBar.MessageColor = StatusBar.Color.Info;
			return;
		}
		
		_message.Clear();
		_message.Append("Processing ");
		_message.Append(_projectTasksCount);
		_message.Append(" tasks.");
		_statusBar.Message      = _message.ToString();
		_statusBar.MessageColor = StatusBar.Color.Info;
	}
}