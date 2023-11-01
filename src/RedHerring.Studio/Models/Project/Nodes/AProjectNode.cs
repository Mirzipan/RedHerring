using Migration;

namespace RedHerring.Studio.Models.Project.FileSystem;

public abstract class AProjectNode
{
	public string    Name { get; set; }
	public string    Path;
	public Metadata Meta = null!;

	protected AProjectNode(string name, string path)
	{
		Name = name;
		Path = path;
	}

	public abstract Task InitMetaRecursive(MigrationManager migrationManager);

	protected async Task InitMeta(MigrationManager migrationManager, string? hash)
	{
		string metaPath = $"{Path}.meta";
		
		// read if possible
		Metadata? meta = null;
		if (File.Exists(metaPath))
		{
			byte[] json = await File.ReadAllBytesAsync(metaPath);
			meta = await MigrationSerializer.DeserializeAsync<Metadata, IMetadataMigratable>(null, json, SerializedDataFormat.JSON, migrationManager, true, StudioModel.Assembly);
		}
		
		// write if needed
		if(meta == null || meta.Hash != hash)
		{
			meta ??= new Metadata();
			meta.UpdateGuid();
			meta.SetHash(hash);

			byte[] json =  await MigrationSerializer.SerializeAsync(meta, SerializedDataFormat.JSON, StudioModel.Assembly);
			await File.WriteAllBytesAsync(metaPath, json);
		}

		Meta = meta;
	}
}