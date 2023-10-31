using RedHerring.Studio.Models.Project.Assets;
using RedHerring.Studio.Models.ViewModels.Console;

namespace RedHerring.Studio.Models.Project.Processors;

// https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchive?view=net-7.0

public sealed class ProcessorIOThread
{
	private readonly CancellationToken _cancellationToken;
	private readonly EventWaitHandle   _waitHandle;
	private readonly Importer          _importer;
	
	public ProcessorIOThread(CancellationToken cancellationToken, EventWaitHandle waitHandle, Importer importer)
	{
		_cancellationToken = cancellationToken;
		_waitHandle        = waitHandle;
		_importer          = importer;
	}

	public void Start()
	{
		Thread thread = new (Do) {IsBackground = true};
		thread.Start();
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
			
			Importer.ImportEntry? data = _importer.GetDataToProcess();
			if (data != null)
			{
				//Write(data);
			}

			_importer.IOThreadFinished();
		}

		// _import.IOThreadExited();
	}

	private void Process(Importer.ImportEntry data)
	{
		// foreach (ImporterDataItem dataItem in data.OutputDataItems)
		// {
		// 	if (!dataItem.ShouldBeExported)
		// 	{
		// 		continue;
		// 	}
		// 	
		// 	// TODO - write file by type
		// 	//???
		// }
	}
}