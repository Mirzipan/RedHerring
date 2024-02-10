namespace RedHerring.Assets;

public readonly struct ResourceDescriptor
{
	public readonly ResourceSourceKind ResourceSourceKind;
	public readonly string             SourceFilePath;

	public ResourceDescriptor(ResourceSourceKind resourceSourceKind, string sourceFilePath)
	{
		ResourceSourceKind = resourceSourceKind;
		SourceFilePath     = sourceFilePath;
	}
}