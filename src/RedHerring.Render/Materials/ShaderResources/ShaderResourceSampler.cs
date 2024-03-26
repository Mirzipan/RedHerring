using Veldrid;

namespace RedHerring.Render.Materials;

public sealed class ShaderResourceSampler : ShaderResource
{
	public readonly SamplerDescription SamplerDescription;
	
	public ShaderResourceSampler(string name, ShaderStages stages, SamplerDescription samplerDescription) : base(name, ResourceKind.Sampler, stages)
	{
		SamplerDescription = samplerDescription;
	}
}