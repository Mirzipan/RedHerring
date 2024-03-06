using RedHerring.Assets;
using Veldrid;

namespace RedHerring.Render.Materials;

public sealed class Pass
{
	public readonly string                       Name;
	public readonly int                          Order;
	public readonly BlendStateDescription        BlendStateDescription;
	public readonly DepthStencilStateDescription DepthStencilStateDescription;
	public readonly RasterizerStateDescription   RasterizerStateDescription;
	public readonly AssetReference               VertexShader;
	public readonly AssetReference               PixelShader;
	public readonly ShaderResourceSet[]          ShaderResourceSets;

	public Pass(
		string                       name,
		int                          order,
		BlendStateDescription        blendStateDescription,
		DepthStencilStateDescription depthStencilStateDescription,
		RasterizerStateDescription   rasterizerStateDescription,
		AssetReference               vertexShader,
		AssetReference               pixelShader,
		ShaderResourceSet[]          shaderResourceSets
	)
	{
		Name                         = name;
		Order                        = order;
		BlendStateDescription        = blendStateDescription;
		DepthStencilStateDescription = depthStencilStateDescription;
		RasterizerStateDescription   = rasterizerStateDescription;
		VertexShader                 = vertexShader;
		PixelShader                  = pixelShader;
		ShaderResourceSets           = shaderResourceSets;
	}
}