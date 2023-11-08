using RedHerring.Studio.Models.Project.FileSystem;

namespace RedHerring.Studio.Models.Project.Importers;

// https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchive?view=net-7.0

public sealed class ImporterThread
{
	private readonly CancellationTokenSource _cancellationTokenSource = new();
	private readonly EventWaitHandle         _waitHandle              = new AutoResetEvent(false);
	private readonly StudioModel             _studioModel;
	private readonly ImporterRegistry        _registry;
	
	public ImporterThread(StudioModel studioModel, ImporterRegistry registry)
	{
		_studioModel = studioModel;
		_registry    = registry;
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

		// import
		Importer importer = _registry.GetImporter(node.Extension);
		using Stream stream = File.OpenRead(node.Path);
		object? intermediate = importer.Import(stream, node.Meta.ImporterSettings);
		if (intermediate == null)
		{
			return;
		}

		// resources path
		string resourcePath = Path.Combine(_studioModel.Project.ProjectSettings.AbsoluteResourcesPath, node.RelativePath);
		
		// process
		List<ImporterProcessor> processors = _registry.GetProcessors(intermediate.GetType());
		foreach (ImporterProcessor processor in processors)
		{
			if (_cancellationTokenSource.IsCancellationRequested)
			{
				return;
			}

			processor.Process(intermediate, node.Meta.ImporterSettings, resourcePath);
		}
	}
}