using Migration;

namespace RedHerring.Studio.Models.Project.FileSystem;

public sealed class ProjectRootNode : ProjectFolderNode
{
	public ProjectRootNode(string name, string path) : base(name, path, "")
	{
	}

	public override async Task InitMetaRecursive(MigrationManager migrationManager)
	{
		Meta = new Metadata
		       {
			       Guid = Name,
			       Hash = $"#{Name}"		// # is not valid base64 symbol, so this hash will be unique no matter what Name is
		       };
		
		foreach (AProjectNode child in Children)
		{
			await child.InitMetaRecursive(migrationManager);
		}
	}
}