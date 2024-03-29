using Migration;
using RedHerring.Studio.Models.Project.Importers;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models.Project.FileSystem;

[Serializable, SerializedClassId("0c8a6450-cc04-4456-8335-02d1aac7e7cd")]
public class Metadata
{
	[ReadOnlyInInspector] public string? Guid  = null;
	[ReadOnlyInInspector] public string? Hash  = null;
	public                       string? ReferenceField = null;
	
	public ImporterSettings? ImporterSettings = null;

	public void UpdateGuid()
	{
		if (string.IsNullOrEmpty(Guid))
		{
			Guid = System.Guid.NewGuid().ToString();
		}
	}
	
	public void SetHash(string? hash)
	{
		Hash = hash;
	}
}

#region Migration
[MigratableInterface(typeof(Metadata))]
public interface IMetadataMigratable
{
}
    
[Serializable, ObsoleteVersion(typeof(Metadata))]
public class Metadata_000 : IMetadataMigratable
{
	public string? Guid;
	public string? Hash;
}

[Serializable, ObsoleteVersion(typeof(Metadata))]
public class Metadata_001 : IMetadataMigratable
{
	public string? Guid;
	public string? Hash;
	
	[MigrateField] public List<IImporterSettingsMigratable>? ImporterSettings;
	
	public void Migrate(Metadata_000 prev)
	{
		Guid = prev.Guid;
		Hash = null; // to force reimport
		
		ImporterSettings = new List<IImporterSettingsMigratable>();
	}
}

[Serializable, LatestVersion(typeof(Metadata))]
public class Metadata_002 : IMetadataMigratable
{
	public string? Guid;
	public string? Hash;
	public string? ReferenceField;
	
	[MigrateField] public IImporterSettingsMigratable? ImporterSettings;
	
	public void Migrate(Metadata_001 prev)
	{
		Guid  = prev.Guid;
		Hash  = null; // to force reimport
		ReferenceField = null;
		
		ImporterSettings = null;
	}
}
#endregion
