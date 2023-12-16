using System.Globalization;

namespace RedHerring.Studio;

public static class FileUtility
{
	public static void Copy(string sourceDirectory, string targetDirectory)
	{
		DirectoryInfo diSource = new (sourceDirectory);
		DirectoryInfo diTarget = new (targetDirectory);

		Copy(diSource, diTarget);
	}	
	
	public static void Copy(DirectoryInfo source, DirectoryInfo target)
	{
		Directory.CreateDirectory(target.FullName);

		// Copy each file into the new directory.
		foreach (FileInfo fileInfo in source.GetFiles())
		{
			fileInfo.CopyTo(Path.Combine(target.FullName, fileInfo.Name), true);
		}

		// Copy each subdirectory using recursion.
		foreach (DirectoryInfo sourceDirectory in source.GetDirectories())
		{
			DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(sourceDirectory.Name);
			Copy(sourceDirectory, nextTargetSubDir);
		}
	}

	public static void ReplaceTextInFile(string path, string find, string replaceBy)
	{
		string text = File.ReadAllText(path);
		text = text.Replace(find, replaceBy, false, CultureInfo.InvariantCulture);
		File.WriteAllText(path, text);
	}

	public static void ReplaceTextInFilesRecursive(string path, Func<string,bool> fileNameFilter, string find, string replaceBy)
	{
		DirectoryInfo source = new (path);
		ReplaceTextInFilesRecursive(source, fileNameFilter, find, replaceBy);
	}

	public static void ReplaceTextInFilesRecursive(DirectoryInfo directory, Func<string,bool> fileNameFilter, string find, string replaceBy)
	{
		foreach (FileInfo fileInfo in directory.GetFiles())
		{
			if (fileNameFilter(fileInfo.Name))
			{
				ReplaceTextInFile(fileInfo.FullName, find, replaceBy);
			}
		}

		foreach (DirectoryInfo subDirectory in directory.GetDirectories())
		{
			ReplaceTextInFilesRecursive(subDirectory, fileNameFilter, find, replaceBy);
		}
	}
}