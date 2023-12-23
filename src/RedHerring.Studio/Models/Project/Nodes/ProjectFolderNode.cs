using Migration;

namespace RedHerring.Studio.Models.Project.FileSystem;

public class ProjectFolderNode : ProjectNode
{
	public override string RelativeDirectoryPath => RelativePath;
	public override bool   Exists                => Directory.Exists(AbsolutePath);

	public List<ProjectNode> Children { get; } = new();
	
	public ProjectFolderNode(string name, string absolutePath, string relativePath, bool hasMetaFile, ProjectNodeType type) : base(name, absolutePath, relativePath, hasMetaFile)
	{
		SetNodeType(type);
	}

	public override void InitMeta(MigrationManager migrationManager, CancellationToken cancellationToken)
	{
		if (HasMetaFile)
		{
			CreateMetaFile(migrationManager, null);
		}
		else
		{
			Meta = new Metadata
			       {
				       Guid = RelativePath,
				       Hash = ""
			       };
		}
	}

	public override void TraverseRecursive(Action<ProjectNode> process, TraverseFlags flags, CancellationToken cancellationToken)
	{
		if ((flags & TraverseFlags.Directories) != 0)
		{
			process(this);
		}

		foreach (ProjectNode child in Children)
		{
			if(cancellationToken.IsCancellationRequested)
			{
				return;
			}
			
			child.TraverseRecursive(process, flags, cancellationToken);
		}
	}

	public override ProjectNode? FindNode(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return this;
		}

		int index = path.IndexOfAny(new [] {'\\','/'});
		if (index == -1)
		{
			// end of path
			return Children.FirstOrDefault(node => node.Name == path);
		}

		string       folderName = path.Substring(0, index);
		ProjectNode? node       = Children.FirstOrDefault(node => node.Name == folderName);

		if (node == null)
		{
			// not found
			return null;
		}

		// continue with shorter path
		return node.FindNode(path.Substring(folderName.Length + 1));
	}

	public ProjectNode? FindChild(string name)
	{
		return Children.FirstOrDefault(child => child.Name == name);
	}
}