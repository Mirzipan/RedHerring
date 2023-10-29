using RedHerring.Studio.Models.ViewModels.Console;

namespace RedHerring.Studio.Models.Project.Importers;

// https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchive?view=net-7.0

public sealed class ImporterIOThread
{
	private readonly CancellationToken _cancellationToken;
	private readonly EventWaitHandle   _waitHandle;
	private readonly Importer          _importer;
	
	public ImporterIOThread(CancellationToken cancellationToken, EventWaitHandle waitHandle, Importer importer)
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

			// try to read
			{
				ImporterData? data = _importer.GetDataToRead();
				if (data != null)
				{
					Read(data);
					_importer.AddDataToProcess(data);
				}
			}
			
			// try to write
			{
				ImporterData? data = _importer.GetDataToRead();
				if (data != null)
				{
					Write(data);
				}
			}

			_importer.IOThreadFinished();
		}

		// _import.IOThreadExited();
	}

	private void Read(ImporterData data)
	{
		try
		{
			byte[] bytes = File.ReadAllBytes(data.InputFileNode.Path);
			
			data.OutputDataItems.Add(
				new ImporterDataItem(bytes, data.InputFileNode.Name, ImporterOutputFormat.Raw)
			);
		}
		catch (Exception e)
		{
			ConsoleViewModel.Log($"Failed to read file {data.InputFileNode.Path}: {e.Message}", ConsoleItemType.Error);
		}
	}

	private void Write(ImporterData data)
	{
		foreach (ImporterDataItem dataItem in data.OutputDataItems)
		{
			if (!dataItem.ShouldBeExported)
			{
				continue;
			}
			
			// TODO - write file by type
			//???
		}
	}
}