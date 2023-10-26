using System.Collections.ObjectModel;
using Migration;

namespace RedHerring.Studio.Models.Project.FileSystem;

public class ProjectFolderNode : AProjectNode
{
	public ObservableCollection<AProjectNode> Children { get; init; } = new();
	
	public ProjectFolderNode(string name, string path) : base(name, path)
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
}