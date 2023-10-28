using Migration;
using RedHerring.Studio.Models.Project.Importers;

namespace RedHerring.Studio.Models.Project.FileSystem;

[Serializable, SerializedClassId("metadata-class-id")]
public class Metadata
{
	public string? Guid    = null;
	public string? Hash    = null;
	
	public List<AnImporterSettings>? ImporterSettings = null;

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

[Serializable, LatestVersion(typeof(Metadata))]
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
#endregion