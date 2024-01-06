using System.Text;
using RedHerring.Studio.Models;

namespace RedHerring.Studio.UserInterface;

public sealed class StatusBarMessageHandler
{
	private readonly StatusBar     _statusBar;
	private readonly StudioModel   _studioModel;
	private readonly StringBuilder _message;
	
	// TODO - this is bad, create something like "variable watcher" system to keep track about changes and show messages with priorities
	private int  _projectTasksCount      = 0;
	private bool _needsUpdateEngineFiles = false;
	private bool _needRefresh            = true;
	
	public StatusBarMessageHandler(StatusBar statusBar, StudioModel studioModel)
	{
		_statusBar   = statusBar;
		_studioModel = studioModel;
		_message     = new StringBuilder();
	}

	public void Update()
	{
		UpdateProjectVariables();
		
		if (_needRefresh)
		{
			Refresh();
			_needRefresh = false;
		}
	}

	private void UpdateProjectVariables()
	{
		int count = _studioModel.Project.TasksCount;
		if (count != _projectTasksCount)
		{
			_projectTasksCount = count;
			_needRefresh       = true;
		}

		if (_studioModel.Project.NeedsUpdateEngineFiles != _needsUpdateEngineFiles)
		{
			_needsUpdateEngineFiles = _studioModel.Project.NeedsUpdateEngineFiles;
			_needRefresh            = true;
		}
	}

	private void Refresh()
	{
		if (_projectTasksCount != 0)
		{
			_message.Clear();
			_message.Append("Processing ");
			_message.Append(_projectTasksCount);
			_message.Append(" tasks.");
			_statusBar.Message      = _message.ToString();
			_statusBar.MessageColor = StatusBar.Color.Info;
			return;
		}

		if (_needsUpdateEngineFiles)
		{
			_statusBar.Message      = "Engine files are obsolete and should be updated.";
			_statusBar.MessageColor = StatusBar.Color.Warning;
			return;
		}

		_statusBar.Message      = "Ready";
		_statusBar.MessageColor = StatusBar.Color.Info;
	}
}