using System.Collections.ObjectModel;
using Migration;

namespace RedHerring.Studio.Models.Project.FileSystem;

public class ProjectFolderNode : ProjectNode
{
	public ObservableCollection<ProjectNode> Children { get; init; } = new();
	
	public ProjectFolderNode(string name, string path, string relativePath) : base(name, path, relativePath)
	{
	}

	public override void InitMeta(MigrationManager migrationManager, CancellationToken cancellationToken)
	{
		CreateMetaFile(migrationManager, null);
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
}