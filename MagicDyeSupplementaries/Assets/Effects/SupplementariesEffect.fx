//build this using Dye Lab https://forums.terraria.org/index.php?threads/dyelab-a-shader-development-tool-for-terraria-tmodloader.132369/
sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor : register(C0);
float3 uSecondaryColor : register(C1);
float uOpacity : register(C2);
float2 uTargetPosition : register(C3);
float uSaturation : register(C4);
float uRotation : register(C5);
float uTime : register(C6);
float4 uSourceRect : register(C7);
float2 uWorldPosition : register(C8);
float uDirection : register(C9);
float3 uLightSource : register(C10);
float2 uImageSize0 : register(C11);
float2 uImageSize1 : register(C12);
float4 uLegacyArmorSourceRect : register(C13);
float2 uLegacyArmorSheetSize : register(C14);
float4 uShaderSpecificData : register(C15);

texture noise;
sampler noise_sampler = sampler_state{Texture = noise;};

float4 Base(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    return tex2D(uImage0, coords);
}

float4 ArmorSepiaTone(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0// source https://alastaira.wordpress.com/2013/12/02/sepia-shader/
{
    float4 color = tex2D(uImage0, coords);
    float4 color1 = (0, 0, 0, 0);
    color1.x = (color.x * .393) + (color.y *.769) + (color.z * .189);
    color1.y = (color.x * .349) + (color.y *.686) + (color.z * .168);
    color1.z = (color.x * .272) + (color.y *.534) + (color.z * .131);
    color1.w = color.w;
    color1 *= sampleColor;
    return color1;
}
float4 ArmorGrayScale(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0 //source https://digitalerr0r.wordpress.com/2009/04/22/xna-shader-programming-tutorial-11-post-process-grayscale/
{
    float4 color = tex2D(uImage0, coords);
    color.rgb = dot(color.rgb, float3(0.3, 0.59, 0.11));
    color *= sampleColor;
    return color;
}
float4 ArmorPosterize(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0//source https://www.codeproject.com/Articles/495631/Video-Effects-in-Direct3D-Part-1
{   
    float4 color = tex2D(uImage0, coords);
    float3 tc = color.xyz;
    tc = pow(tc, uColor.y);
    tc = tc * uColor.x;
    tc = floor(tc);
    tc = tc / uColor.x;
    tc = pow(tc,1.0 / uColor.y);
    tc *= sampleColor;
    return float4(tc,color.w);
}
float4 ArmorDetermination(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0 //source http://blogs.microsoft.co.il/tamir/2008/06/17/hlsl-pixel-shader-effects-tutorial/
{
    float4 Color = tex2D(uImage0, coords);
    float4 color1 = Color;
    Color.rgb = (Color.r+Color.g+Color.b)/3.0f;
    if (Color.a != 0)
    {
        if (Color.r<uSaturation || Color.r>0.9) Color.r = uSecondaryColor.r; else Color.r = uColor.r;
        if (Color.g<uSaturation || Color.g>0.9) Color.g = uSecondaryColor.g; else Color.g = uColor.g;
        if (Color.b<uSaturation || Color.b>0.9) Color.b = uSecondaryColor.b; else Color.b = uColor.b;
    }
    Color *= sampleColor;
    return Color;
}

technique Default
{
    pass Base
    {
        PixelShader = compile ps_2_0 Base();
    }
    
    pass ArmorSepiaTone //12
    {
        PixelShader = compile ps_2_0 ArmorSepiaTone();
    }
    pass ArmorGrayScale // 13
    {
        PixelShader = compile ps_2_0 ArmorGrayScale();
    }
    pass ArmorPosterize //14
    {
        PixelShader = compile ps_2_0 ArmorPosterize();
    }
    pass ArmorDetermination //15
    {
        PixelShader = compile ps_2_0 ArmorDetermination();
    }
}