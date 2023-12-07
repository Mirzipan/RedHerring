using Migration;

namespace RedHerring.Studio.Models.Project.FileSystem;

public sealed class ProjectRootNode : ProjectFolderNode
{
	public ProjectRootNode(string name, string path) : base(name, path, "")
	{
	}

	public override void InitMeta(MigrationManager migrationManager, CancellationToken cancellationToken)
	{
		Meta = new Metadata
		       {
			       Guid = Name,
			       Hash = $"#{Name}" // # is not valid base64 symbol, so this hash will be unique no matter what Name is
		       };
	}
}