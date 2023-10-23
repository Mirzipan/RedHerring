using System.IO;
using System.Threading.Tasks;
using Migration;

namespace RedHerring.Studio.Models;

public abstract class FileSystemNode
{
	public string    Name { get; set; }
	public string    Path;
	public Metadata? Meta;

	protected FileSystemNode(string name, string path)
	{
		Name = name;
		Path = path;
	}

	public abstract Task InitMetaRecursive(MigrationManager migrationManager);

	protected async Task InitMeta(MigrationManager migrationManager, string? hash)
	{
		string metaPath = $"{Path}.meta";
		
		// read if possible
		if (File.Exists(metaPath))
		{
			byte[] json = await File.ReadAllBytesAsync(metaPath);
			Meta = await MigrationSerializer.DeserializeAsync<Metadata, IMetadataMigratable>(null, json, SerializedDataFormat.JSON, migrationManager, true);
		}
		
		// write if needed
		if(Meta == null || Meta.Hash == hash)
		{
			Meta ??= new Metadata();
			Meta.UpdateGuid();
			Meta.SetHash(hash);

			byte[] json =  await MigrationSerializer.SerializeAsync(Meta, SerializedDataFormat.JSON, ProjectModel.Assembly);
			await File.WriteAllBytesAsync(metaPath, json);
		}
	}
}