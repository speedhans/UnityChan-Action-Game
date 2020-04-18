Shader "Custom/URP Toon NSM Shader"
{
    Properties
    {
        _BaseColor("BaseColor", Color) = (0.5,0.5,0.5,1)
        _BaseMap("BaseMap", 2D) = "white" {}
        _RampTex("RampTex", 2D) = "white" {}
        _RampOffset("RampOffset", Range(-1.0, 1.0)) = 0.0
        _ReciveShadowOffset("ReciveShadowOffset", Range(0.0, 1.0)) = 0.0
        _SpecMap("Specular", 2D) = "white" {}
        _SpecPow("SpecPow", float) = 0.0
        _RimColor("RimColor", Color) = (1,1,1,1)
        _RimPow("RimPow", float) = 0.0
        _NormalMap("NormalMap", 2D) = "white" {}
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        // Blending state
        [HideInInspector] _Surface("__surface", Float) = 0.0
        [HideInInspector] _Blend("__blend", Float) = 0.0
        [HideInInspector] _AlphaClip("__clip", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
        _Cull("__cull", Float) = 2.0

        _ReceiveShadows("Receive Shadows", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
        LOD 100

        Pass
        {
            Name "Universal Forward"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite[_ZWrite]
            Cull[_Cull]

            HLSLPROGRAM

            #pragma shader_feature _NORMALMAP
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

            struct vertexdata
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
#if _NORMALMAP
                half3 tangentWS                 : TEXCOORD4;
                half3 bitangentWS               : TEXCOORD5;
#endif
        #ifdef _MAIN_LIGHT_SHADOWS
                float4 shadowCoord              : TEXCOORD6; // compute shadow coord per-vertex for the main light
        #endif
                float4 positionCS               : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(ToonMaterial)
            float _RampOffset;
            float _SpecPow;
            float _ReciveShadowOffset;
            float4 _RimColor;
            float _RimPow;
            CBUFFER_END
            TEXTURE2D(_RampTex); SAMPLER(sampler_RampTex); float4 _RampTex_TexelSize;
            TEXTURE2D(_NormalMap); SAMPLER(sampler_NormalMap); float4 _NormalMap_TexelSize;
            TEXTURE2D(_SpecMap); SAMPLER(sampler_SpecMap); float4 _SpecMap_TexelSize;

            Varyings vert (vertexdata v)
            {
                Varyings o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz); // vertex 관련 데이터 계산 및 구조체로 만들기
                VertexNormalInputs vertexNormalInput = GetVertexNormalInputs(v.normal, v.tangent); // normal 관련 데이터 계산 및 구조체로 만들기

                float fogFactor = ComputeFogFactor(vertexInput.positionCS.z); // Fog factor

                o.positionWSAndFogFactor = float4(vertexInput.positionWS, fogFactor);
                o.positionCS = vertexInput.positionCS; // 오브젝트 포지션
                o.normalWS = vertexNormalInput.normalWS; // 월드 노멀
                o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
                o.uvLM = v.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw; // 뭔지 모르겠다 ?

#ifdef _NORMALMAP
                o.tangentWS = vertexNormalInput.tangentWS;
                o.bitangentWS = vertexNormalInput.bitangentWS;
#endif

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

            inline half3 CalSpecular(half3 attenColor, half3 diffSamplerColor, half2 specUV, half3 normal, half3 viewDir, half3 lightDir, half3 lightColor)
            {
                float NdotL = dot(normal, lightDir);
                //float3 reflectionVector = normalize((2.0 * normal * NdotL) - lightDir);

                //float spec = smoothstep(0.005f, 0.1f, pow(abs(dot(reflectionVector, viewDir)), _SpecPow));
                //float3 finalSpec = SAMPLE_TEXTURE2D(_SpecMap, sampler_SpecMap, spec);

                //half3 c = baseColor + lightColor * finalSpec;

                float NdotE = dot(normal, viewDir);
                half4 reflectionMaskColor = SAMPLE_TEXTURE2D(_SpecMap, sampler_SpecMap, specUV);
                half4 lighting = lit(NdotE, NdotL, _SpecPow);
                half3 specularColor = saturate(lighting.z) * reflectionMaskColor.rgb * diffSamplerColor;
                attenColor += specularColor;

                return attenColor;
            }

            half4 frag (Varyings i) : SV_Target
            {

#if _NORMALMAP
                half3 normalTS = SampleNormal(i.uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), 1.0f);
                half3 normalWS = TransformTangentToWorld(normalTS,
                    half3x3(i.tangentWS, i.bitangentWS, i.normalWS));
#else

                half3 normalWS = i.normalWS;
#endif

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

                if (additionalLightsCount > 0)
                {
                    half3 additionalcolor;
                    half AddSA;
                    for (int j = 0; j < additionalLightsCount; ++j)
                    {
                        // GetMainLight와 비슷하지만 for-loop 인덱스가 필요합니다.
                        // 이것은 오브젝트 별 라이트 인덱스를 알아 내고 라이트 구조체를 초기화하기 위해 라이트 버퍼를 샘플링합니다.
                        // _ADDITIONAL_LIGHT_SHADOWS가 정의 된 경우 그림자도 계산합니다.

                        Light light = GetAdditionalLight(j, positionWS);
                        // Same functions used to shade the main light.
                        additionalcolor += light.color * dot(normalWS, light.direction) * light.shadowAttenuation * light.distanceAttenuation;
                        AddSA += light.shadowAttenuation * light.distanceAttenuation;
                    }
                    AddSA /= additionalLightsCount;
                    additionalcolor /= additionalLightsCount;

                    SA += AddSA;
                    basecolor += additionalcolor;
                }
#endif
                half3 viewDirectionWS = SafeNormalize(GetCameraPositionWS() - positionWS); // 카메라 뷰 가져오기

                //color += surfaceData.emission;
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);
                color *= half4(basecolor, 1.0f);
                float2 rampUV = float2(saturate(dot(normalWS, mainLight.direction) + _RampOffset), 0.5f);
                rampUV.x = clamp(rampUV.x - (1.0f - saturate(SA + _ReciveShadowOffset)), 0.01f, 0.99f);
                half4 rcol = SAMPLE_TEXTURE2D(_RampTex, sampler_RampTex, rampUV);

                half3 calcolor = CalSpecular(color.rgb, rcol.rgb * basecolor, i.uv, normalWS, viewDirectionWS, mainLight.direction, mainLight.color).rgb;
                float fogFactor = i.positionWSAndFogFactor.w;

                float3 finalcolor = lerp(_RimColor.rgb, calcolor, pow(abs(dot(normalWS, viewDirectionWS)), _RimPow));

                finalcolor = MixFog(finalcolor, fogFactor);

                return half4(finalcolor, _BaseColor.a);
            }
            ENDHLSL
        }

        UsePass "Universal Render Pipeline/Lit/ShadowCaster" // 쉐도우 캐스트 페스
        UsePass "Universal Render Pipeline/Lit/DepthOnly" // 뎁스 패스
        UsePass "Universal Render Pipeline/Lit/Meta" // 메타 패스
        //UsePass "Custom/URP OutLine Shader"


    }
    //FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
