﻿using RedHerring.Studio.Models.Project.FileSystem;

namespace RedHerring.Studio.Models.Project.Importers;

// https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchive?view=net-7.0

public sealed class ImporterThread
{
	private readonly CancellationTokenSource _cancellationTokenSource = new();
	private readonly EventWaitHandle         _waitHandle              = new AutoResetEvent(false);
	private readonly ImporterRegistry        _importerRegistry        = new();
	private readonly StudioModel             _studioModel;
	
	public ImporterThread(StudioModel studioModel)
	{
		_studioModel = studioModel;
		Start();
	}

	public void Start()
	{
		Thread thread = new (Do) {IsBackground = true};
		thread.Start();
	}

	public void Continue()
	{
		_waitHandle.Set();
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
			_waitHandle.WaitOne();
			if (_cancellationTokenSource.IsCancellationRequested)
			{
				break;
			}
			
			_studioModel.Project.AssetsFolder?.TraverseRecursive(Import, _cancellationTokenSource.Token);
		}
	}

	private void Import(AProjectNode node)
	{
		if (node is ProjectFolderNode)
		{
			return;
		}

		if (_cancellationTokenSource.IsCancellationRequested)
		{
			return;
		}

		Importer importer = _importerRegistry.GetImporter(node.Extension);
		
		using Stream stream = File.OpenRead(node.Path);
		importer.Import(stream, node.Meta.ImporterSettings);
	}
}