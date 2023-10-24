using System.Collections.ObjectModel;
using Migration;

namespace RedHerring.Studio.Models.Project.FileSystem;

public sealed class FolderNode : FileSystemNode
{
	public ObservableCollection<FileSystemNode> Children { get; init; } = new();
	
	public FolderNode(string name, string path) : base(name, path)
	{
	}

	public override async Task InitMetaRecursive(MigrationManager migrationManager)
	{
		await InitMeta(migrationManager, null);
		
		foreach (FileSystemNode child in Children)
		{
			await child.InitMetaRecursive(migrationManager);
		}
	}
}