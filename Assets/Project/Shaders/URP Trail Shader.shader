Shader "Custom/URP Trail Shader"
{
    Properties
    {
        _EffectTex("EffectTex", 2D) = "white" {}
        _BlurVector("BlurVector", Vector) = (0,0,0,0)
        _BlurPower("BlurPower", float) = 0.0
        _SampleCount("SampleCount", Range(1,6)) = 1
    }
    SubShader
    {
        Tags{ "RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Transparent" "IgnoreProjector" = "True"}
        LOD 100

        Pass
        {
            Name "TrailRender"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite On
            ZTest On

            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" // URP 코어 코드

            struct vertexdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 pixelPos : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(UnityPerMaterial)
            Vector _BlurVector;
            float _BlurPower;
            float _SampleCount;
            CBUFFER_END

            TEXTURE2D(_EffectTex); SAMPLER(sampler_EffectTex); float4 _EffectTex_TexelSize;
            TEXTURE2D(_CameraOpaqueTexture); SAMPLER(sampler_CameraOpaqueTexture); float4 _CameraOpaqueTexture_TexelSize;

            Varyings vert(vertexdata v)
            {
                Varyings o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
                o.positionCS = TransformWorldToHClip(positionWS);
                o.uv = float2(v.uv.y, v.uv.x);
                float4 screenPos = ComputeScreenPos(o.positionCS);
                o.pixelPos = screenPos.xy / screenPos.w;
                return o;
            }

            half3 Sampling(float2 _uv)
            {
                half3 color;
                half x = (-_BlurVector.x) * _BlurPower;
                half y = (-_BlurVector.y) * _BlurPower;
                int fullsamplecount = 0;
                for (int i = 0; i < _SampleCount; ++i)
                {
                    color += SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, float2(_uv.x + (_BlurPower * i) + x, _uv.y)).rgb;
                    color += SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, float2(_uv.x, _uv.y + (_BlurPower * i) + y)).rgb;
                    color += SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, float2(_uv.x - (_BlurPower * i) - x, _uv.y)).rgb;
                    color += SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, float2(_uv.x, _uv.y - (_BlurPower * i) - y)).rgb;
                    fullsamplecount += 4;
                }
                color /= fullsamplecount;
                return color;
            }

            half4 frag(Varyings i) : SV_Target
            { 
                UNITY_SETUP_INSTANCE_ID(i);
                
                half4 finalcolor = SAMPLE_TEXTURE2D(_EffectTex, sampler_EffectTex, i.uv);
                finalcolor.rgb *= Sampling(i.pixelPos) * 5;
                finalcolor.a = saturate(finalcolor.r * 3);
                return finalcolor;
            }

            ENDHLSL
        }
    }
    //FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
