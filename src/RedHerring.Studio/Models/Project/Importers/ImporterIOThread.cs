using RedHerring.Studio.Models.Project.Assets;
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
				Importer.ImportEntry? data = _importer.GetDataToRead();
				if (data is not null)
				{
					Read(data.Value);
					_importer.AddDataToProcess(data.Value);
				}
			}
			
			// try to write
			{
				Importer.ImportEntry? data = _importer.GetDataToRead();
				if (data != null)
				{
					//Write(data);
				}
			}

			_importer.IOThreadFinished();
		}

		// _import.IOThreadExited();
	}

	private void Read(Importer.ImportEntry data)
	{
		try
		{
			string extension = Path.GetExtension(data.FileNode.Path);
			var importer = _importer.Find(extension);
			using var stream = File.OpenRead(data.FileNode.Path);
			data.Output = importer.Import(stream);
			data.OutputType = data.Output!.GetType();
		}
		catch (Exception e)
		{
			ConsoleViewModel.Log($"Failed to read file {data.FileNode.Path}: {e.Message}", ConsoleItemType.Error);
		}
	}

	private void Write(Importer.ImportEntry data)
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