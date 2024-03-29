using ImageMagick;

namespace RedHerring.Render;

[Serializable]
public sealed class Image
{
	public const int AllMipmaps = -1;
	
	public ImagePixelFormat PixelFormat;
	public ImageMipMap[]    MipMaps; // ordered from biggest to smallest

	
	public static Image CreateFromMagicImage2D(MagickImage magickImage, ImagePixelFormat pixelFormat, int mipMapsCount = AllMipmaps)
	{
		Image image = new () {PixelFormat = pixelFormat};

		if (mipMapsCount == AllMipmaps)
		{
			int maxSize = Math.Max(magickImage.Width, magickImage.Height);
			mipMapsCount = 0;
			while (maxSize > 0)
			{
				maxSize >>= 1;
				++mipMapsCount;
			}
		}

		image.MipMaps = new ImageMipMap[mipMapsCount];

		Func<MagickImage, ImageMipMap> createMipMap = pixelFormat switch
		{
			ImagePixelFormat.R8_G8_B8_A8 => CreateMipMapR8G8B8A8,
			ImagePixelFormat.R8_G8       => CreateMipMapR8G8,
			ImagePixelFormat.R8          => CreateMipMapR8,
			_ => _ => throw new NotImplementedException()
		};
		
		using MagickImage? copy = mipMapsCount > 1 ? new MagickImage(magickImage) : null;
		for (int mipMapIndex = 0; mipMapIndex < mipMapsCount; ++mipMapIndex)
		{
			if (mipMapIndex == 0)
			{
				image.MipMaps[mipMapIndex] = createMipMap(magickImage);
			}
			else
			{
				copy!.FilterType = FilterType.Quadratic;
				copy.Resize(Math.Max(copy.Width/2,1), Math.Max(copy.Height/2,1));
				image.MipMaps[mipMapIndex] = createMipMap(copy);
			}
		}

		return image;
	}

	// mipmap creation functions
	private static ImageMipMap CreateMipMapR8G8B8A8(MagickImage magickImage)
	{
		return CreateMipMapUncompressed(magickImage, 4);
	}

	private static ImageMipMap CreateMipMapR8G8(MagickImage magickImage)
	{
		return CreateMipMapUncompressed(magickImage, 2);
	}

	private static ImageMipMap CreateMipMapR8(MagickImage magickImage)
	{
		return CreateMipMapUncompressed(magickImage, 1);
	}

	private static ImageMipMap CreateMipMapUncompressed(MagickImage magickImage, int colorComponentsCount)
	{
		ImageMipMap mipMap = new() {Width = magickImage.Width, Height = magickImage.Height, Depth = 1};

		mipMap.Layers = new ImageLayer[1];

		ImageLayer layer = new ();
		mipMap.Layers[0] = layer;

		layer.Pixels = new byte[mipMap.Width * mipMap.Height * mipMap.Depth * colorComponentsCount];
		
		using IPixelCollection<byte> pixels = magickImage.GetPixels();
		int count = pixels.Count();
		
		int index = 0;
		for (int z = 0; z < mipMap.Depth; ++z)
		{
			for (int y = 0; y < mipMap.Height; ++y)
			{
				for (int x = 0; x < mipMap.Width; ++x)
				{
					IPixel<byte>       pixel = pixels[x,y]!;
					IMagickColor<byte> color = pixel.ToColor()!;

					layer.Pixels[index++] = color.R;
					if (colorComponentsCount > 1)
					{
						layer.Pixels[index++] = color.G;
						if (colorComponentsCount > 2)
						{
							layer.Pixels[index++] = color.B;
							layer.Pixels[index++] = color.A;
						}
					}
				}
			}
		}

		return mipMap;
	}
}