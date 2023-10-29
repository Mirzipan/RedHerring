namespace RedHerring.Studio.TaskProcessing;

public class TaskProcessor
{
	private readonly PriorityQueue<ATask, int> _tasks          = new();
	private readonly Queue<ATask>              _tasksToProcess = new();
	
	private readonly TaskProcessorDispatcher _dispatcher;

	private int _workerThreadsCount;
	private int _availableWorkerThreads;
	public  int WorkerThreadsCount     => _workerThreadsCount;
	public  int AvailableWorkerThreads => _availableWorkerThreads;

	private readonly CancellationTokenSource _cancellationTokenSource = new();
	private readonly EventWaitHandle         _workerWaitHandle        = new AutoResetEvent(false);

	public bool AnyWorkersRunning => _workerThreadsCount > 0;

	public delegate void           OnChangeDelegate(object sender);
	public event OnChangeDelegate? OnChanged;
	
	public TaskProcessor(int requiredWorkerThreads)
	{
		// create workers
		for (int i = 0; i < requiredWorkerThreads; ++i)
		{
			new TaskProcessorWorker(_cancellationTokenSource.Token, _workerWaitHandle, this).Start();
		}

		_workerThreadsCount     = requiredWorkerThreads;
		_availableWorkerThreads = requiredWorkerThreads;

		// create dispatcher
		_dispatcher = new TaskProcessorDispatcher(_cancellationTokenSource.Token, this);
		_dispatcher.Start();
	}

	public void EnqueueTask(ATask task, int priority)
	{
		lock (_tasks)
		{
			_tasks.Enqueue(task, priority);
		}

		_dispatcher.Notify();
		OnChanged?.Invoke(this);
	}

	public int GetRemainingTasks()
	{
		lock(_tasks)
		{
			return _tasks.Count;
		}
	}
	
	public void Cancel()
	{
		_cancellationTokenSource.Cancel();
		_dispatcher.Notify();
	}

	public bool CanProcessTask()
	{
		lock (_tasks)
		{
			return _tasks.Count > 0 && _availableWorkerThreads > 0;
		}
	}
	
	public void ProcessTask()
	{
		// get task
		ATask task;
		lock (_tasks)
		{
			task = _tasks.Dequeue();
		}

		// process task
		Interlocked.Decrement(ref _availableWorkerThreads);
		
		lock(_tasksToProcess)
		{
			_tasksToProcess.Enqueue(task);
		}

		// notify one of workers
		_workerWaitHandle.Set();

		OnChanged?.Invoke(this);
	}
	
	public ATask GetTaskToProcess()
	{
		lock (_tasksToProcess)
		{
			return _tasksToProcess.Dequeue();
		}
	}
	
	public void WorkerFinished()
	{
		Interlocked.Increment(ref _availableWorkerThreads);
		_dispatcher.Notify();
		OnChanged?.Invoke(this);
	}

	public void NotifyWorker()
	{
		_workerWaitHandle.Set();
	}

	public void WorkerExited()
	{
		Interlocked.Decrement(ref _workerThreadsCount);
		_dispatcher.Notify();
		OnChanged?.Invoke(this);
	}
}