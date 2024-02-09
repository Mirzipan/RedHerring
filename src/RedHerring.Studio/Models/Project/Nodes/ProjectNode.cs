using Migration;
using RedHerring.Studio.Models.Project.Importers;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models.Project.FileSystem;

public abstract class ProjectNode
{
	[ReadOnlyInInspector] public ProjectNodeType Type = ProjectNodeType.Uninitialized;

	public          string Name { get; }
	public readonly string AbsolutePath;
	public readonly string RelativePath; // relative path inside Assets directory
	public abstract string RelativeDirectoryPath { get; }

	[ReadOnlyInInspector] public bool HasMetaFile;
	
	public Metadata? Meta;

	public          string Extension => System.IO.Path.GetExtension(AbsolutePath).ToLower(); // cache if needed
	public abstract bool   Exists    { get; }

	protected ProjectNode(string name, string absolutePath, string relativePath, bool hasMetaFile)
	{
		Name         = name;
		AbsolutePath         = absolutePath;
		RelativePath = relativePath;
		HasMetaFile  = hasMetaFile;
	}

	public abstract void InitMeta(MigrationManager migrationManager, ImporterRegistry importerRegistry, CancellationToken cancellationToken);

	public void ResetMetaHash()
	{
		Meta?.SetHash(null);
	}
	
	public void UpdateMetaFile()
	{
		string metaPath = $"{AbsolutePath}.meta";
		byte[] json     = MigrationSerializer.SerializeAsync(Meta, SerializedDataFormat.JSON, StudioModel.Assembly).GetAwaiter().GetResult();
		File.WriteAllBytes(metaPath, json);
	}

	public void SetNodeType(ProjectNodeType type)
	{
		Type = type;
	}

	protected void CreateMetaFile(MigrationManager migrationManager)
	{
		string metaPath = $"{AbsolutePath}.meta";
		
		 Metadata? meta = null;
		 if (File.Exists(metaPath))
		 {
		 	byte[] json = File.ReadAllBytes(metaPath);
		 	meta = MigrationSerializer.DeserializeAsync<Metadata, IMetadataMigratable>(null, json, SerializedDataFormat.JSON, migrationManager, true, StudioModel.Assembly).GetAwaiter().GetResult();
		 }
		
		 // write if needed
		 if(meta == null)
		 {
		 	meta ??= new Metadata();
		 	meta.UpdateGuid();
		
		 	byte[] json = MigrationSerializer.SerializeAsync(meta, SerializedDataFormat.JSON, StudioModel.Assembly).GetAwaiter().GetResult();
		 	File.WriteAllBytes(metaPath, json);
		 }
		
		 Meta = meta;
	}

	public abstract void TraverseRecursive(Action<ProjectNode> process, TraverseFlags flags, CancellationToken cancellationToken);

	public abstract ProjectNode? FindNode(string path);
}