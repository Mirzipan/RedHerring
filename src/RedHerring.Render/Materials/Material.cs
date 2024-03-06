using System.Numerics;

namespace RedHerring.Render.Materials;

public abstract class Material
{
	public readonly Pass[] Passes;

	public Material(params Pass[] passes)
	{
		Passes = passes;
	}

	public void SetShaderConstant(string name, Vector4 value)
	{
		// TODO
	}
}