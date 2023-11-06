using RedHerring.Studio.Models.Project.Assets;
using RedHerring.Studio.Models.Project.Processors;
using RedHerring.Studio.Models.ViewModels.Console;

namespace RedHerring.Studio.Models.Project.Importers;

// https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchive?view=net-7.0

public sealed class ImporterIOThread
{
	private readonly CancellationToken _cancellationToken;
	private readonly EventWaitHandle   _waitHandle;
	private readonly ContentPipeline     _contentPipeline;
	
	public ImporterIOThread(CancellationToken cancellationToken, EventWaitHandle waitHandle, ContentPipeline contentPipeline)
	{
		_cancellationToken = cancellationToken;
		_waitHandle        = waitHandle;
		_contentPipeline          = contentPipeline;
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

			ContentPipeline.ImportEntry? toRead = _contentPipeline.GetDataToRead();
			if (toRead is not null)
			{
				Read(toRead.Value);
				_contentPipeline.AddDataToProcess(toRead.Value);
			}

			ContentPipeline.ImportEntry? toProcess = _contentPipeline.GetDataToProcess();
			if (toProcess != null)
			{
				Process(toProcess.Value);
			}
			
			ImporterData? toWrite = _contentPipeline.GetDataToWrite();
			if (toWrite != null)
			{
				Write(toWrite);
			}

			_contentPipeline.IOThreadFinished();
		}

		// _import.IOThreadExited();
	}

	private void Read(ContentPipeline.ImportEntry data)
	{
		try
		{
			string extension = Path.GetExtension(data.FileNode.Path);
			var importer = _contentPipeline.Find(extension);
			using var stream = File.OpenRead(data.FileNode.Path);
			data.Output = importer.Import(stream);
			data.OutputType = data.Output!.GetType();
		}
		catch (Exception e)
		{
			ConsoleViewModel.Log($"Failed to read file {data.FileNode.Path}: {e.Message}", ConsoleItemType.Error);
		}
	}

	private void Process(ContentPipeline.ImportEntry data)
	{
		try
		{
			
		}
		catch (Exception e)
		{
			ConsoleViewModel.Log($"Failed to process file {data.FileNode.Path}: {e.Message}", ConsoleItemType.Error);
		}
	}

	private void Write(ImporterData data)
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