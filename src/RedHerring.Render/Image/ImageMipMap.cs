namespace RedHerring.Render;

[Serializable]
public sealed class ImageMipMap
{
	// layer indexes in case of cube map
	public const int PositiveXArrayLayer = 0;
	public const int NegativeXArrayLayer = 1;
	public const int PositiveYArrayLayer = 2;
	public const int NegativeYArrayLayer = 3;
	public const int PositiveZArrayLayer = 4;
	public const int NegativeZArrayLayer = 5;
	
	public int          Width;
	public int          Height;
	public int          Depth;
	public ImageLayer[] Layers; // 1 for standard texture, 6 for cube maps
}