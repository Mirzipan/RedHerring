namespace RedHerring.Studio.TaskProcessing;

public class TaskProcessorDispatcher
{
	private CancellationToken _cancellationToken;
	private EventWaitHandle   _waitHandle;
	private TaskProcessor     _processor;
	
	public TaskProcessorDispatcher(CancellationToken cancellationToken, TaskProcessor processor)
	{
		_cancellationToken = cancellationToken;
		_waitHandle        = new AutoResetEvent(false);
		_processor         = processor;
	}

	public void Start()
	{
		Thread thread = new (Do) {IsBackground = true};
		thread.Start();
	}
	
	public void Notify()
	{
		_waitHandle.Set();
	}

	private void Do()
	{
		while (!_cancellationToken.IsCancellationRequested)
		{
			if (_processor.CanProcessTask())
			{
				_processor.ProcessTask();
				continue;
			}

			_waitHandle.WaitOne();
		}

		// cancel already requested - wait for all workers to finish
		while (_processor.AnyWorkersRunning)
		{
			_processor.NotifyWorker();
			_waitHandle.WaitOne();
		}
	}
}