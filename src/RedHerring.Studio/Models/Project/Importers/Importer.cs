using RedHerring.Studio.Models.Project.FileSystem;

namespace RedHerring.Studio.Models.Project.Importers;

public sealed class Importer
{
	private readonly ImporterSelector _importerSelector = new();
	
	private readonly CancellationTokenSource _cancellationTokenSource = new();
	private readonly EventWaitHandle         _importerIOWaitHandle    = new AutoResetEvent(false);
	private readonly ImporterIOThread        _importerIOThread;
	
	private readonly Queue<ImporterData> _dataToRead    = new(); // processed by importer io thread
	private readonly Queue<ImporterData> _dataToProcess = new(); // processed by worker threads
	private readonly Queue<ImporterData> _dataToWrite   = new(); // processed by importer io thread

	public Importer()
	{
		_importerIOThread = new ImporterIOThread(_cancellationTokenSource.Token, _importerIOWaitHandle, this);
		_importerIOThread.Start();
	}

	public void Cancel()
	{
		_cancellationTokenSource.Cancel();
		_importerIOWaitHandle.Set();
	}
	
	public void ImportFile(ProjectFileNode fileNode)
	{
		_dataToRead.Enqueue(new ImporterData(fileNode));
		_importerIOWaitHandle.Set();
	}
	
	#region IO thread functions
	public ImporterData? GetDataToRead()
	{
		lock (_dataToRead)
		{
			if (_dataToRead.Count > 0)
			{
				return _dataToRead.Dequeue();
			}
		}

		return null;
	}

	public void AddDataToProcess(ImporterData data)
	{
		lock (_dataToProcess)
		{
			_dataToProcess.Enqueue(data);
		}
	}

	public ImporterData? GetDataToProcess()
	{
		lock (_dataToProcess)
		{
			if (_dataToProcess.Count > 0)
			{
				return _dataToProcess.Dequeue();
			}
		}

		return null;
	}

	public void AddDataToWrite(ImporterData data)
	{
		lock (_dataToWrite)
		{
			_dataToWrite.Enqueue(data);
		}
		_importerIOWaitHandle.Set();
	}
	
	public ImporterData? GetDataToWrite()
	{
		lock (_dataToWrite)
		{
			if (_dataToWrite.Count > 0)
			{
				return _dataToWrite.Dequeue();
			}
		}

		return null;
	}

	public void IOThreadFinished()
	{
		lock (_dataToRead)
		{
			if (_dataToRead.Count != 0)
			{
				_importerIOWaitHandle.Set();
			}
		}

		lock (_dataToWrite)
		{
			if (_dataToWrite.Count != 0)
			{
				_importerIOWaitHandle.Set();
			}
		}
	}
	
	#endregion
}