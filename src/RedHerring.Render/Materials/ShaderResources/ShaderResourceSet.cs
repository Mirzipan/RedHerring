namespace RedHerring.Render.Materials;

public sealed class ShaderResourceSet
{
	public readonly ShaderResource[] Resources;

	public ShaderResourceSet(params ShaderResource[] resources)
	{
		Resources = resources;
	}
}