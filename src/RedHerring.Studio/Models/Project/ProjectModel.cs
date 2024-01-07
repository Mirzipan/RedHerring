using System.Collections.Concurrent;
using System.Reflection;
using System.Security.Cryptography;
using Migration;
using RedHerring.Studio.Models.Project.FileSystem;
using RedHerring.Studio.Models.Project.Importers;
using RedHerring.Studio.Models.ViewModels.Console;

namespace RedHerring.Studio.Models.Project;

public sealed class ProjectModel
{
	private const           string _assetsFolderName  = "Assets";
	private const           string _scriptsFolderName = "GameLibrary";
	private const           string _settingsFileName  = "Project.json";
	private static readonly char[] _slash             = {'/', '\\'};

	public static           Assembly      Assembly => typeof(ProjectModel).Assembly; 
	private static readonly HashAlgorithm _hashAlgorithm = SHA1.Create();

	private readonly MigrationManager _migrationManager;
	private readonly ImporterRegistry _importerRegistry;
	
	public readonly object ProjectTreeLock = new(); // synchronization lock
	
	private ProjectRootNode? _assetsFolder;
	public  ProjectRootNode? AssetsFolder => _assetsFolder;

	private ProjectRootNode? _scriptsFolder;
	public  ProjectRootNode? ScriptsFolder => _scriptsFolder; 

	private ProjectSettings? _projectSettings;
	public  ProjectSettings ProjectSettings => _projectSettings!;

	public bool IsOpened               => !string.IsNullOrEmpty(_projectSettings?.ProjectFolderPath);
	public bool NeedsUpdateEngineFiles { get; private set; } = false;
	
	private          FileSystemWatcher?           _assetsWatcher;
	private          FileSystemWatcher?           _scriptsWatcher;
	private          bool                         _watchersActive      = true;
	private readonly ConcurrentQueue<ProjectTask> _waitingWatcherTasks = new();
	
	private readonly ProjectThread _thread = new ();
	public           int           TasksCount => _thread.TasksCount;
	
	public ProjectModel(MigrationManager migrationManager, ImporterRegistry importerRegistry)
	{
		_migrationManager = migrationManager;
		_importerRegistry = importerRegistry;
	}

	public void Cancel()
	{
		_thread.Cancel();
	}

	#region Open/close
	public void Close()
	{
		DisposeWatchers();

		_thread.ClearQueue();
		
		SaveSettings();
		_assetsFolder = null;
	}
	
	public void Open(string projectPath)
	{
		lock (ProjectTreeLock)
		{
			// create roots
			string          assetsPath   = Path.Join(projectPath, _assetsFolderName);
			ProjectRootNode assetsFolder = new ProjectRootNode(_assetsFolderName, assetsPath, ProjectNodeType.AssetFolder);
			_assetsFolder = assetsFolder;

			string          scriptsGameLibraryPath   = Path.Join(projectPath, _scriptsFolderName);
			ProjectRootNode scriptsGameLibraryFolder = new ProjectRootNode(_scriptsFolderName, scriptsGameLibraryPath, ProjectNodeType.ScriptFolder);
			_scriptsFolder = scriptsGameLibraryFolder;

			// check
			if (!Directory.Exists(assetsPath))
			{
				// error
				ConsoleViewModel.LogError($"Assets folder not found on path {assetsPath}");
				InitMeta();
				return;
			}

			// load / create settings
			LoadSettings(projectPath);

			// read assets
			try
			{
				List<string> foundMetaFiles = new();
				RecursiveAssetScan(assetsPath, "", assetsFolder, foundMetaFiles);
				MetaCleanup(foundMetaFiles);
			}
			catch (Exception e)
			{
				ConsoleViewModel.LogException($"Exception: {e}");
			}

			// read scripts
			try
			{
				RecursiveScriptScan(scriptsGameLibraryPath, "", scriptsGameLibraryFolder);
			}
			catch (Exception e)
			{
				ConsoleViewModel.LogException($"Exception: {e}");
			}
		}

		EnqueueProjectTask(CreateEngineFilesCheckTask(projectPath));

		InitMeta();

		ImportAll();

		CreateWatchers();
	}
	#endregion
	
	#region Import
	public void ClearResources()
	{
		EnqueueProjectTask(CreateClearResourcesTask(_projectSettings!.AbsoluteResourcesPath));

		lock (ProjectTreeLock)
		{
			_assetsFolder!.TraverseRecursive(
				node => EnqueueProjectTask(CreateResetMetaHashTask(_assetsFolder!, node.RelativePath)),
				TraverseFlags.Directories | TraverseFlags.Files,
				default
			);
		}
	}

	public void ImportAll()
	{
		lock (ProjectTreeLock)
		{
			_assetsFolder!.TraverseRecursive(
				node => EnqueueProjectTask(CreateImportTask(_assetsFolder!, node.RelativePath)),
				TraverseFlags.Files,
				default
			);
		}
	}
	#endregion
	
	#region Folder watchers
	public void PauseWatchers()
	{
		_watchersActive = false;
	}

	public void ResumeWatchers()
	{
		while (!_waitingWatcherTasks.IsEmpty)
		{
			if (_waitingWatcherTasks.TryDequeue(out ProjectTask? task))
			{
				_thread.Enqueue(task);
			}
		}

		_watchersActive = true;
	}

	private void CreateWatchers()
	{
		_assetsWatcher                       =  new FileSystemWatcher(_projectSettings!.AbsoluteAssetsPath);
		_assetsWatcher.NotifyFilter          =  NotifyFilters.DirectoryName | NotifyFilters.FileName;
		_assetsWatcher.Created               += OnAssetCreated;
		_assetsWatcher.Deleted               += OnAssetDeleted;
		_assetsWatcher.Renamed               += OnAssetRenamed;
		_assetsWatcher.Filter                =  "";
		_assetsWatcher.IncludeSubdirectories =  true;
		_assetsWatcher.EnableRaisingEvents   =  true;

		_scriptsWatcher                       =  new FileSystemWatcher(_projectSettings!.AbsoluteScriptsPath);
		_scriptsWatcher.NotifyFilter          =  NotifyFilters.DirectoryName | NotifyFilters.FileName;
		_scriptsWatcher.Created               += OnScriptCreated;
		_scriptsWatcher.Deleted               += OnScriptDeleted;
		_scriptsWatcher.Renamed               += OnScriptRenamed;
		_scriptsWatcher.Filter                =  "";
		_scriptsWatcher.IncludeSubdirectories =  true;
		_scriptsWatcher.EnableRaisingEvents   =  true;
	}

	private void OnAssetCreated(object sender, FileSystemEventArgs evt)
	{
		if (evt.Name == null)
		{
			return;
		}

		OnAssetCreated(evt.FullPath, evt.Name);
	}

	private void OnAssetDeleted(object sender, FileSystemEventArgs evt)
	{
		if (evt.Name == null)
		{
			return;
		}

		OnAssetDeleted(evt.FullPath, evt.Name);
	}

	private void OnAssetRenamed(object sender, RenamedEventArgs evt)
	{
		if (evt.Name == null || evt.OldName == null)
		{
			return;
		}

		OnAssetDeleted(evt.OldFullPath, evt.OldName);
		OnAssetCreated(evt.FullPath, evt.Name);
	}
 
	private void OnScriptCreated(object sender, FileSystemEventArgs evt)
	{
		if (evt.Name == null)
		{
			return;
		}

		if (!Directory.Exists(evt.FullPath) && !evt.Name.EndsWith(".cs"))
		{
			return;
		}

		OnScriptCreated(evt.FullPath, evt.Name);
	}
	private void OnScriptDeleted(object sender, FileSystemEventArgs evt)
	{
		if (evt.Name == null)
		{
			return;
		}

		OnScriptDeleted(evt.FullPath, evt.Name);
	}
	private void OnScriptRenamed(object sender, RenamedEventArgs evt)
	{
		if (evt.Name == null || evt.OldName == null)
		{
			return;
		}

		if (!Directory.Exists(evt.FullPath) && !evt.Name.EndsWith(".cs"))
		{
			return;
		}
		
		OnScriptDeleted(evt.OldFullPath, evt.OldName);
		OnScriptCreated(evt.FullPath, evt.Name);
	}

	private void DisposeWatchers()
	{
		_assetsWatcher?.Dispose();
		_assetsWatcher = null;
		
		_scriptsWatcher?.Dispose();
		_scriptsWatcher = null;
	}

	private void EnqueueProjectTaskFromWatcher(ProjectTask task)
	{
		if (_watchersActive)
		{
			_thread.Enqueue(task);
			return;
		}

		_waitingWatcherTasks.Enqueue(task);
	}
	#endregion
	
	#region Settings
	public void SaveSettings()
	{
		if (_projectSettings == null)
		{
			return;
		}

		byte[] json = MigrationSerializer.SerializeAsync(_projectSettings, SerializedDataFormat.JSON, Assembly).GetAwaiter().GetResult();
		File.WriteAllBytes(Path.Join(_projectSettings.ProjectFolderPath, _settingsFileName), json);
	}

	public void LoadSettings(string projectPath)
	{
		string path = Path.Join(projectPath, _settingsFileName);
		if(!File.Exists(path))
		{
			_projectSettings = new ProjectSettings
			                   {
				                   ProjectFolderPath = projectPath
			                   };
			return;
		}
		
		byte[] json = File.ReadAllBytes(path);
		ProjectSettings settings = MigrationSerializer.DeserializeAsync<ProjectSettings, IStudioSettingsMigratable>(_migrationManager.TypesHash, json, SerializedDataFormat.JSON, _migrationManager, false, Assembly).GetAwaiter().GetResult();
		settings.ProjectFolderPath = projectPath;
		
		_projectSettings = settings;
	}
	#endregion
	
	#region Asset manipulation
	private void RecursiveAssetScan(string path, string relativePath, ProjectFolderNode root, List<string> foundMetaFiles)
	{
		// scan directories
		foreach (string directoryPath in Directory.EnumerateDirectories(path))
		{
			string            directory             = Path.GetFileName(directoryPath);
			string            relativeDirectoryPath = Path.Combine(relativePath, directory);
			ProjectFolderNode folderNode            = new(directory, directoryPath, relativeDirectoryPath, true, ProjectNodeType.AssetFolder);
			root.Children.Add(folderNode);
			
			RecursiveAssetScan(directoryPath, relativeDirectoryPath, folderNode, foundMetaFiles);
		}

		// scan files except meta
		foreach (string filePath in Directory.EnumerateFiles(path))
		{
			string fileName = Path.GetFileName(filePath);
			if (fileName.EndsWith(".meta"))
			{
				foundMetaFiles.Add(filePath);
				continue;
			}

			string               relativeFilePath = Path.Combine(relativePath, fileName);
			ProjectAssetFileNode assetFileNode    = new(fileName, filePath, relativeFilePath);
			root.Children.Add(assetFileNode);
		}
	}
	
	private void InitMeta()
	{
		lock (ProjectTreeLock)
		{
			_assetsFolder!.TraverseRecursive(
				node => _thread.Enqueue(CreateInitMetaTask(_assetsFolder!, node.RelativePath)),
				TraverseFlags.Directories | TraverseFlags.Files,
				default
			);

			_scriptsFolder!.TraverseRecursive(
				node => _thread.Enqueue(CreateInitMetaTask(_scriptsFolder!, node.RelativePath)),
				TraverseFlags.Directories | TraverseFlags.Files,
				default
			);
		}
	}

	private void OnAssetCreated(string eventAbsolutePath, string eventRelativePath)
	{
		if (eventRelativePath.EndsWith(".meta"))
		{
			return;
		}

		string relativePath = "";
		string path         = eventRelativePath;
		while (path.Length > 0)
		{
			int index = path.IndexOfAny(_slash);
			if (index >= 0)
			{
				string folderName = path.Substring(0, index);
				path = path.Substring(index + 1);

				string parentRelativePath = relativePath;
				relativePath = Path.Join(relativePath, folderName);

				// folder
				EnqueueProjectTaskFromWatcher(CreateNewFolderNodeTask(_assetsFolder!, parentRelativePath, folderName, true, ProjectNodeType.AssetFolder));
				EnqueueProjectTaskFromWatcher(CreateInitMetaTask(_assetsFolder!, relativePath));
			}
			else
			{
				if (Directory.Exists(eventAbsolutePath))
				{
					// created directory
					EnqueueProjectTaskFromWatcher(CreateNewFolderNodeTask(_assetsFolder!, relativePath, path, true, ProjectNodeType.AssetFolder));
					EnqueueProjectTaskFromWatcher(CreateInitMetaTask(_assetsFolder!, eventRelativePath));
				}
				else
				{
					// created file
					EnqueueProjectTaskFromWatcher(CreateNewAssetFileTask(_assetsFolder!, relativePath, path));
					EnqueueProjectTaskFromWatcher(CreateInitMetaTask(_assetsFolder!, eventRelativePath));
					EnqueueProjectTaskFromWatcher(CreateImportTask(_assetsFolder!, eventRelativePath));
				}
				break;
			}
		}
	}

	private void OnAssetDeleted(string eventAbsolutePath, string eventRelativePath)
	{
		if (eventRelativePath.EndsWith(".meta"))
		{
			// deleted meta file .. create new
			string assetRelativeFilePath = eventRelativePath.Substring(0, eventRelativePath.Length - ".meta".Length);
			EnqueueProjectTaskFromWatcher(CreateInitMetaTask(_assetsFolder!, assetRelativeFilePath));
			return;
		}

		// deleted folder or file
		{
			int    index      = eventRelativePath.LastIndexOfAny(_slash);
			string parentPath = eventRelativePath.Substring(0, index);
			string nodeName   = eventRelativePath.Substring(index + 1);
			EnqueueProjectTaskFromWatcher(CreateDeleteNodeTask(_assetsFolder!, parentPath, nodeName));
		}
	}

	private void MetaCleanup(List<string> metaFiles)
	{
		foreach (string metaFile in metaFiles)
		{
			string path = metaFile.Substring(0, metaFile.Length - ".meta".Length);
			if (Directory.Exists(path) || File.Exists(path))
			{
				continue;
			}

			ConsoleViewModel.LogWarning($"Removing unused meta file: {metaFile}");
			File.Delete(metaFile);
		}
	}
	#endregion
	
	#region Script manipulation
	private void RecursiveScriptScan(string path, string relativePath, ProjectFolderNode root)
	{
		// scan directories
		foreach (string directoryPath in Directory.EnumerateDirectories(path))
		{
			string            directory             = Path.GetFileName(directoryPath);
			string            relativeDirectoryPath = Path.Combine(relativePath, directory);
			ProjectFolderNode folderNode            = new(directory, directoryPath, relativeDirectoryPath, false, ProjectNodeType.ScriptFolder);
			root.Children.Add(folderNode);
			
			RecursiveScriptScan(directoryPath, relativeDirectoryPath, folderNode);
		}

		// scan all files
		foreach (string filePath in Directory.EnumerateFiles(path))
		{
			string                fileName         = Path.GetFileName(filePath);
			string                relativeFilePath = Path.Combine(relativePath, fileName);
			ProjectScriptFileNode assetFileNode    = new(fileName, filePath, relativeFilePath);
			root.Children.Add(assetFileNode);
		}
	}

	private void OnScriptCreated(string eventAbsolutePath, string eventRelativePath)
	{
		string relativePath = "";
		string path         = eventRelativePath;
		while (path.Length > 0)
		{
			int index = path.IndexOfAny(_slash);
			if (index >= 0)
			{
				string folderName = path.Substring(0, index);
				path = path.Substring(index + 1);

				string parentRelativePath = relativePath;
				relativePath = Path.Join(relativePath, folderName);

				// folder
				EnqueueProjectTaskFromWatcher(CreateNewFolderNodeTask(_scriptsFolder!, parentRelativePath, folderName, false, ProjectNodeType.ScriptFolder));
			}
			else
			{
				if (Directory.Exists(eventAbsolutePath))
				{
					// created directory
					EnqueueProjectTaskFromWatcher(CreateNewFolderNodeTask(_scriptsFolder!, relativePath, path, false, ProjectNodeType.ScriptFolder));
				}
				else
				{
					// created file
					EnqueueProjectTaskFromWatcher(CreateNewScriptFileTask(_scriptsFolder!, relativePath, path));
					EnqueueProjectTaskFromWatcher(CreateInitMetaTask(_scriptsFolder!, eventRelativePath));
				}
				break;
			}
		}
	}

	private void OnScriptDeleted(string eventAbsolutePath, string eventRelativePath)
	{
		{
			int    index      = eventRelativePath.LastIndexOfAny(_slash);
			string parentPath = eventRelativePath.Substring(0, index);
			string nodeName   = eventRelativePath.Substring(index + 1);
			EnqueueProjectTaskFromWatcher(CreateDeleteNodeTask(_scriptsFolder!, parentPath, nodeName));
		}
	}
	#endregion
	
	#region Engine files manipulation
	public void UpdateEngineFiles()
	{
		EnqueueProjectTask(CreateEngineFilesUpdateTask(_projectSettings!.ProjectFolderPath));
		EnqueueProjectTask(CreateEngineFilesCheckTask(_projectSettings!.ProjectFolderPath));
	}
	#endregion
	
	#region Tasks
	private void EnqueueProjectTask(ProjectTask task)
	{
		_thread.Enqueue(task);
	}
	
	private ProjectTask CreateNewFolderNodeTask(ProjectRootNode root, string parentPath, string name, bool hasMetaFile, ProjectNodeType type)
	{
		return new ProjectTask(
			cancellationToken =>
			{
				lock (ProjectTreeLock)
				{
					if (root.FindNode(parentPath) is ProjectFolderNode parentNode)
					{
						if (parentNode.FindChild(name) == null)
						{
							ProjectFolderNode newNode = new(
								name,
								Path.Join(parentNode.AbsolutePath, name),
								Path.Join(parentNode.RelativePath, name),
								hasMetaFile,
								type
							);

							parentNode.Children.Add(newNode);
						}
					}
				}
			}
		);
	}

	private ProjectTask CreateNewAssetFileTask(ProjectRootNode root, string parentPath, string name)
	{
		return new ProjectTask(
			cancellationToken =>
			{
				lock (ProjectTreeLock)
				{
					if (root.FindNode(parentPath) is ProjectFolderNode parentNode)
					{
						if (parentNode.FindChild(name) == null)
						{
							ProjectAssetFileNode newNode = new(
								name,
								Path.Join(parentNode.AbsolutePath, name),
								Path.Join(parentNode.RelativePath, name)
							);

							parentNode.Children.Add(newNode);
						}
					}
				}
			}
		);
	}

	private ProjectTask CreateNewScriptFileTask(ProjectRootNode root, string parentPath, string name)
	{
		return new ProjectTask(
			cancellationToken =>
			{
				lock (ProjectTreeLock)
				{
					if (root.FindNode(parentPath) is ProjectFolderNode parentNode)
					{
						if (parentNode.FindChild(name) == null)
						{
							ProjectScriptFileNode newNode = new(
								name,
								Path.Join(parentNode.AbsolutePath, name),
								Path.Join(parentNode.RelativePath, name)
							);

							parentNode.Children.Add(newNode);
						}
					}
				}
			}
		);
	}
	
	private ProjectTask CreateInitMetaTask(ProjectRootNode root, string path)
	{
		return new ProjectTask(
			cancellationToken =>
			{
				lock (ProjectTreeLock)
				{
					ProjectNode? node = root.FindNode(path);
					if (node != null && node.Exists)
					{
						node.InitMeta(_migrationManager, _importerRegistry, cancellationToken);
					}
				}
			}
		);
	}

	private ProjectTask CreateResetMetaHashTask(ProjectRootNode root, string path)
	{
		return new ProjectTask(
			cancellationToken =>
			{
				lock (ProjectTreeLock)
				{
					ProjectNode? node = root.FindNode(path);
					if (node != null && node.Exists)
					{
						node.ResetMetaHash();
					}
				}
			}
		);
	}
	
	private ProjectTask CreateImportTask(ProjectRootNode root, string path)
	{
		return new ProjectTask(
			cancellationToken =>
			{
				// check node
				ProjectAssetFileNode? node = root.FindNode(path) as ProjectAssetFileNode;
				if (node == null || node.Meta == null)
				{
					return;
				}

				// check file
				if (!File.Exists(node.AbsolutePath))
				{
					return;
				}

				// calculate hash
				string? hash = null;
				try
				{
					using FileStream file = new(node.AbsolutePath, FileMode.Open);
					hash = Convert.ToBase64String(_hashAlgorithm.ComputeHash(file)); // how to cancel compute hash?
				}
				catch (Exception e)
				{
					return;
				}

				// check hash
				if (node.Meta.Hash == hash)
				{
					return;
				}

				// import
				Importer importer = _importerRegistry.GetImporter(node.Extension);
				node.Meta!.ImporterSettings ??= importer.CreateSettings();

				string resourcePath = Path.Combine(_projectSettings!.AbsoluteResourcesPath, node.RelativePath);

				try
				{
					using Stream   stream = File.OpenRead(node.AbsolutePath);
					ImporterResult result = importer.Import(stream, node.Meta.ImporterSettings, resourcePath, _migrationManager, cancellationToken);

					if (result == ImporterResult.FinishedSettingsChanged)
					{
						node.UpdateMetaFile();
					}
				}
				catch (Exception e)
				{
					ConsoleViewModel.LogError($"While importing file {node.AbsolutePath} an exception occured: {e}");
					return;
				}

				// update hash
				node.Meta.SetHash(hash);
			}
		);
	}
	
	private ProjectTask CreateDeleteNodeTask(ProjectRootNode root, string parentPath, string name)
	{
		return new ProjectTask(
			cancellationToken =>
			{
				lock (ProjectTreeLock)
				{
					ProjectFolderNode? parent = root.FindNode(parentPath) as ProjectFolderNode;
					if (parent != null)
					{
						int index = parent.Children.FindIndex(child => child.Name == name);
						if (index == -1)
						{
							return;
						}

						ProjectNode child = parent.Children[index];
						File.Delete(child.AbsolutePath + ".meta");
						parent.Children.RemoveAt(index);
					}
				}
			}
		);
	}

	private ProjectTask CreateEngineFilesCheckTask(string projectRootPath)
	{
		return new ProjectTask(
			cancellationToken =>
			{
				NeedsUpdateEngineFiles = TemplateUtility.NeedsUpdateFromTemplate(projectRootPath);
			}
		);
	}

	private ProjectTask CreateEngineFilesUpdateTask(string projectRootPath)
	{
		return new ProjectTask(
			cancellationToken =>
			{
				TemplateUtility.UpdateLibrariesFromTemplate(projectRootPath);
			}
		);
	}

	private ProjectTask CreateClearResourcesTask(string absoluteResourcesPath)
	{
		return new ProjectTask(
			cancellationToken =>
			{
				if (Directory.Exists(absoluteResourcesPath))
				{
					Directory.Delete(absoluteResourcesPath, true);
				}

				Directory.CreateDirectory(absoluteResourcesPath);
			}
		);
	}
	#endregion
}