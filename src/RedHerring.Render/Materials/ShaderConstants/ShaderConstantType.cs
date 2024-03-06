namespace RedHerring.Render.Materials;

public enum ShaderConstantType
{
	Float4,
	Int4,
	Float4x4,
}

public static class ShaderConstantTypeExtensions
{
	public static int SizeInBytes(this ShaderConstantType type)
	{
		return type switch
		{
			ShaderConstantType.Float4 => 4 *sizeof(float),
			ShaderConstantType.Int4 => 4 *sizeof(int),
			ShaderConstantType.Float4x4 => 16 *sizeof(float),
			_ => 0
		};
	}
}