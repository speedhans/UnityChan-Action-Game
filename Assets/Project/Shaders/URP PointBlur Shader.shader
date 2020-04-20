Shader "Custom/URP PointBlur Shader"
{
    Properties
    {
        [MainTexture] _MainTex("MainTex", 2D) = "white" {}
        _BlurPointX("BlurPointX", float) = 0.0
        _BlurPointY("BlurPointY", float) = 0.0
        _BlurPower("BlurPower", float) = 0.0
        _SampleCount("SampleCount", float) = 0.0
    }
        SubShader
        {
            Tags{ "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True" }

            Pass
            {
                Name "CustomColorGrading"
                Tags { "LightMode" = "UniversalForward" }

                Cull off
                Zwrite off

                HLSLPROGRAM

                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" // URP 코어 코드

                struct vertexdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct Varyings
                {
                    float4 position : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
                CBUFFER_START(UnityPerMaterial)
                float _BlurPointX;
                float _BlurPointY;
                float _BlurPower;
                float _SampleCount;
                CBUFFER_END

                Varyings vert(vertexdata v)
                {
                    Varyings o;
                    float3 posWS = TransformObjectToWorld(v.vertex.xyz);
                    o.position = TransformWorldToHClip(posWS);
                    o.uv = v.uv;
                    return o;
                }

                inline float4 CalculateBlur(float2 _uv)
                {
                    float2 dir = _uv - float2(_BlurPointX, _BlurPointY);
                    float leng = smoothstep(0.35f, 0.0f, length(dir));

                    float4 center = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _uv);

                    if (_SampleCount > 0)
                    {
                        float4 Add;
                        for (int i = 0; i < _SampleCount; ++i)
                        {
                            float offset = _BlurPower * (_SampleCount * i + 1);
                            float2 adduv = (normalize(dir) * leng * (offset)) + (sin(_Time.w) + cos(_Time.z)) * leng * 0.01f;
                            Add += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _uv + adduv);
                        }
                        Add /= _SampleCount;
                        center = center * 0.7f + Add * 0.3f;
                    }
                    return center;
                }

                float4 frag(Varyings v) : SV_Target
                {
                    return CalculateBlur(v.uv);
                }

                ENDHLSL
            }
        }
}
