layout(set = 1, binding = 0) cbuffer PSHConstants
{
	float4 ObjectColor;

	float4 LightDirection;
	float4 LightColor;
	float4 AmbientColor;
};

struct VStoPS // TODO - share this between shaders
{
	layout(location=0) float4 Position : SV_POSITION;
	layout(location=1) float4 Normal : NORMAL0;
	layout(location=2) float2 UV : TEXCOORD0;
};

half4 main(VStoPS input) : SV_TARGET
{
	return ObjectColor * (AmbientColor + saturate(dot(-LightDirection, normalize(input.Normal))) * LightColor);
}

