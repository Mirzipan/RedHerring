using Migration;
using RedHerring.Studio.Models.Project.Importers;

namespace RedHerring.Studio.Models.Project.FileSystem;

public sealed class ProjectRootNode : ProjectFolderNode
{
	public ProjectRootNode(string name, string absolutePath, ProjectNodeType type) : base(name, absolutePath, "", false, type)
	{
	}

	public override void InitMeta(MigrationManager migrationManager, ImporterRegistry importerRegistry, CancellationToken cancellationToken)
	{
		Meta = new Metadata
		       {
			       Guid = Name,
			       Hash = $"#{Name}" // # is not valid base64 symbol, so this hash will be unique no matter what Name is
		       };
	}
}