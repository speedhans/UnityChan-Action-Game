Shader "Custom/URP Weapon Shader"
{
    Properties
    {
        [HDR] _LightColor("LightColor", Color) = (1,1,1,1)
        _LightPow("LightPow", float) = 0.0
        _ProgressDir("ProgressDir", float) = 0.0
    }
    SubShader
    {
        Tags{ "RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Transparent" "IgnoreProjector" = "True"}
        LOD 100

        Pass
        {
            Name "Weapon"
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
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float boundary : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(UnityPerMaterial)
            float4 _LightColor;
            float _LightPow;
            float _ProgressDir;
            CBUFFER_END

            Varyings vert(vertexdata v)
            {
                Varyings o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.positionWS = TransformObjectToWorld(v.vertex.xyz);
                o.positionCS = TransformWorldToHClip(o.positionWS);
                o.boundary = (_ProgressDir > 0 ? v.vertex.x : v.vertex.y) > _LightPow ? 1.0f : 0.0f;

                return o;
            }

            half4 frag(Varyings i) : SV_Target
            { 
                UNITY_SETUP_INSTANCE_ID(i);
                
                float4 finalcolor = _LightColor;
                finalcolor.a = i.boundary;

                return finalcolor;
            }

            ENDHLSL
        }
    }
    //FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
