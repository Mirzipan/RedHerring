using RedHerring.Render.Models;

namespace RedHerring.Assets;

[Serializable]
public sealed class SceneReference : Reference<Scene>
{
	public SceneReference(string path) : base(path)
	{
	}

	public override Scene? LoadValue()
	{
		try
		{
			// TODO - how to access engine from here?
			// byte[]? bytes = Engine.Resources.ReadResource(Path);
			// if (bytes is null)
			// {
			// 	return null;
			// }
			//
			// return SerializationUtility.DeserializeValue<Model>(bytes, DataFormat.Binary);
			return null;
		}
		catch (Exception e)
		{
			return null;
		}
	}
}