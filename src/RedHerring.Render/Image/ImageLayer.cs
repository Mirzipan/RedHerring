namespace RedHerring.Render;

[Serializable]
public sealed class ImageLayer
{
	/*
		Pixels order:
			- foreach depth unit
				- foreach height unit
					- foreach width unit
						- pixel
	*/
	public byte[] Pixels;
}