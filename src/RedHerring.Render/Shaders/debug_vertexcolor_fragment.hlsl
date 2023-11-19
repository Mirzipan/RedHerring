struct PixelInput
{
    float4 position : POSITION;
    float4 color : COLOR;
};

float4 PS(PixelInput input) : SV_TARGET
{
    return input.color;
}

