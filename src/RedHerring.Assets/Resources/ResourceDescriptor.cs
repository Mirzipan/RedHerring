namespace RedHerring.Assets;

public readonly struct ResourceDescriptor
{
	public readonly ResourceSourceType ResourceSourceType;
	public readonly string             SourceFilePath;

	public ResourceDescriptor(ResourceSourceType resourceSourceType, string sourceFilePath)
	{
		ResourceSourceType = resourceSourceType;
		SourceFilePath     = sourceFilePath;
	}
}