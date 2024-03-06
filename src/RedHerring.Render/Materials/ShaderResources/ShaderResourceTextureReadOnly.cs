using RedHerring.Assets;
using Veldrid;

namespace RedHerring.Render.Materials;

public sealed class ShaderResourceTextureReadOnly : ShaderResource
{
	public readonly AssetReference Reference;
	
	public ShaderResourceTextureReadOnly(string name, ShaderStages stages, AssetReference reference) : base(name, ResourceKind.TextureReadOnly, stages)
	{
		Reference = reference;
	}
}