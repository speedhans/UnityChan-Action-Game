﻿Shader "Custom/URP Toon Transparent Shader"
{
    Properties
    {
        _BaseColor("BaseColor", Color) = (0.5,0.5,0.5,1)
        _BaseMap("BaseMap", 2D) = "white" {}
        _RampTex("RampTex", 2D) = "white" {}
        _RampOffset("RampOffset", Range(-1.0, 1.0)) = 0.0
        _ReciveShadowOffset("ReciveShadowOffset", Range(0.0, 1.0)) = 0.0
        _SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
        _SpecPow("SpecPow", float) = 0.0

        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        // Blending state
        [HideInInspector] _Surface("__surface", Float) = 0.0
        [HideInInspector] _Blend("__blend", Float) = 0.0
        [HideInInspector] _AlphaClip("__clip", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
        [HideInInspector] _Cull("__cull", Float) = 2.0

        _ReceiveShadows("Receive Shadows", Float) = 1.0
    }
    SubShader
    {
        Tags{"RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline" "IgnoreProjector" = "True"}
        LOD 100

        Pass
        {
            Name "Universal Forward"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha, One One

            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM

            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS // 추가 광원을 사용할 것인가?
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS // 추가 광원의 그림자 지원 여부
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

            // -------------------------------------

            // Unity defined keywords

            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_instancing

            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            //#include "UnityCG.cginc"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" // URP 코어 코드
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl" // URP 빛 관련 코드
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl" // SurfaceData 등 UPR의 기본 데이터를 받아올때 사용되는 듯?

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 uv : TEXCOORD0;
                float2 uvLM : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };


            struct Varyings
            {
                float2 uv                       : TEXCOORD0;
                float2 uvLM                     : TEXCOORD1;
                float4 positionWSAndFogFactor   : TEXCOORD2; // xyz: positionWS, w: vertex fog factor
                half3  normalWS                 : TEXCOORD3;
                float cameraDistance : TEXCOORD4;
        #ifdef _MAIN_LIGHT_SHADOWS
                float4 shadowCoord              : TEXCOORD6; // compute shadow coord per-vertex for the main light
        #endif
                float4 positionCS               : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(ToonBuffer)
            float _RampOffset;
            float _SpecPow;
            float _ReciveShadowOffset;
            TEXTURE2D(_RampTex); SAMPLER(sampler_RampTex); float4 _RampTex_TexelSize;
            CBUFFER_END

            Varyings vert (appdata v)
            {
                Varyings o;

                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz); // vertex 관련 데이터 계산 및 구조체로 만들기
                VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(v.normal, v.tangent); // normal 관련 데이터 계산 및 구조체로 만들기

                float fogFactor = ComputeFogFactor(vertexInput.positionCS.z); // Fog factor

                o.positionWSAndFogFactor = float4(vertexInput.positionWS, fogFactor);
                o.positionCS = vertexInput.positionCS; // 오브젝트 포지션
                o.normalWS = vertexNormalInput.normalWS; // 월드 노멀
                o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
                o.uvLM = v.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw; // 뭔지 모르겠다 ?

#ifdef _MAIN_LIGHT_SHADOWS
                // 메인 라이트의 그림자 좌표는 정점으로 계산됩니다.
                // 캐스케이드가 활성화 된 경우 LWRP는 화면 공간의 그림자를
                // 해결하고 이 좌표는 화면 공간 그림자 질감의 UV 좌표가됩니다.
                // 그렇지 않으면 LWRP는 조명 공간에서 그림자를 해결합니다 (Depth Pre-Pass 및 Shadow Correction Pass(그림자 수정 패스) 없음).
                // 이 경우 shadowCoord는 조명 공간에 위치됩니다.
                o.shadowCoord = GetShadowCoord(vertexInput);
#endif

                return o;
            }

            inline half3 CalSpecular(half3 baseColor, half3 normal, half3 viewDir, half3 lightDir, half3 lightColor)
            {
                float NdotL = dot(normal, lightDir);
                float3 reflectionVector = normalize((2.0 * normal * NdotL) - lightDir);

                float spec = smoothstep(0.005f, 0.1f, pow(dot(reflectionVector, viewDir), _SpecPow));
                float3 finalSpec = _SpecColor.rgb * spec;

                half3 c = baseColor + lightColor * finalSpec;
                return c;
            }

            half4 frag (Varyings i) : SV_Target
            {

                half3 normalWS = i.normalWS;
                normalWS = normalize(normalWS);

                half3 bakedGI = SampleSH(normalWS);

                float3 positionWS = i.positionWSAndFogFactor.xyz;

#ifdef _MAIN_LIGHT_SHADOWS // 메인 라이트 계산
                // 메인 라이트는 가장 밝은 지향성 라이트입니다.
                // 이것은 빛 루프 외부에서 음영 처리 된 특정 변수 세트와 음영 경로를 제공하므로,
                // 지향성 라이트가 1 개일때 가장 빠릅니다
                // 옵션에서 shadowCoord(정점마다 계산)을 전달할 수 있습니다.이 경우 shadowAttenuation이 계산됩니다.
                Light mainLight = GetMainLight(i.shadowCoord);
#else
                Light mainLight = GetMainLight();
#endif
                half3 basecolor = _BaseColor.rgb;
                half SA = mainLight.shadowAttenuation * mainLight.distanceAttenuation;

#ifdef _ADDITIONAL_LIGHTS // 추가 광원 계산
                int additionalLightsCount = GetAdditionalLightsCount();

                for (int j = 0; j < additionalLightsCount; ++j)
                {
                    // GetMainLight와 비슷하지만 for-loop 인덱스가 필요합니다.
                    // 이것은 오브젝트 별 라이트 인덱스를 알아 내고 라이트 구조체를 초기화하기 위해 라이트 버퍼를 샘플링합니다.
                    // _ADDITIONAL_LIGHT_SHADOWS가 정의 된 경우 그림자도 계산합니다.

                    Light light = GetAdditionalLight(j, positionWS);
                    // Same functions used to shade the main light.
                    basecolor += light.color * dot(normalWS, light.direction) * light.shadowAttenuation;
                    SA += light.shadowAttenuation * light.distanceAttenuation;
                }
                SA /= (half)(additionalLightsCount + 1);
#endif
                half3 viewDirectionWS = SafeNormalize(GetCameraPositionWS() - positionWS); // 카메라 뷰 가져오기

                //color += surfaceData.emission;
                float fogFactor = i.positionWSAndFogFactor.w;
                basecolor = MixFog(basecolor, fogFactor);
                basecolor = CalSpecular(basecolor, normalWS, viewDirectionWS, mainLight.direction, mainLight.color).rgb;
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);
                color *= half4(basecolor, 1.0f);
                float2 rampUV = float2(saturate(dot(normalWS, mainLight.direction) + _RampOffset), 0.5f);
                rampUV.x = clamp(rampUV.x - (1.0f - saturate(SA + _ReciveShadowOffset)), 0.0f, 0.99f);
                half4 rcol = SAMPLE_TEXTURE2D(_RampTex, sampler_RampTex, rampUV);

                return half4(color.rgb * rcol.rgb, _BaseColor.a);
            }
            ENDHLSL
        }

        UsePass "Universal Render Pipeline/Lit/ShadowCaster" // 쉐도우 캐스트 페스
        UsePass "Universal Render Pipeline/Lit/DepthOnly" // 뎁스 패스
        UsePass "Universal Render Pipeline/Lit/Meta" // 메타 패스
    }
}
