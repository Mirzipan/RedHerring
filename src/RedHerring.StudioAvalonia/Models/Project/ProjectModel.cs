using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Migration;

namespace RedHerring.Studio.Models;

public sealed class ProjectModel : INotifyPropertyChanged
{
	private const string _assetsFolderName = "Assets";

	public static    Assembly         Assembly => typeof(ProjectModel).Assembly; 
	private readonly MigrationManager _migrationManager = new(Assembly);
	
	private FolderNode _assetsFolder;
	public  FolderNode AssetsFolder => _assetsFolder;

	public event PropertyChangedEventHandler? PropertyChanged;
	
	public async Task Open(string projectPath)
	{
		string assetsPath = Path.Join(projectPath, _assetsFolderName);
		_assetsFolder = new FolderNode(_assetsFolderName, assetsPath);

		if (!Directory.Exists(assetsPath))
		{
			// this is error - TODO
			OnPropertyChanged(nameof(AssetsFolder));
			return;
		}
		
		//Console.WriteLine($"Scan project path {projectPath}");

		try
		{
			RecursiveScan(assetsPath, _assetsFolder);
		}
		catch (Exception e)
		{
			Console.WriteLine($"Exception: {e}");
		}

		await _assetsFolder.InitMetaRecursive(_migrationManager);
		
		OnPropertyChanged(nameof(AssetsFolder));
	}

	private void RecursiveScan(string path, FolderNode root)
	{
		// scan directories
		foreach (string directoryPath in Directory.EnumerateDirectories(path))
		{
			string     directory  = Path.GetFileName(directoryPath);
			FolderNode folderNode = new(directory, directoryPath);
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

			FileNode fileNode = new(fileName, filePath);
			root.Children.Add(fileNode);
		}
	}

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	// private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
	// {
	// 	if (EqualityComparer<T>.Default.Equals(field, value)) return false;
	// 	field = value;
	// 	OnPropertyChanged(propertyName);
	// 	return true;
	// }
}