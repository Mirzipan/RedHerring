using ImageMagick;
using OdinSerializer;
using RedHerring.Assets;

namespace RedHerring.Render;

public sealed class ImageReference : Reference<Image>
{
	public ImageReference(string path) : base(path)
	{
	}

	public override Image? LoadValue()
	{
		try
		{
			byte[]? bytes = Resources.ReadResource(Path);
			if (bytes is null)
			{
				return null;
			}

			if (Path.EndsWith(".image"))
			{
				return SerializationUtility.DeserializeValue<Image>(bytes, DataFormat.Binary);
			}

			// fixed conversion?
			MagickImage image = new (bytes);
			return Image.CreateFromMagicImage2D(image, ImagePixelFormat.R8_G8_B8_A8, 1);
		}
		catch (Exception e)
		{
			return null;
		}
	}
}