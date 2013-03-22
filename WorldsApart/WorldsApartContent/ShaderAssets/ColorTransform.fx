float4 DestColor;

texture2D ColorMap;
sampler2D ColorMapSampler = sampler_state
{
    Texture = <ColorMap>;
	AddressU = clamp;
	AddressV = clamp;
};

struct PixelShaderInput
{
    float2 TexCoord : TEXCOORD0;
};

float4 PixelShaderFunction(PixelShaderInput input) : COLOR0
{
    float4 srcRGBA = tex2D(ColorMapSampler, input.TexCoord);

    float fmax = max(srcRGBA.r, max(srcRGBA.g, srcRGBA.b));
    float fmin = min(srcRGBA.r, min(srcRGBA.g, srcRGBA.b));
    float delta = fmax - fmin;

    float4 originalDestColor = float4(1, 0, 0, 1);
    float4 deltaDestColor = originalDestColor - DestColor;

    //float4 finalRGBA = srcRGBA - (deltaDestColor * delta);
	float4 finalRGBA = srcRGBA;
	finalRGBA.rgb = DestColor.rgb;

    return finalRGBA;
}

technique Technique1
{
    pass ColorTransform
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
