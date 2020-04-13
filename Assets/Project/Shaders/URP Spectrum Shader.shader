Shader "Custom/URP Spectrum Shader"
{
    Properties
    {
        _EffectTex("EffectTex", 2D) = "white" {}
        [HDR] _RimColor("RimColor", Color) = (1,1,1,1)
        _RimPow("RimPow", float) = 0.0
    }
    SubShader
    {
        Tags{ "RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Transparent" "IgnoreProjector" = "True"}
        LOD 100

        Pass
        {
            Name "OutLine"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite On
            ZTest On

            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" // URP 코어 코드
            //#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"

            struct vertexdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 normalWS : NORMAL;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(UnityPerMaterial)
            float4 _RimColor;
            float _RimPow;
            CBUFFER_END

            TEXTURE2D(_EffectTex); SAMPLER(sampler_EffectTex); float4 _EffectTex_TexelSize;

            Varyings vert(vertexdata v)
            {
                Varyings o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.positionWS = TransformObjectToWorld(v.vertex.xyz);
                o.positionCS = TransformWorldToHClip(o.positionWS);
                o.normalWS = TransformObjectToWorldNormal(v.normal);
                o.uv = v.uv;

                return o;
            }

            half4 frag(Varyings i) : SV_Target
            { 
                UNITY_SETUP_INSTANCE_ID(i);
                
                float4 effectcolor = SAMPLE_TEXTURE2D(_EffectTex, sampler_EffectTex, i.uv);

                float3 InversViewDir = SafeNormalize(i.positionWS - GetCameraPositionWS());

                float4 finalcolor = lerp(_RimColor * effectcolor, float4(0,0,0,0),  pow(abs(dot(i.normalWS, InversViewDir)), _RimPow));

                return finalcolor;
            }

            ENDHLSL
        }
    }
    //FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
