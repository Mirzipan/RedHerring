using System.Reflection;
using Migration;
using RedHerring.Studio.Models.Project.FileSystem;
using RedHerring.Studio.Models.Project.Importers;

namespace RedHerring.Studio.Models.Project;

public sealed class ProjectModel
{
	private const string _assetsFolderName = "Assets";
	private const string _settingsFileName = "Project.json";

	public static Assembly Assembly => typeof(ProjectModel).Assembly; 

	private readonly MigrationManager _migrationManager;
	private readonly ImporterRegistry _importerRegistry;
	
	private ProjectFolderNode? _assetsFolder;
	public  ProjectFolderNode? AssetsFolder => _assetsFolder;

	private ProjectSettings? _projectSettings;
	public  ProjectSettings ProjectSettings => _projectSettings!;
	
	private readonly ProjectThread _thread = new ();
	
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
		_thread.ClearQueue();
		
		SaveSettings();
		_assetsFolder = null;
	}
	
	public void Open(string projectPath)
	{
		LoadSettings(projectPath);

		string            assetsPath   = Path.Join(projectPath, _assetsFolderName);
		ProjectFolderNode assetsFolder = new ProjectRootNode(_assetsFolderName, assetsPath);

		if (!Directory.Exists(assetsPath))
		{
			// error
			Console.WriteLine($"Assets folder not found on path {assetsPath}");
			_assetsFolder = assetsFolder;
			InitMeta();
			return;
		}
		
		//Console.WriteLine($"Scan project path {projectPath}");

		try
		{
			RecursiveScan(assetsPath, "", assetsFolder);
		}
		catch (Exception e)
		{
			Console.WriteLine($"Exception: {e}");
		}

		_assetsFolder = assetsFolder;
		InitMeta();
		ImportAll();
	}

	private void RecursiveScan(string path, string relativePath, ProjectFolderNode root)
	{
		// scan directories
		foreach (string directoryPath in Directory.EnumerateDirectories(path))
		{
			string            directory             = Path.GetFileName(directoryPath);
			string            relativeDirectoryPath = Path.Combine(relativePath, directory);
			ProjectFolderNode folderNode            = new(directory, directoryPath, relativeDirectoryPath);
			root.Children.Add(folderNode);
			
			RecursiveScan(directoryPath, relativeDirectoryPath, folderNode);
		}

		// scan files except meta
		foreach (string filePath in Directory.EnumerateFiles(path))
		{
			string fileName = Path.GetFileName(filePath);
			if (fileName.EndsWith(".meta"))
			{
				continue;
			}

			string          relativeFilePath = Path.Combine(relativePath, fileName);
			ProjectFileNode fileNode         = new(fileName, filePath, relativeFilePath);
			root.Children.Add(fileNode);
		}
	}

	private void InitMeta()
	{
		_assetsFolder!.TraverseRecursive(
			node => _thread.Enqueue(new ProjectTask(cancellationToken => node.InitMeta(_migrationManager, cancellationToken))),
			TraverseFlags.Directories | TraverseFlags.Files,
			default
		);
	}

	#endregion
	
	#region Import
	public void ImportAll()
	{
		// TODO - delete everything from Resources?

		_assetsFolder!.TraverseRecursive(ImportProjectNode, TraverseFlags.Files, default);
	}

	private void ImportProjectNode(ProjectNode node)
	{
		_thread.Enqueue(
			new ProjectTask(
				cancellationToken =>
				{
					Importer importer = _importerRegistry.GetImporter(node.Extension);
					node.Meta.ImporterSettings ??= importer.CreateSettings();

					string resourcePath = Path.Combine(_projectSettings!.AbsoluteResourcesPath, node.RelativePath);

					using Stream stream = File.OpenRead(node.Path);
					ImporterResult result = importer.Import(stream, node.Meta.ImporterSettings, resourcePath, cancellationToken);

					if (result == ImporterResult.FinishedSettingsChanged)
					{
						node.UpdateMetaFile();
					}
				}
			)
		);
	}

	#endregion
	
	#region Settings
	public void SaveSettings()
	{
		if (_projectSettings == null)
		{
			return;
		}

		byte[] json = MigrationSerializer.SerializeAsync(_projectSettings, SerializedDataFormat.JSON, Assembly).Result;
		File.WriteAllBytes(Path.Join(_projectSettings.GameFolderPath, _settingsFileName), json);
	}

	public void LoadSettings(string projectPath)
	{
		string path = Path.Join(projectPath, _settingsFileName);
		if(!File.Exists(path))
		{
			_projectSettings = new ProjectSettings
                              {
                                  GameFolderPath = projectPath
                              };
			return;
		}
		
		byte[] json = File.ReadAllBytes(path);
		ProjectSettings settings = MigrationSerializer.DeserializeAsync<ProjectSettings, IStudioSettingsMigratable>(_migrationManager.TypesHash, json, SerializedDataFormat.JSON, _migrationManager, false, Assembly).Result;
		settings.GameFolderPath = projectPath;
		
		_projectSettings = settings;
	}
	#endregion
}