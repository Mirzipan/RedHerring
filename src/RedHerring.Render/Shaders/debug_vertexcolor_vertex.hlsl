cbuffer ProjectionBuffer : register(b0)
{
    float4x4 projection;
}

cbuffer ViewBuffer : register(b1)
{
    float4x4 view;
}

cbuffer WorldBuffer : register(b2)
{
    float4x4 world;
}

struct VertexInput
{
    float3 position : POSITION;
    float4 color : COLOR;
};

struct VertexOutput
{
    float4 position : POSITION;
    float4 color : COLOR;
};

VertexOutput VS(VertexInput input)
{    
    float4 worldPosition = mul(world, float4(input.position, 1));
    float4 viewPosition = mul(view, worldPosition);
    float4 clipPosition = mul(projection, viewPosition);
    
    VertexOutput output;
    output.position = clipPosition;    
    output.color = input.color;
    
    return output;
}

