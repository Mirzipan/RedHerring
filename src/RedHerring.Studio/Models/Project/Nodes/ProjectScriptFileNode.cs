using Migration;
using RedHerring.Studio.Models.Project.Importers;

namespace RedHerring.Studio.Models.Project.FileSystem;

public class ProjectScriptFileNode : ProjectNode
{
	public override string RelativeDirectoryPath => RelativePath.Substring(0, RelativePath.Length - Name.Length);
	public override bool   Exists                => File.Exists(AbsolutePath);
	
	[Serializable]
	private class FileId
	{
		public string Guid { get; set; }
		public string Type { get; set; } // TODO - constants or something
	}

	public ProjectScriptFileNode(string name, string absolutePath, string relativePath) : base(name, absolutePath, relativePath, false)
	{
		SetNodeType(ProjectNodeType.ScriptFile);
	}

	public override void Init(MigrationManager migrationManager, CancellationToken cancellationToken)
	{
		string guid = RelativePath;
		
		// try to parse file header
		ProjectScriptFileHeader.FileId? fileId = ProjectScriptFileHeader.ReadFromFile(AbsolutePath);
		if(fileId != null)
		{
			guid = fileId.Guid;
			SetNodeType(ProjectNodeType.ScriptFile);
		}
		
		Meta = new Metadata
		       {
			       Guid = guid,
			       Hash = "",
		       };

		InitImporter();
		Importer!.UpdateCache();
	}

	public override void TraverseRecursive(Action<ProjectNode> process, TraverseFlags flags, CancellationToken cancellationToken)
	{
		if ((flags & TraverseFlags.Files) != 0)
		{
			process(this);
		}
	}

	public override ProjectNode? FindNode(string path)
	{
		return null;
	}

	public LoadingResult LoadFile()
	{
		try
		{
			string[] result = File.ReadAllLines(AbsolutePath);
			return new LoadingResult(result);
		}
		catch (FileNotFoundException)
		{
			return new LoadingResult(LoadingResultCode.FileNotFound);
		}
		catch (Exception)
		{
			return new LoadingResult(LoadingResultCode.Failed);
		}
	}

	public SavingResultCode SaveFile()
	{
		
		return SavingResultCode.Ok;
	}

	public struct LoadingResult
	{
		public string[]? Lines;
		public LoadingResultCode Code; 

		internal LoadingResult(string[] lines)
		{
			Lines = lines;
			Code = LoadingResultCode.Ok;
		}

		internal LoadingResult(LoadingResultCode code)
		{
			Code = code;
		}
	}

	public enum LoadingResultCode
	{
		Ok = 0,
		Failed = 1,
		FileNotFound = 2,
	}

	public enum SavingResultCode
	{
		Ok = 0,
		FileAccessFailed = 1,
	}
}