Shader "Custom/URP HPBar Shader"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _GradientTex("GradientTex", 2D) = "white" {}
        _Progress("Progress", float) = 1.0
    }
    SubShader
    {
        Tags{"RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True" "CanUseSpriteAtlas" = "True"}
        LOD 100

        Pass
        {
            Name "OutLine"
            Tags { "LightMode" = "UniversalForward" }

            Cull Off
            Lighting Off
            ZWrite Off
            Blend One OneMinusSrcAlpha

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
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(UnityPerMaterial)
            float _Progress;
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            TEXTURE2D(_GradientTex); SAMPLER(sampler_GradientTex); float4 _GradientTex_TexelSize;
            Varyings vert(vertexdata v)
            {
                Varyings o;

                UNITY_SETUP_INSTANCE_ID(v);
                float3 posWS = TransformObjectToWorld(v.vertex.xyz);
                o.positionCS = TransformWorldToHClip(posWS);
                o.uv = v.uv;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            { 
                UNITY_SETUP_INSTANCE_ID(i);
                
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                if (_Progress >= i.uv.x && color.r > 0.4f)
                {
                    color = SAMPLE_TEXTURE2D(_GradientTex, sampler_GradientTex, float2(_Progress, 0.5f));
                }
                return color;
            }

            ENDHLSL
        }
    }
    //FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
