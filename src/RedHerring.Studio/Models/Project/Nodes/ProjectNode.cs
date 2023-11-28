using Migration;

namespace RedHerring.Studio.Models.Project.FileSystem;

public abstract class ProjectNode
{
	public          string   Name { get; set; }
	public readonly string   Path;
	public readonly string   RelativePath; // relative path inside Assets directory
	public          Metadata Meta = null!;
	
	public string Extension => System.IO.Path.GetExtension(Path).ToLower();	// cache if needed

	protected ProjectNode(string name, string path, string relativePath)
	{
		Name         = name;
		Path         = path;
		RelativePath = relativePath;
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

	public abstract void TraverseRecursive(Action<ProjectNode> process, CancellationToken cancellationToken);
}