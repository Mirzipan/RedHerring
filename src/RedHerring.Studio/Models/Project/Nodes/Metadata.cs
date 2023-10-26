using Migration;

namespace RedHerring.Studio.Models.Project.FileSystem;

[Serializable, SerializedClassId("metadata-class-id")]
public class Metadata
{
	public string? Guid = null;
	public string? Hash  = null;

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
    
[Serializable, LatestVersion(typeof(Metadata))]
public class Metadata_000 : IMetadataMigratable
{
	public string? Guid = null;
	public string? Hash = null;
}
#endregion