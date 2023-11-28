using System.Collections.ObjectModel;
using Migration;

namespace RedHerring.Studio.Models.Project.FileSystem;

public class ProjectFolderNode : ProjectNode
{
	public ObservableCollection<ProjectNode> Children { get; init; } = new();
	
	public ProjectFolderNode(string name, string path, string relativePath) : base(name, path, relativePath)
	{
	}

	public override async Task InitMetaRecursive(MigrationManager migrationManager)
	{
		await InitMeta(migrationManager, null);
		
		foreach (ProjectNode child in Children)
		{
			await child.InitMetaRecursive(migrationManager);
		}
	}

	public override void TraverseRecursive(Action<ProjectNode> process, CancellationToken cancellationToken)
	{
		process(this);

		foreach (ProjectNode child in Children)
		{
			if(cancellationToken.IsCancellationRequested)
			{
				return;
			}
			
			child.TraverseRecursive(process, cancellationToken);
		}
	}
}