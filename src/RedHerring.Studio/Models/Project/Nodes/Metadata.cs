using Migration;
using RedHerring.Studio.Models.Project.Importers;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.Models.Project.FileSystem;

[Serializable, SerializedClassId("metadata-class-id")]
public class Metadata
{
	[ReadOnlyInInspector] public string? Guid    = null;
	[HideInInspector] public string? Hash    = null;
	
	[AllowDeleteReference] public ImporterSettings? ImporterSettings = null;

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
	
	[MigrateField] public IImporterSettingsMigratable? ImporterSettings;
	
	public void Migrate(Metadata_001 prev)
	{
		Guid = prev.Guid;
		Hash = null; // to force reimport
		
		ImporterSettings = null;
	}
}
#endregion