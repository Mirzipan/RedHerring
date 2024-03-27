using Migration;
using RedHerring.Studio.Models.Project.Importers;

namespace RedHerring.Studio.Models.Project.FileSystem;

public sealed class ProjectRootNode : ProjectFolderNode
{
	public ProjectRootNode(string name, string absolutePath, ProjectNodeKind kind) : base(name, absolutePath, "", false, kind)
	{
	}

	public override void Init(MigrationManager migrationManager, CancellationToken cancellationToken)
	{
		Meta = new Metadata
		       {
			       Guid = Name,
			       Hash = $"#{Name}" // # is not valid base64 symbol, so this hash will be unique no matter what Name is
		       };
	}
}