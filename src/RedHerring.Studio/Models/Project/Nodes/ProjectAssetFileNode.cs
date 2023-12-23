using System.Security.Cryptography;
using Migration;

namespace RedHerring.Studio.Models.Project.FileSystem;

public sealed class ProjectAssetFileNode : ProjectNode
{
	public override string RelativeDirectoryPath => RelativePath.Substring(0, RelativePath.Length - Name.Length);
	public override bool   Exists                => File.Exists(AbsolutePath);
	
	private static readonly HashAlgorithm _hashAlgorithm = SHA1.Create();
	
	public ProjectAssetFileNode(string name, string absolutePath, string relativePath) : base(name, absolutePath, relativePath, true)
	{
	}

	public override void InitMeta(MigrationManager migrationManager, CancellationToken cancellationToken)
	{
		// calculate file hash
		string hash;
		try
		{
			using FileStream file = new(AbsolutePath, FileMode.Open);
			hash = Convert.ToBase64String(_hashAlgorithm.ComputeHash(file)); // how to cancel compute hash?
		}
		catch (Exception e)
		{
			hash = "";
		}

		// init meta
		CreateMetaFile(migrationManager, hash);
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