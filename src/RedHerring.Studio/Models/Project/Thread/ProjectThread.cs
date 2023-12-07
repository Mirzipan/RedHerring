using System.Collections.Concurrent;

namespace RedHerring.Studio.Models.Project;

public sealed class ProjectThread
{
	private readonly CancellationTokenSource      _cancellationTokenSource = new();
	private readonly EventWaitHandle              _waitHandle              = new AutoResetEvent(false);
	private readonly ConcurrentQueue<ProjectTask> _tasks                   = new();
	
	public ProjectThread()
	{
		Start();
	}

	public void Start()
	{
		Thread thread = new (Do) {IsBackground = true};
		thread.Start();
	}

	public void Enqueue(ProjectTask task)
	{
		_tasks.Enqueue(task);
		_waitHandle.Set();
	}
	
	public void ClearQueue()
	{
		_tasks.Clear();
	}

	public void Cancel()
	{
		_cancellationTokenSource.Cancel();
		_waitHandle.Set();
	}
	
	private void Do()
	{
		while (true)
		{
			if (!_tasks.TryDequeue(out ProjectTask? task))
			{
				_waitHandle.WaitOne();
			}

			if (_cancellationTokenSource.IsCancellationRequested)
			{
				break;
			}

			if (task == null)
			{
				continue;
			}

			task.Process(_cancellationTokenSource.Token);
		}
	}
}