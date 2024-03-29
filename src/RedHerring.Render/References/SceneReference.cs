using OdinSerializer;
using RedHerring.Assets;
using RedHerring.Render.Models;

namespace RedHerring.Render;

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
			byte[]? bytes = Resources.ReadResource(Path);
			if (bytes is null)
			{
				return null;
			}
			
			return SerializationUtility.DeserializeValue<Scene>(bytes, DataFormat.Binary);
		}
		catch (Exception e)
		{
			return null;
		}
	}
}