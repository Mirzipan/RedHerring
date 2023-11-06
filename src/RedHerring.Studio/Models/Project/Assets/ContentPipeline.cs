using RedHerring.Studio.Models.Project.FileSystem;
using RedHerring.Studio.Models.Project.Importers;
using RedHerring.Studio.Models.Project.Processors;

namespace RedHerring.Studio.Models.Project.Assets;

public sealed class ContentPipeline
{
	internal record struct ImportEntry(ProjectFileNode FileNode, object? Output = null, Type? OutputType = null);

	private readonly ImporterRegistry _importerRegistry = new();
	private readonly ProcessorRegistry _processorRegistry = new();
	
	private readonly CancellationTokenSource _cancellationTokenSource = new();
	private readonly EventWaitHandle         _importerIOWaitHandle    = new AutoResetEvent(false);
	private readonly ImporterIOThread        _importerIOThread;
	
	private readonly Queue<ImportEntry> _dataToRead    = new(); // processed by importer io thread
	private readonly Queue<ImportEntry> _dataToProcess = new(); // processed by worker threads
	private readonly Queue<ImporterData> _dataToWrite   = new(); // processed by importer io thread

	public ContentPipeline()
	{
		_importerIOThread = new ImporterIOThread(_cancellationTokenSource.Token, _importerIOWaitHandle, this);
		_importerIOThread.Start();
	}

	public void Cancel()
	{
		_cancellationTokenSource.Cancel();
		_importerIOWaitHandle.Set();
	}
	
	public Importer Find(string extension) => _importerRegistry.GetImporter(extension);

	public Processor Find(Type input, Type output) => _processorRegistry.Find(input, output);
	
	public void ImportFile(ProjectFileNode fileNode)
	{
		_dataToRead.Enqueue(new ImportEntry(fileNode));
		_importerIOWaitHandle.Set();
	}
	
	#region IO thread functions
	internal ImportEntry? GetDataToRead()
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

	internal void AddDataToProcess(ImportEntry data)
	{
		lock (_dataToProcess)
		{
			_dataToProcess.Enqueue(data);
		}
	}

	internal ImportEntry? GetDataToProcess()
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