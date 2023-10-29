namespace RedHerring.Studio.TaskProcessing;

public class TaskProcessorWorker
{
	private CancellationToken _cancellationToken;
	private EventWaitHandle   _waitHandle;
	private TaskProcessor     _processor;

	public TaskProcessorWorker(CancellationToken cancellationToken, EventWaitHandle waitHandle, TaskProcessor processor)
	{
		_cancellationToken = cancellationToken;
		_waitHandle        = waitHandle;
		_processor         = processor;
	}

	public void Start()
	{
		Thread workerThread = new (Do) {IsBackground = true};
		workerThread.Start();
	}

	private void Do()
	{
		while (true)
		{
			_waitHandle.WaitOne();
			if (_cancellationToken.IsCancellationRequested)
			{
				break;
			}

			_processor.GetTaskToProcess().Process(_cancellationToken);
			_processor.WorkerFinished();
		}

		_processor.WorkerExited();
	}
}