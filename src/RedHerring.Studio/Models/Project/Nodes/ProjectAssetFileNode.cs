using System.Security.Cryptography;
using Migration;
using RedHerring.Studio.Models.Project.Importers;

namespace RedHerring.Studio.Models.Project.FileSystem;

public sealed class ProjectAssetFileNode : ProjectNode
{
	public override string RelativeDirectoryPath => RelativePath.Substring(0, RelativePath.Length - Name.Length);
	public override bool   Exists                => File.Exists(AbsolutePath);
	
	private static readonly HashAlgorithm _hashAlgorithm = SHA1.Create();
	
	public ProjectAssetFileNode(string name, string absolutePath, string relativePath) : base(name, absolutePath, relativePath, true)
	{
	}

	public override void InitMeta(MigrationManager migrationManager, ImporterRegistry importerRegistry, CancellationToken cancellationToken)
	{
		// CreateMetaFile(migrationManager);
		//
		// if (Meta == null)
		// {
		// 	return;
		// }
		//
		// if (Meta.ImporterSettings == null)
		// {
		// 	Importer importer = importerRegistry.GetImporter(Extension);
		// 	Meta.ImporterSettings = importer.CreateSettings();
		// }
		//
		// SetNodeType(Meta.ImporterSettings.NodeType);
	}

	public override void TraverseRecursive(Action<ProjectNode> process, TraverseFlags flags, CancellationToken cancellationToken)
	{
		if ((flags & TraverseFlags.Files) != 0)
		{
			process(this);
		}
	}

	public override ProjectNode? FindNode(string path)
	{
		return null;
	}
}