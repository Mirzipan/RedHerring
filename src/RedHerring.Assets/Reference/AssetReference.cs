namespace RedHerring.Assets;

[Serializable]
public sealed class AssetReference : Reference<byte[]>
{
	public AssetReference(string path) : base(path)
	{
	}

	public override byte[]? LoadValue()
	{
		try
		{
			byte[]? bytes = Resources.ReadResource(Path);
			return bytes;
		}
		catch (Exception e)
		{
			return null;
		}
	}
}