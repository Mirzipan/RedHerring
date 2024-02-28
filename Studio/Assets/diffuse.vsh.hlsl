layout(set = 0, binding = 0) cbuffer VSHConstants
{
	float4x4 WVPMatrix;
	float4x4 WMatrix;
};

struct VSInput
{
	layout(location=0) float3 Position : POSITION0;
	layout(location=1) float3 Normal : NORMAL0;
	layout(location=2) float2 UV : TEXCOORD0;
};

struct VStoPS // TODO - share this between shaders
{
	layout(location=0) float4 Position : SV_POSITION;
	layout(location=1) float4 Normal : NORMAL0;
	layout(location=2) float2 UV : TEXCOORD0;
};

VStoPS main(VSInput input)
{
	VStoPS output;
	output.Position = mul(float4(input.Position,1), WVPMatrix);
	output.Normal=mul(float4(input.Normal,0), WMatrix);
	output.UV = input.UV;
	return output;
}
