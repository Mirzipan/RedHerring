using Veldrid;

namespace RedHerring.Render.Materials;

public abstract class ShaderResource
{
	public readonly string       Name;
	public readonly ResourceKind Kind;
	public readonly ShaderStages Stages;

	protected ShaderResource(string name, ResourceKind kind, ShaderStages stages)
	{
		Name   = name;
		Kind   = kind;
		Stages = stages;
	}
}