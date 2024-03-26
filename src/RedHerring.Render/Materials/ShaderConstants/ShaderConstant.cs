namespace RedHerring.Render.Materials;

public sealed class ShaderConstant
{
	public readonly string                 Name;
	public readonly ShaderConstantSemantic Semantic;
	public readonly ShaderConstantType     Type;

	public int SizeInBytes => Type.SizeInBytes();

	public ShaderConstant(string name, ShaderConstantSemantic semantic, ShaderConstantType type)
	{
		Name     = name;
		Semantic = semantic;
		Type     = type;
	}
}