layout(set = 0, binding = 0) cbuffer VSHConstants
{
	float4x4 WVPMatrix;
	float4x4 WMatrix;

//	float4 WVPMatrixCol0;
//	float4 WVPMatrixCol1;
//	float4 WVPMatrixCol2;
//	float4 WVPMatrixCol3;
//
//	float4 WMatrixCol0;
//	float4 WMatrixCol1;
//	float4 WMatrixCol2;
//	float4 WMatrixCol3;
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
	output.Position = float4(input.Position,1) * WVPMatrix;
	output.Normal = float4(input.Normal,0) * WMatrix;

//	float4 inPosition = float4(input.Position,1);
//	output.Position.x = dot(inPosition, WVPMatrixCol0);
//	output.Position.y = dot(inPosition, WVPMatrixCol1);
//	output.Position.z = dot(inPosition, WVPMatrixCol2);
//	output.Position.w = dot(inPosition, WVPMatrixCol3);
//
//	output.Normal.x = dot(input.Normal.xyz, WMatrixCol0.xyz);
//	output.Normal.y = dot(input.Normal.xyz, WMatrixCol1.xyz);
//	output.Normal.z = dot(input.Normal.xyz, WMatrixCol2.xyz);
//	output.Normal.w = 0;

	output.UV = input.UV;
	return output;
}
