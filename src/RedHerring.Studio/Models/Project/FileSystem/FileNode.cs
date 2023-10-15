using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Migration;

namespace RedHerring.Studio.Models;

public sealed class FileNode : FileSystemNode
{
	private static readonly HashAlgorithm _hashAlgorithm = SHA1.Create();
	
	public FileNode(string name, string path) : base(name, path)
	{
	}

	public override async Task InitMetaRecursive(MigrationManager migrationManager)
	{
		// calculate file hash
		string hash;
		await using(FileStream file = new (Path, FileMode.Open))
		{
			hash = Convert.ToBase64String(await _hashAlgorithm.ComputeHashAsync(file));
			await InitMeta(migrationManager, hash);
		}
		
		// init meta
		await InitMeta(migrationManager, hash);
	}
}