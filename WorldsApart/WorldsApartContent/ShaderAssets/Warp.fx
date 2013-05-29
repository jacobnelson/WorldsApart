float4 DestColor;

float2 resolution;
float ringWidth;
float ringSize;
float ringMag;

float scale;
float magnitude;

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
    
	
	
	
	if (false)
	{
	float4 color = tex2D(ColorMapSampler, input.TexCoord);
	
	if (ringMag != !.0)
	{
		float ratio = resolution.x / resolution.y;
		float2 p = input.TexCoord.xy - float2(.5, .5);
		p.x *= ratio;
		
		float distance = length(p);
		float radians = (ringSize - distance) * ringWidth;
		float dratio = cos(radians);
		dratio = ringMag + (1.0 - ringMag) * dratio;
		
		p *= dratio;
		p.x /= ratio;
		p += float2(.5, .5);
		
		if (distance < ringSize && radians < 3.14159265)
		{
			color = tex2D(ColorMapSampler, p);
		}
	}
	}

	input.TexCoord += sin( scale * (input.TexCoord) ) * magnitude;

	float4 colour = tex2D(ColorMapSampler, input.TexCoord);
	return colour;

}

technique Technique1
{
    pass ColorTransform
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
