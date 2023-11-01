using System.Reflection;
using Migration;
using RedHerring.Studio.Models.Project.FileSystem;
using RedHerring.Studio.Models.Project.Importers;

namespace RedHerring.Studio.Models.Project;

public sealed class ProjectModel
{
	private const string _assetsFolderName = "Assets";

	private readonly MigrationManager _migrationManager;
	
	private ProjectFolderNode? _assetsFolder;
	public  ProjectFolderNode? AssetsFolder => _assetsFolder;

	public ProjectModel(MigrationManager migrationManager)
	{
		_migrationManager = migrationManager;
	}
	
	#region Open/close
	public void Close()
	{
		_assetsFolder = null;
	}
	
	public async Task Open(string projectPath)
	{
		string assetsPath = Path.Join(projectPath, _assetsFolderName);
		ProjectFolderNode assetsFolder = new ProjectRootNode(_assetsFolderName, assetsPath);

		if (!Directory.Exists(assetsPath))
		{
			// error
			Console.WriteLine($"Assets folder not found on path {assetsPath}");
			await assetsFolder.InitMetaRecursive(_migrationManager); // create meta for at least root
			return;
		}
		
		//Console.WriteLine($"Scan project path {projectPath}");

		try
		{
			RecursiveScan(assetsPath, assetsFolder);
		}
		catch (Exception e)
		{
			Console.WriteLine($"Exception: {e}");
		}

		await assetsFolder.InitMetaRecursive(_migrationManager);
		_assetsFolder = assetsFolder;
	}

	private void RecursiveScan(string path, ProjectFolderNode root)
	{
		// scan directories
		foreach (string directoryPath in Directory.EnumerateDirectories(path))
		{
			string     directory  = Path.GetFileName(directoryPath);
			ProjectFolderNode folderNode = new(directory, directoryPath);
			root.Children.Add(folderNode);
			
			RecursiveScan(directoryPath, folderNode);
		}

		// scan files except meta
		foreach (string filePath in Directory.EnumerateFiles(path))
		{
			string fileName = Path.GetFileName(filePath);
			if (fileName.EndsWith(".meta"))
			{
				continue;
			}

			ProjectFileNode fileNode = new(fileName, filePath);
			root.Children.Add(fileNode);
		}
	}
	#endregion
	
	#region Import
	
	#endregion
}