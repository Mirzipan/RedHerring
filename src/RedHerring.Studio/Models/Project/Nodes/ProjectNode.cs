using Migration;
using RedHerring.Deduction;
using RedHerring.Studio.Models.Project.Importers;
using RedHerring.Studio.Models.ViewModels;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models.Project.FileSystem;

public abstract class ProjectNode : ISelectable
{
	[ReadOnlyInInspector] public ProjectNodeType Type = ProjectNodeType.Uninitialized;

	public          string Name { get; }
	public readonly string AbsolutePath;
	public readonly string RelativePath; // relative path inside Assets directory
	public abstract string RelativeDirectoryPath { get; }

	[ReadOnlyInInspector] public bool HasMetaFile;
	
	public    Metadata? Meta;
	protected Importer? Importer;

	public          string Extension => System.IO.Path.GetExtension(AbsolutePath).ToLower(); // cache if needed
	public abstract bool   Exists    { get; }

	protected ProjectNode(string name, string absolutePath, string relativePath, bool hasMetaFile)
	{
		Name         = name;
		AbsolutePath         = absolutePath;
		RelativePath = relativePath;
		HasMetaFile  = hasMetaFile;
	}

	public abstract void Init(MigrationManager migrationManager, CancellationToken cancellationToken);

	public void ResetMetaHash()
	{
		Meta?.SetHash(null);
	}

	public void ApplyChanges()
	{
		UpdateMetaFile();
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

	protected void InitImporter()
	{
		if (Meta is null)
		{
			return;
		}
		
		if (Importer is null || Meta.ImporterSettings is null)
		{
			var registry = Findings.IndexerByType<ImporterRegistry>();
			if (registry is not null)
			{
				Importer              = registry.CreateImporter(this);
				Meta.ImporterSettings = Importer.CreateImportSettings();
			}
			else
			{
				// TODO(mirzi): we might have an issue
				throw new NullReferenceException("Missing ImporterRegistry");
			}
		}
	}

	public T? GetImporter<T>() where T : Importer
	{
		return Importer as T;
	}
	
	public abstract void TraverseRecursive(Action<ProjectNode> process, TraverseFlags flags, CancellationToken cancellationToken);

	public abstract ProjectNode? FindNode(string path);
}