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
			// TODO - how to access engine from here?
			//byte[]? bytes = Engine.Resources.ReadResource(Path);
			//return bytes;
			return null;
		}
		catch (Exception e)
		{
			return null;
		}
	}
}