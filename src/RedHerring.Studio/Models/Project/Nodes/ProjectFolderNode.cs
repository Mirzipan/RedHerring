using System.Collections.ObjectModel;
using Migration;

namespace RedHerring.Studio.Models.Project.FileSystem;

public class ProjectFolderNode : AProjectNode
{
	public ObservableCollection<AProjectNode> Children { get; init; } = new();
	
	public ProjectFolderNode(string name, string path, string relativePath) : base(name, path, relativePath)
	{
	}

	public override async Task InitMetaRecursive(MigrationManager migrationManager)
	{
		await InitMeta(migrationManager, null);
		
		foreach (AProjectNode child in Children)
		{
			await child.InitMetaRecursive(migrationManager);
		}
	}

	public override void TraverseRecursive(Action<AProjectNode> process, CancellationToken cancellationToken)
	{
		process(this);

		foreach (AProjectNode child in Children)
		{
			if(cancellationToken.IsCancellationRequested)
			{
				return;
			}
			
			child.TraverseRecursive(process, cancellationToken);
		}
	}
}