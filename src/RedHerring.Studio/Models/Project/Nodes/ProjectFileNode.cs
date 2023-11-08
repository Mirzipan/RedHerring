using System.Security.Cryptography;
using Migration;

namespace RedHerring.Studio.Models.Project.FileSystem;

public sealed class ProjectFileNode : AProjectNode
{
	private static readonly HashAlgorithm _hashAlgorithm = SHA1.Create();
	
	public ProjectFileNode(string name, string path, string relativePath) : base(name, path, relativePath)
	{
	}

	public override async Task InitMetaRecursive(MigrationManager migrationManager)
	{
		// calculate file hash
		string hash;
		await using(FileStream file = new (Path, FileMode.Open))
		{
			hash = Convert.ToBase64String(await _hashAlgorithm.ComputeHashAsync(file));
		}
		
		// init meta
		await InitMeta(migrationManager, hash);
	}

	public override void TraverseRecursive(Action<AProjectNode> process, CancellationToken cancellationToken)
	{
		process(this);
	}
}