namespace RedHerring.Studio;

public sealed class StudioAssetDatabaseItem
{
	public string  Guid          = null!;
	public string? Field         = null;
	public string  Path          = null!;
	public string  ReferenceType = "Reference";

	public StudioAssetDatabaseItem()
	{
	}

	public StudioAssetDatabaseItem(string guid, string? field, string path, string referenceType)
	{
		Guid          = guid;
		Field         = field;
		Path          = path;
		ReferenceType = referenceType;
	}
}