Shader "Custom/BloodyDoors_Full"
{
    Properties
    {
        [MainColor] _BaseColor("Color", Color) = (1,1,1,1)
        [MainTexture] _BaseMap("Albedo", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _MetallicMap("Metallic (R) Smoothness (A)", 2D) = "black" {}
        _OcclusionMap("Occlusion", 2D) = "white" {}
        
        _BloodColor("Blood Color", Color) = (0.5,0,0,1)
        _BloodMap("Blood Texture", 2D) = "black" {}
        _BloodNormal("Blood Normal", 2D) = "bump" {}
        _BloodMetallic("Blood Metallic (R) Smoothness (A)", 2D) = "black" {}
        _BloodAmount("Blood Amount", Range(0, 1)) = 0
        _BloodDissolve("Blood Dissolve", 2D) = "white" {}
        _BloodFlowSpeed("Blood Flow Speed", Float) = 0.1
        _BloodFlowBump("Blood Flow Bump", 2D) = "bump" {}

        [Toggle(_NORMALMAP)] _UseNormalMap("Use Normal Map", Float) = 1
        [Toggle(_METALLIC)] _UseMetallic("Use Metallic", Float) = 1
        _Glossiness("Smoothness", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        ENDHLSL

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma shader_feature _NORMALMAP
            #pragma shader_feature _METALLIC
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _SHADOWS_SOFT

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float4 tangentWS : TEXCOORD3;
                float3 viewDirWS : TEXCOORD4;
                float4 shadowCoord : TEXCOORD5;
            };

            TEXTURE2D(_BaseMap);
            TEXTURE2D(_NormalMap);
            TEXTURE2D(_MetallicMap);
            TEXTURE2D(_OcclusionMap);
            TEXTURE2D(_BloodMap);
            TEXTURE2D(_BloodNormal);
            TEXTURE2D(_BloodMetallic);
            TEXTURE2D(_BloodDissolve);
            TEXTURE2D(_BloodFlowBump);

            SAMPLER(sampler_BaseMap);
            SAMPLER(sampler_NormalMap);
            SAMPLER(sampler_MetallicMap);
            SAMPLER(sampler_OcclusionMap);
            SAMPLER(sampler_BloodMap);
            SAMPLER(sampler_BloodNormal);
            SAMPLER(sampler_BloodMetallic);
            SAMPLER(sampler_BloodDissolve);
            SAMPLER(sampler_BloodFlowBump);

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _NormalMap_ST;
            float4 _MetallicMap_ST;
            float4 _OcclusionMap_ST;
            float4 _BloodMap_ST;
            float4 _BloodNormal_ST;
            float4 _BloodMetallic_ST;
            float4 _BloodDissolve_ST;
            float4 _BloodFlowBump_ST;
            float4 _BaseColor;
            float4 _BloodColor;
            float _BloodAmount;
            float _BloodFlowSpeed;
            float _Glossiness;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                output.tangentWS = float4(normalInput.tangentWS, input.tangentOS.w);
                output.viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.shadowCoord = GetShadowCoord(vertexInput);
                
                return output;
            }

           half4 frag(Varyings input) : SV_Target
{
    // 1. Проверка базовой текстуры
    half4 baseTex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
    if (length(baseTex.rgb) < 0.01) {
        return half4(1,0,1,1); // Фиолетовый - текстура не загружена
    }

    // 2. Основной цвет
    half4 baseColor = baseTex * _BaseColor;
    
    // 3. Кровь (только если текстура назначена)
    half4 bloodColor = _BloodColor; // Базовый цвет крови
    if (_BloodAmount > 0.01) {
        half4 bloodTex = SAMPLE_TEXTURE2D(_BloodMap, sampler_BloodMap, input.uv);
        bloodColor *= bloodTex;
        half bloodDissolve = SAMPLE_TEXTURE2D(_BloodDissolve, sampler_BloodDissolve, input.uv).r;
        half bloodBlend = saturate((_BloodAmount * 2.0 - 1.0) + bloodDissolve);
        baseColor.rgb = lerp(baseColor.rgb, bloodColor.rgb, bloodBlend);
    }

    // 4. Возврат результата
    return half4(baseColor.rgb, 1.0);
}
            ENDHLSL
        }
        
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            
            ZWrite On
            ZTest LEqual
            ColorMask 0
            
            HLSLPROGRAM
            #pragma vertex vertShadow
            #pragma fragment fragShadow
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            
            float3 _LightDirection;
            
            struct ShadowAttributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };
            
            float4 vertShadow(ShadowAttributes input) : SV_POSITION
            {
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));
                return positionCS;
            }
            
            half4 fragShadow() : SV_TARGET
            {
                return 0;
            }
            ENDHLSL
        }
    }
    
    FallBack "Universal Render Pipeline/Lit"
    CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.LitShaderGUI"
}