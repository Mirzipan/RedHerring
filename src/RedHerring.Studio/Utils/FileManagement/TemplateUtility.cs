using System.Security.Cryptography;

namespace RedHerring.Studio;

public static class TemplateUtility
{
	private static readonly HashAlgorithm _hashAlgorithm = SHA1.Create();
	
	private const string _templatePath  = "Template";
	private const string _librariesPath = "Libraries";

	// copy template to target path and rename to project name
	// targetPath is path to project directory (including name of project directory at the end)
	public static void InstantiateTemplate(string targetPath, string projectName)
	{
		FileUtility.Copy(_templatePath, targetPath);
		
		File.Move(Path.Combine(targetPath, "Template.sln"), Path.Combine(targetPath, $"{projectName}.sln"));
		
		FileUtility.ReplaceTextInFilesRecursive(targetPath, fileName => fileName.EndsWith(".cs"), "Template", projectName);
	}

	// check if libraries in target path are up to date with template
	public static bool NeedsUpdateFromTemplate(string targetPath)
	{
		string sourceLibrariesPath = Path.Combine(_templatePath, _librariesPath);
		string targetLibrariesPath = Path.Combine(targetPath,    _librariesPath);

		DirectoryInfo sourceDirectory = new DirectoryInfo(sourceLibrariesPath);
		
		foreach (FileInfo fileInfo in sourceDirectory.GetFiles())
		{
			string targetFilePath = Path.Combine(targetLibrariesPath, fileInfo.Name);
			if (!File.Exists(targetFilePath))
			{
				return true;
			}

			byte[]? sourceHash = null;
			using (FileStream file = new(fileInfo.FullName, FileMode.Open))
			{
				sourceHash = _hashAlgorithm.ComputeHash(file);
			}

			byte[]? targetHash = null;
			using (FileStream file = new(targetFilePath, FileMode.Open))
			{
				targetHash = _hashAlgorithm.ComputeHash(file);
			}

			if (!sourceHash.SequenceEqual(targetHash))
			{
				return true;
			}
		}

		return false;
	}
	
	// update just libraries in target path from template
	public static void UpdateLibrariesFromTemplate(string targetPath)
	{
		string sourceLibrariesPath = Path.Combine(_templatePath, _librariesPath);
		string targetLibrariesPath = Path.Combine(targetPath,    _librariesPath);

		FileUtility.Copy(sourceLibrariesPath, targetLibrariesPath);
	}
}