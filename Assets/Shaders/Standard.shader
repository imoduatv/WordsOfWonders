// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable

Shader "Standard"
{
  Properties
  {
    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Albedo", 2D) = "white" {}
    _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
    _Glossiness ("Smoothness", Range(0, 1)) = 0.5
    _GlossMapScale ("Smoothness Scale", Range(0, 1)) = 1
    [Enum(Metallic Alpha,0,Albedo Alpha,1)] _SmoothnessTextureChannel ("Smoothness texture channel", float) = 0
    [Gamma] _Metallic ("Metallic", Range(0, 1)) = 0
    _MetallicGlossMap ("Metallic", 2D) = "white" {}
    [ToggleOff] _SpecularHighlights ("Specular Highlights", float) = 1
    [ToggleOff] _GlossyReflections ("Glossy Reflections", float) = 1
    _BumpScale ("Scale", float) = 1
    _BumpMap ("Normal Map", 2D) = "bump" {}
    _Parallax ("Height Scale", Range(0.005, 0.08)) = 0.02
    _ParallaxMap ("Height Map", 2D) = "black" {}
    _OcclusionStrength ("Strength", Range(0, 1)) = 1
    _OcclusionMap ("Occlusion", 2D) = "white" {}
    _EmissionColor ("Color", Color) = (0,0,0,1)
    _EmissionMap ("Emission", 2D) = "white" {}
    _DetailMask ("Detail Mask", 2D) = "white" {}
    _DetailAlbedoMap ("Detail Albedo x2", 2D) = "grey" {}
    _DetailNormalMapScale ("Scale", float) = 1
    _DetailNormalMap ("Normal Map", 2D) = "bump" {}
    [Enum(UV0,0,UV1,1)] _UVSec ("UV Set for secondary textures", float) = 0
    [HideInInspector] _Mode ("__mode", float) = 0
    [HideInInspector] _SrcBlend ("__src", float) = 1
    [HideInInspector] _DstBlend ("__dst", float) = 0
    [HideInInspector] _ZWrite ("__zw", float) = 1
  }
  SubShader
  {
    Tags
    { 
      "PerformanceChecks" = "False"
      "RenderType" = "Opaque"
    }
    LOD 300
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "FORWARDBASE"
        "PerformanceChecks" = "False"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      LOD 300
      ZWrite Off
      Blend Zero Zero
      GpuProgramID 60147
      // m_ProgramMask = 6
      //#ifdef DIRECTIONAL
      //  !!!!GLES
      CGPROGRAM
      #pragma multi_compile DIRECTIONAL
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      #define conv_mxt3x3_0(mat4x4) float3(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x)
      #define conv_mxt3x3_1(mat4x4) float3(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y)
      #define conv_mxt3x3_2(mat4x4) float3(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z)
      
      #define CODE_BLOCK_VERTEX
      uniform float4 _MainTex_ST;
      uniform float4 _DetailAlbedoMap_ST;
      uniform float _Metallic;
      uniform float _Glossiness;
      uniform float _UVSec;
      uniform float4 _LightColor0;
      uniform sampler2D unity_NHxRoughness;
      uniform float4 _Color;
      uniform sampler2D _MainTex;
      uniform sampler2D _OcclusionMap;
      uniform float _OcclusionStrength;
      struct IN_Data_Vert
      {
          float4 in_POSITION :POSITION;
          float3 in_NORMAL :NORMAL;
          float4 in_TEXCOORD0 :TEXCOORD0;
          float4 in_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float4 xlv_TEXCOORD5 :TEXCOORD5;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float4 xlv_TEXCOORD5 :TEXCOORD5;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          float3 tmpvar_1;
          tmpvar_1 = in_v.in_NORMAL;
          float2 tmpvar_2;
          tmpvar_2 = in_v.in_TEXCOORD0.xy;
          float2 tmpvar_3;
          tmpvar_3 = in_v.in_TEXCOORD1.xy;
          float3 normalWorld_4;
          float3 eyeVec_5;
          float4 tmpvar_6;
          float4 tmpvar_7;
          float4 tmpvar_8;
          float4 tmpvar_9;
          tmpvar_9 = mul(unity_ObjectToWorld, in_v.in_POSITION);
          float4 tmpvar_10;
          float4 tmpvar_11;
          tmpvar_11.w = 1;
          tmpvar_11.xyz = in_v.in_POSITION.xyz;
          tmpvar_10 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_11));
          float4 texcoord_12;
          texcoord_12.xy = ((in_v.in_TEXCOORD0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
          float2 tmpvar_13;
          if((_UVSec==0))
          {
              tmpvar_13 = tmpvar_2;
          }
          else
          {
              tmpvar_13 = tmpvar_3;
          }
          texcoord_12.zw = ((tmpvar_13 * _DetailAlbedoMap_ST.xy) + _DetailAlbedoMap_ST.zw);
          float3 tmpvar_14;
          tmpvar_14 = normalize((tmpvar_9.xyz - _WorldSpaceCameraPos));
          eyeVec_5 = tmpvar_14;
          float3 norm_15;
          norm_15 = tmpvar_1;
          float3x3 tmpvar_16;
          tmpvar_16[0] = unity_WorldToObject[0].xyz;
          tmpvar_16[1] = unity_WorldToObject[1].xyz;
          tmpvar_16[2] = unity_WorldToObject[2].xyz;
          float3 tmpvar_17;
          tmpvar_17 = normalize(mul(norm_15, tmpvar_16));
          normalWorld_4 = tmpvar_17;
          tmpvar_8.xyz = normalWorld_4;
          tmpvar_6.xyz = eyeVec_5;
          tmpvar_7.yzw = (eyeVec_5 - (2 * (dot(normalWorld_4, eyeVec_5) * normalWorld_4)));
          float x_18;
          x_18 = (1 - clamp(dot(normalWorld_4, (-eyeVec_5)), 0, 1));
          tmpvar_8.w = ((x_18 * x_18) * (x_18 * x_18));
          float tmpvar_19;
          tmpvar_19 = (1 - (0.7790837 - (_Metallic * 0.7790837)));
          float tmpvar_20;
          tmpvar_20 = clamp((_Glossiness + tmpvar_19), 0, 1);
          tmpvar_6.w = tmpvar_20;
          out_v.gl_Position = tmpvar_10;
          out_v.xlv_TEXCOORD0 = texcoord_12;
          out_v.xlv_TEXCOORD1 = tmpvar_6;
          out_v.xlv_TEXCOORD2 = float4(0, 0, 0, 0);
          out_v.xlv_TEXCOORD4 = tmpvar_7;
          out_v.xlv_TEXCOORD5 = tmpvar_8;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 impl_low_textureCubeLodEXT(samplerCUBE sampler, float3 coord, float lod)
      {
          return textureCubeLodEXT(sampler, coord, lod);
          return textureCube(sampler, coord, lod);
      }
      
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          float rl_1;
          float3 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy);
          float2 mg_4;
          mg_4.x = _Metallic;
          mg_4.y = _Glossiness;
          float tmpvar_5;
          tmpvar_5 = mg_4.y;
          float3 tmpvar_6;
          tmpvar_6 = (_Color.xyz * tmpvar_3.xyz);
          float3 tmpvar_7;
          tmpvar_7 = lerp(float3(0.2209163, 0.2209163, 0.2209163), tmpvar_6, float3(_Metallic, _Metallic, _Metallic));
          tmpvar_2 = in_f.xlv_TEXCOORD5.xyz;
          float3 tmpvar_8;
          tmpvar_8 = _LightColor0.xyz;
          float tmpvar_9;
          float tmpvar_10;
          tmpvar_10 = clamp(dot(tmpvar_2, _WorldSpaceLightPos0.xyz), 0, 1);
          tmpvar_9 = tmpvar_10;
          float occ_11;
          float tmpvar_12;
          tmpvar_12 = tex2D(_OcclusionMap, in_f.xlv_TEXCOORD0.xy).y;
          occ_11 = tmpvar_12;
          rl_1 = dot(in_f.xlv_TEXCOORD4.yzw, _WorldSpaceLightPos0.xyz);
          float4 tmpvar_13;
          tmpvar_13 = unity_SpecCube0_HDR;
          float tmpvar_14;
          float tmpvar_15;
          float smoothness_16;
          smoothness_16 = tmpvar_5;
          tmpvar_15 = (1 - smoothness_16);
          tmpvar_14 = tmpvar_15;
          float4 hdr_17;
          hdr_17 = tmpvar_13;
          float4 tmpvar_18;
          tmpvar_18.xyz = in_f.xlv_TEXCOORD4.yzw;
          tmpvar_18.w = ((tmpvar_14 * (1.7 - (0.7 * tmpvar_14))) * 6);
          float4 tmpvar_19;
          tmpvar_19 = impl_low_textureCubeLodEXT(unity_SpecCube0, in_f.xlv_TEXCOORD4.yzw, tmpvar_18.w);
          float4 tmpvar_20;
          tmpvar_20 = tmpvar_19;
          float tmpvar_21;
          tmpvar_21 = ((rl_1 * rl_1) * (rl_1 * rl_1));
          float specular_22;
          float smoothness_23;
          smoothness_23 = tmpvar_5;
          float2 tmpvar_24;
          tmpvar_24.x = tmpvar_21;
          tmpvar_24.y = (1 - smoothness_23);
          float tmpvar_25;
          tmpvar_25 = (tex2D(unity_NHxRoughness, tmpvar_24).w * 16);
          specular_22 = tmpvar_25;
          float4 tmpvar_26;
          tmpvar_26.w = 1;
          tmpvar_26.xyz = (((((hdr_17.x * ((hdr_17.w * (tmpvar_20.w - 1)) + 1)) * tmpvar_20.xyz) * ((1 - _OcclusionStrength) + (occ_11 * _OcclusionStrength))) * lerp(tmpvar_7, in_f.xlv_TEXCOORD1.www, in_f.xlv_TEXCOORD5.www)) + (((tmpvar_6 * (0.7790837 - (_Metallic * 0.7790837))) + (specular_22 * tmpvar_7)) * (tmpvar_8 * tmpvar_9)));
          float4 xlat_varoutput_27;
          xlat_varoutput_27.xyz = tmpvar_26.xyz;
          xlat_varoutput_27.w = 1;
          out_f.SV_Target0 = xlat_varoutput_27;
          return out_f;
      }
      
      
      //#endif // DIRECTIONAL
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: FORWARD_DELTA
    {
      Name "FORWARD_DELTA"
      Tags
      { 
        "LIGHTMODE" = "FORWARDADD"
        "PerformanceChecks" = "False"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      LOD 300
      ZWrite Off
      Blend Zero One
      GpuProgramID 81374
      // m_ProgramMask = 6
      //#ifdef POINT
      //  !!!!GLES
      CGPROGRAM
      #pragma multi_compile POINT
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      #define conv_mxt3x3_0(mat4x4) float3(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x)
      #define conv_mxt3x3_1(mat4x4) float3(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y)
      #define conv_mxt3x3_2(mat4x4) float3(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z)
      
      #define CODE_BLOCK_VERTEX
      uniform float4 _MainTex_ST;
      uniform float4 _DetailAlbedoMap_ST;
      uniform float _UVSec;
      uniform float4 _LightColor0;
      uniform sampler2D unity_NHxRoughness;
      uniform float4 _Color;
      uniform sampler2D _MainTex;
      uniform float _Metallic;
      uniform float _Glossiness;
      uniform sampler2D _LightTexture0;
      uniform float4x4 unity_WorldToLight;
      struct IN_Data_Vert
      {
          float4 in_POSITION :POSITION;
          float3 in_NORMAL :NORMAL;
          float4 in_TEXCOORD0 :TEXCOORD0;
          float4 in_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float3 xlv_TEXCOORD5 :TEXCOORD5;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float3 xlv_TEXCOORD5 :TEXCOORD5;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          float3 tmpvar_1;
          tmpvar_1 = in_v.in_NORMAL;
          float2 tmpvar_2;
          tmpvar_2 = in_v.in_TEXCOORD0.xy;
          float2 tmpvar_3;
          tmpvar_3 = in_v.in_TEXCOORD1.xy;
          float3 normalWorld_4;
          float3 eyeVec_5;
          float3 lightDir_6;
          float4 tmpvar_7;
          float4 tmpvar_8;
          tmpvar_8 = mul(unity_ObjectToWorld, in_v.in_POSITION);
          float4 tmpvar_9;
          float4 tmpvar_10;
          tmpvar_10.w = 1;
          tmpvar_10.xyz = in_v.in_POSITION.xyz;
          tmpvar_9 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_10));
          float4 texcoord_11;
          texcoord_11.xy = ((in_v.in_TEXCOORD0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
          float2 tmpvar_12;
          if((_UVSec==0))
          {
              tmpvar_12 = tmpvar_2;
          }
          else
          {
              tmpvar_12 = tmpvar_3;
          }
          texcoord_11.zw = ((tmpvar_12 * _DetailAlbedoMap_ST.xy) + _DetailAlbedoMap_ST.zw);
          float3 tmpvar_13;
          tmpvar_13 = (_WorldSpaceLightPos0.xyz - (tmpvar_8.xyz * _WorldSpaceLightPos0.w));
          lightDir_6 = tmpvar_13;
          float3 tmpvar_14;
          float3 n_15;
          n_15 = lightDir_6;
          float3 tmpvar_16;
          tmpvar_16 = normalize(n_15);
          tmpvar_14 = tmpvar_16;
          lightDir_6 = tmpvar_14;
          float3 tmpvar_17;
          tmpvar_17 = normalize((tmpvar_8.xyz - _WorldSpaceCameraPos));
          eyeVec_5 = tmpvar_17;
          float3 norm_18;
          norm_18 = tmpvar_1;
          float3x3 tmpvar_19;
          tmpvar_19[0] = unity_WorldToObject[0].xyz;
          tmpvar_19[1] = unity_WorldToObject[1].xyz;
          tmpvar_19[2] = unity_WorldToObject[2].xyz;
          float3 tmpvar_20;
          tmpvar_20 = normalize(mul(norm_18, tmpvar_19));
          normalWorld_4 = tmpvar_20;
          tmpvar_7.yzw = (eyeVec_5 - (2 * (dot(normalWorld_4, eyeVec_5) * normalWorld_4)));
          out_v.gl_Position = tmpvar_9;
          out_v.xlv_TEXCOORD0 = texcoord_11;
          out_v.xlv_TEXCOORD1 = tmpvar_8.xyz;
          out_v.xlv_TEXCOORD3 = tmpvar_7;
          out_v.xlv_TEXCOORD4 = tmpvar_14;
          out_v.xlv_TEXCOORD5 = normalWorld_4;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          float atten_1;
          float3 lightCoord_2;
          float3 c_3;
          float4 tmpvar_4;
          tmpvar_4 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy);
          float2 mg_5;
          mg_5.x = _Metallic;
          mg_5.y = _Glossiness;
          float tmpvar_6;
          tmpvar_6 = mg_5.y;
          float3 tmpvar_7;
          tmpvar_7 = (_Color.xyz * tmpvar_4.xyz);
          float tmpvar_8;
          tmpvar_8 = dot(in_f.xlv_TEXCOORD3.yzw, in_f.xlv_TEXCOORD4);
          float tmpvar_9;
          tmpvar_9 = ((tmpvar_8 * tmpvar_8) * (tmpvar_8 * tmpvar_8));
          float specular_10;
          float smoothness_11;
          smoothness_11 = tmpvar_6;
          float2 tmpvar_12;
          tmpvar_12.x = tmpvar_9;
          tmpvar_12.y = (1 - smoothness_11);
          float tmpvar_13;
          tmpvar_13 = (tex2D(unity_NHxRoughness, tmpvar_12).w * 16);
          specular_10 = tmpvar_13;
          c_3 = (((tmpvar_7 * (0.7790837 - (_Metallic * 0.7790837))) + (specular_10 * lerp(float3(0.2209163, 0.2209163, 0.2209163), tmpvar_7, float3(_Metallic, _Metallic, _Metallic)))) * _LightColor0.xyz);
          float4 tmpvar_14;
          tmpvar_14.w = 1;
          tmpvar_14.xyz = in_f.xlv_TEXCOORD1;
          lightCoord_2 = mul(unity_WorldToLight, tmpvar_14).xyz;
          float tmpvar_15;
          float _tmp_dvx_0 = dot(lightCoord_2, lightCoord_2);
          tmpvar_15 = tex2D(_LightTexture0, float2(_tmp_dvx_0, _tmp_dvx_0)).w;
          atten_1 = tmpvar_15;
          c_3 = (c_3 * (atten_1 * clamp(dot(in_f.xlv_TEXCOORD5, in_f.xlv_TEXCOORD4), 0, 1)));
          float4 tmpvar_16;
          tmpvar_16.w = 1;
          tmpvar_16.xyz = c_3;
          float4 xlat_varoutput_17;
          xlat_varoutput_17.xyz = tmpvar_16.xyz;
          xlat_varoutput_17.w = 1;
          out_f.SV_Target0 = xlat_varoutput_17;
          return out_f;
      }
      
      
      //#endif // POINT
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: SHADOWCASTER
    {
      Name "SHADOWCASTER"
      Tags
      { 
        "LIGHTMODE" = "SHADOWCASTER"
        "PerformanceChecks" = "False"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      LOD 300
      GpuProgramID 195972
      // m_ProgramMask = 6
      //#ifdef SHADOWS_DEPTH
      //  !!!!GLES
      CGPROGRAM
      #pragma multi_compile SHADOWS_DEPTH
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      #define conv_mxt3x3_0(mat4x4) float3(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x)
      #define conv_mxt3x3_1(mat4x4) float3(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y)
      #define conv_mxt3x3_2(mat4x4) float3(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z)
      
      #define CODE_BLOCK_VERTEX
      struct IN_Data_Vert
      {
          float4 in_POSITION :POSITION;
          float3 in_NORMAL :NORMAL;
      };
      
      struct OUT_Data_Vert
      {
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 wPos_2;
          float4 tmpvar_3;
          tmpvar_3 = mul(unity_ObjectToWorld, in_v.in_POSITION);
          wPos_2 = tmpvar_3;
          if((unity_LightShadowBias.z!=0))
          {
              float3x3 tmpvar_4;
              tmpvar_4[0] = unity_WorldToObject[0].xyz;
              tmpvar_4[1] = unity_WorldToObject[1].xyz;
              tmpvar_4[2] = unity_WorldToObject[2].xyz;
              float3 tmpvar_5;
              tmpvar_5 = normalize(mul(in_v.in_NORMAL, tmpvar_4));
              float tmpvar_6;
              tmpvar_6 = dot(tmpvar_5, normalize((_WorldSpaceLightPos0.xyz - (tmpvar_3.xyz * _WorldSpaceLightPos0.w))));
              wPos_2.xyz = (tmpvar_3.xyz - (tmpvar_5 * (unity_LightShadowBias.z * sqrt((1 - (tmpvar_6 * tmpvar_6))))));
          }
          tmpvar_1 = mul(unity_MatrixVP, wPos_2);
          float4 clipPos_7;
          clipPos_7.xyw = tmpvar_1.xyw;
          clipPos_7.z = (tmpvar_1.z + clamp((unity_LightShadowBias.x / tmpvar_1.w), 0, 1));
          clipPos_7.z = lerp(clipPos_7.z, max(clipPos_7.z, (-tmpvar_1.w)), unity_LightShadowBias.y);
          out_v.gl_Position = clipPos_7;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          out_f.SV_Target0 = float4(0, 0, 0, 0);
          return out_f;
      }
      
      
      //#endif // SHADOWS_DEPTH
      ENDCG
      
    } // end phase
    Pass // ind: 4, name: DEFERRED
    {
      Name "DEFERRED"
      Tags
      { 
        "LIGHTMODE" = "DEFERRED"
        "PerformanceChecks" = "False"
        "RenderType" = "Opaque"
      }
      LOD 300
      GpuProgramID 201409
      // m_ProgramMask = 6
      //  !!!!GLES
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      #define conv_mxt3x3_0(mat4x4) float3(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x)
      #define conv_mxt3x3_1(mat4x4) float3(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y)
      #define conv_mxt3x3_2(mat4x4) float3(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z)
      
      #define CODE_BLOCK_VERTEX
      uniform float4 _MainTex_ST;
      uniform float4 _DetailAlbedoMap_ST;
      uniform float _UVSec;
      uniform float4 _Color;
      uniform sampler2D _MainTex;
      uniform float _Metallic;
      uniform float _Glossiness;
      uniform sampler2D _OcclusionMap;
      uniform float _OcclusionStrength;
      struct IN_Data_Vert
      {
          float4 in_POSITION :POSITION;
          float3 in_NORMAL :NORMAL;
          float4 in_TEXCOORD0 :TEXCOORD0;
          float4 in_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD2_1 :TEXCOORD2_1;
          float4 xlv_TEXCOORD2_2 :TEXCOORD2_2;
          float4 xlv_TEXCOORD5 :TEXCOORD5;
          float3 xlv_TEXCOORD6 :TEXCOORD6;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD2_2 :TEXCOORD2_2;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
          float4 SV_Target1 :SV_Target1;
          float4 SV_Target2 :SV_Target2;
          float4 SV_Target3 :SV_Target3;
      };
      
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          float3 tmpvar_1;
          tmpvar_1 = in_v.in_NORMAL;
          float2 tmpvar_2;
          tmpvar_2 = in_v.in_TEXCOORD0.xy;
          float2 tmpvar_3;
          tmpvar_3 = in_v.in_TEXCOORD1.xy;
          float3 tmpvar_4;
          float4 tmpvar_5;
          float4 tmpvar_6;
          float4 tmpvar_7;
          float3 tmpvar_8;
          float4 tmpvar_9;
          tmpvar_9 = mul(unity_ObjectToWorld, in_v.in_POSITION);
          tmpvar_8 = tmpvar_9.xyz;
          float4 tmpvar_10;
          float4 tmpvar_11;
          tmpvar_11.w = 1;
          tmpvar_11.xyz = in_v.in_POSITION.xyz;
          tmpvar_10 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_11));
          float4 texcoord_12;
          texcoord_12.xy = ((in_v.in_TEXCOORD0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
          float2 tmpvar_13;
          if((_UVSec==0))
          {
              tmpvar_13 = tmpvar_2;
          }
          else
          {
              tmpvar_13 = tmpvar_3;
          }
          texcoord_12.zw = ((tmpvar_13 * _DetailAlbedoMap_ST.xy) + _DetailAlbedoMap_ST.zw);
          float3 tmpvar_14;
          float3 n_15;
          n_15 = (tmpvar_9.xyz - _WorldSpaceCameraPos);
          tmpvar_14 = n_15;
          tmpvar_4 = tmpvar_14;
          float3 norm_16;
          norm_16 = tmpvar_1;
          float3x3 tmpvar_17;
          tmpvar_17[0] = unity_WorldToObject[0].xyz;
          tmpvar_17[1] = unity_WorldToObject[1].xyz;
          tmpvar_17[2] = unity_WorldToObject[2].xyz;
          tmpvar_5.xyz = float3(0, 0, 0);
          tmpvar_6.xyz = float3(0, 0, 0);
          tmpvar_7.xyz = normalize(mul(norm_16, tmpvar_17));
          out_v.gl_Position = tmpvar_10;
          out_v.xlv_TEXCOORD0 = texcoord_12;
          out_v.xlv_TEXCOORD1 = tmpvar_4;
          out_v.xlv_TEXCOORD2 = tmpvar_5;
          out_v.xlv_TEXCOORD2_1 = tmpvar_6;
          out_v.xlv_TEXCOORD2_2 = tmpvar_7;
          out_v.xlv_TEXCOORD5 = float4(0, 0, 0, 0);
          out_v.xlv_TEXCOORD6 = tmpvar_8;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          float4 tmpvar_1;
          tmpvar_1 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy);
          float2 mg_2;
          mg_2.x = _Metallic;
          mg_2.y = _Glossiness;
          float3 tmpvar_3;
          tmpvar_3 = (_Color.xyz * tmpvar_1.xyz);
          float occ_4;
          float tmpvar_5;
          tmpvar_5 = tex2D(_OcclusionMap, in_f.xlv_TEXCOORD0.xy).y;
          occ_4 = tmpvar_5;
          float4 outGBuffer2_6;
          float4 tmpvar_7;
          tmpvar_7.xyz = (tmpvar_3 * (0.7790837 - (_Metallic * 0.7790837)));
          tmpvar_7.w = ((1 - _OcclusionStrength) + (occ_4 * _OcclusionStrength));
          float4 tmpvar_8;
          tmpvar_8.xyz = lerp(float3(0.2209163, 0.2209163, 0.2209163), tmpvar_3, float3(_Metallic, _Metallic, _Metallic));
          tmpvar_8.w = mg_2.y;
          float4 tmpvar_9;
          tmpvar_9.w = 1;
          tmpvar_9.xyz = ((normalize(in_f.xlv_TEXCOORD2_2.xyz) * 0.5) + 0.5);
          outGBuffer2_6 = tmpvar_9;
          float4 tmpvar_10;
          tmpvar_10.w = 1;
          tmpvar_10.xyz = float3(1, 1, 1);
          out_f.SV_Target0 = tmpvar_7;
          out_f.SV_Target1 = tmpvar_8;
          out_f.SV_Target2 = outGBuffer2_6;
          out_f.SV_Target3 = tmpvar_10;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 5, name: META
    {
      Name "META"
      Tags
      { 
        "LIGHTMODE" = "META"
        "PerformanceChecks" = "False"
        "RenderType" = "Opaque"
      }
      LOD 300
      Cull Off
      GpuProgramID 287608
      // m_ProgramMask = 6
      //  !!!!GLES
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      #define conv_mxt3x3_0(mat4x4) float3(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x)
      #define conv_mxt3x3_1(mat4x4) float3(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y)
      #define conv_mxt3x3_2(mat4x4) float3(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z)
      
      #define CODE_BLOCK_VERTEX
      // uniform float4 unity_LightmapST;
      // uniform float4 unity_DynamicLightmapST;
      uniform float4 _MainTex_ST;
      uniform float4 _DetailAlbedoMap_ST;
      uniform float _UVSec;
      uniform bool4 unity_MetaVertexControl;
      uniform float4 _Color;
      uniform sampler2D _MainTex;
      uniform float _Metallic;
      uniform float _Glossiness;
      uniform bool4 unity_MetaFragmentControl;
      uniform float unity_OneOverOutputBoost;
      uniform float unity_MaxOutputValue;
      uniform float unity_UseLinearSpace;
      struct IN_Data_Vert
      {
          float4 in_POSITION :POSITION;
          float4 in_TEXCOORD0 :TEXCOORD0;
          float4 in_TEXCOORD1 :TEXCOORD1;
          float4 in_TEXCOORD2 :TEXCOORD2;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          float2 tmpvar_1;
          tmpvar_1 = in_v.in_TEXCOORD0.xy;
          float2 tmpvar_2;
          tmpvar_2 = in_v.in_TEXCOORD1.xy;
          float4 vertex_3;
          vertex_3 = in_v.in_POSITION;
          if(unity_MetaVertexControl.x)
          {
              vertex_3.xy = ((in_v.in_TEXCOORD1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
              float tmpvar_4;
              if((in_v.in_POSITION.z>0))
              {
                  tmpvar_4 = 0.0001;
              }
              else
              {
                  tmpvar_4 = 0;
              }
              vertex_3.z = tmpvar_4;
          }
          if(unity_MetaVertexControl.y)
          {
              vertex_3.xy = ((in_v.in_TEXCOORD2.xy * unity_DynamicLightmapST.xy) + unity_DynamicLightmapST.zw);
              float tmpvar_5;
              if((vertex_3.z>0))
              {
                  tmpvar_5 = 0.0001;
              }
              else
              {
                  tmpvar_5 = 0;
              }
              vertex_3.z = tmpvar_5;
          }
          float4 tmpvar_6;
          float4 tmpvar_7;
          tmpvar_7.w = 1;
          tmpvar_7.xyz = vertex_3.xyz;
          tmpvar_6 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_7));
          float4 texcoord_8;
          texcoord_8.xy = ((in_v.in_TEXCOORD0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
          float2 tmpvar_9;
          if((_UVSec==0))
          {
              tmpvar_9 = tmpvar_1;
          }
          else
          {
              tmpvar_9 = tmpvar_2;
          }
          texcoord_8.zw = ((tmpvar_9 * _DetailAlbedoMap_ST.xy) + _DetailAlbedoMap_ST.zw);
          out_v.xlv_TEXCOORD0 = texcoord_8;
          out_v.gl_Position = tmpvar_6;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          float4 tmpvar_1;
          float2 mg_2;
          mg_2.x = _Metallic;
          mg_2.y = _Glossiness;
          float4 tmpvar_3;
          tmpvar_3 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy);
          float3 tmpvar_4;
          tmpvar_4 = (_Color.xyz * tmpvar_3.xyz);
          float3 res_5;
          res_5 = ((tmpvar_4 * (0.7790837 - (_Metallic * 0.7790837))) + ((lerp(float3(0.2209163, 0.2209163, 0.2209163), tmpvar_4, float3(_Metallic, _Metallic, _Metallic)) * ((1 - mg_2.y) * (1 - mg_2.y))) * 0.5));
          float4 res_6;
          res_6 = float4(0, 0, 0, 0);
          if(unity_MetaFragmentControl.x)
          {
              float4 tmpvar_7;
              tmpvar_7.w = 1;
              tmpvar_7.xyz = res_5;
              res_6.w = tmpvar_7.w;
              float3 tmpvar_8;
              float _tmp_dvx_1 = clamp(unity_OneOverOutputBoost, 0, 1);
              tmpvar_8 = clamp(pow(res_5, float3(_tmp_dvx_1, _tmp_dvx_1, _tmp_dvx_1)), float3(0, 0, 0), float3(unity_MaxOutputValue, unity_MaxOutputValue, unity_MaxOutputValue));
              res_6.xyz = tmpvar_8;
          }
          if(unity_MetaFragmentControl.y)
          {
              float3 emission_9;
              if(int(unity_UseLinearSpace))
              {
                  emission_9 = float3(0, 0, 0);
              }
              else
              {
                  emission_9 = float3(0, 0, 0);
              }
              float4 tmpvar_10;
              tmpvar_10.w = 1;
              tmpvar_10.xyz = emission_9;
              res_6 = tmpvar_10;
          }
          tmpvar_1 = res_6;
          out_f.SV_Target0 = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  SubShader
  {
    Tags
    { 
      "PerformanceChecks" = "False"
      "RenderType" = "Opaque"
    }
    LOD 150
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "FORWARDBASE"
        "PerformanceChecks" = "False"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      LOD 150
      ZWrite Off
      Blend Zero Zero
      GpuProgramID 378374
      // m_ProgramMask = 6
      //#ifdef DIRECTIONAL
      //  !!!!GLES
      CGPROGRAM
      #pragma multi_compile DIRECTIONAL
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      #define conv_mxt3x3_0(mat4x4) float3(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x)
      #define conv_mxt3x3_1(mat4x4) float3(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y)
      #define conv_mxt3x3_2(mat4x4) float3(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z)
      
      #define CODE_BLOCK_VERTEX
      uniform float4 _MainTex_ST;
      uniform float4 _DetailAlbedoMap_ST;
      uniform float _Metallic;
      uniform float _Glossiness;
      uniform float _UVSec;
      uniform float4 _LightColor0;
      uniform sampler2D unity_NHxRoughness;
      uniform float4 _Color;
      uniform sampler2D _MainTex;
      uniform sampler2D _OcclusionMap;
      struct IN_Data_Vert
      {
          float4 in_POSITION :POSITION;
          float3 in_NORMAL :NORMAL;
          float4 in_TEXCOORD0 :TEXCOORD0;
          float4 in_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float4 xlv_TEXCOORD5 :TEXCOORD5;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float4 xlv_TEXCOORD5 :TEXCOORD5;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          float3 tmpvar_1;
          tmpvar_1 = in_v.in_NORMAL;
          float2 tmpvar_2;
          tmpvar_2 = in_v.in_TEXCOORD0.xy;
          float2 tmpvar_3;
          tmpvar_3 = in_v.in_TEXCOORD1.xy;
          float3 normalWorld_4;
          float3 eyeVec_5;
          float4 tmpvar_6;
          float4 tmpvar_7;
          float4 tmpvar_8;
          float4 tmpvar_9;
          tmpvar_9 = mul(unity_ObjectToWorld, in_v.in_POSITION);
          float4 tmpvar_10;
          float4 tmpvar_11;
          tmpvar_11.w = 1;
          tmpvar_11.xyz = in_v.in_POSITION.xyz;
          tmpvar_10 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_11));
          float4 texcoord_12;
          texcoord_12.xy = ((in_v.in_TEXCOORD0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
          float2 tmpvar_13;
          if((_UVSec==0))
          {
              tmpvar_13 = tmpvar_2;
          }
          else
          {
              tmpvar_13 = tmpvar_3;
          }
          texcoord_12.zw = ((tmpvar_13 * _DetailAlbedoMap_ST.xy) + _DetailAlbedoMap_ST.zw);
          float3 tmpvar_14;
          tmpvar_14 = normalize((tmpvar_9.xyz - _WorldSpaceCameraPos));
          eyeVec_5 = tmpvar_14;
          float3 norm_15;
          norm_15 = tmpvar_1;
          float3x3 tmpvar_16;
          tmpvar_16[0] = unity_WorldToObject[0].xyz;
          tmpvar_16[1] = unity_WorldToObject[1].xyz;
          tmpvar_16[2] = unity_WorldToObject[2].xyz;
          float3 tmpvar_17;
          tmpvar_17 = normalize(mul(norm_15, tmpvar_16));
          normalWorld_4 = tmpvar_17;
          tmpvar_8.xyz = normalWorld_4;
          tmpvar_6.xyz = eyeVec_5;
          tmpvar_7.yzw = (eyeVec_5 - (2 * (dot(normalWorld_4, eyeVec_5) * normalWorld_4)));
          float x_18;
          x_18 = (1 - clamp(dot(normalWorld_4, (-eyeVec_5)), 0, 1));
          tmpvar_8.w = ((x_18 * x_18) * (x_18 * x_18));
          float tmpvar_19;
          tmpvar_19 = (1 - (0.7790837 - (_Metallic * 0.7790837)));
          float tmpvar_20;
          tmpvar_20 = clamp((_Glossiness + tmpvar_19), 0, 1);
          tmpvar_6.w = tmpvar_20;
          out_v.gl_Position = tmpvar_10;
          out_v.xlv_TEXCOORD0 = texcoord_12;
          out_v.xlv_TEXCOORD1 = tmpvar_6;
          out_v.xlv_TEXCOORD2 = float4(0, 0, 0, 0);
          out_v.xlv_TEXCOORD4 = tmpvar_7;
          out_v.xlv_TEXCOORD5 = tmpvar_8;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 impl_low_textureCubeLodEXT(samplerCUBE sampler, float3 coord, float lod)
      {
          return textureCubeLodEXT(sampler, coord, lod);
          return textureCube(sampler, coord, lod);
      }
      
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          float rl_1;
          float3 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy);
          float2 mg_4;
          mg_4.x = _Metallic;
          mg_4.y = _Glossiness;
          float tmpvar_5;
          tmpvar_5 = mg_4.y;
          float3 tmpvar_6;
          tmpvar_6 = (_Color.xyz * tmpvar_3.xyz);
          float3 tmpvar_7;
          tmpvar_7 = lerp(float3(0.2209163, 0.2209163, 0.2209163), tmpvar_6, float3(_Metallic, _Metallic, _Metallic));
          tmpvar_2 = in_f.xlv_TEXCOORD5.xyz;
          float3 tmpvar_8;
          tmpvar_8 = _LightColor0.xyz;
          float tmpvar_9;
          float tmpvar_10;
          tmpvar_10 = clamp(dot(tmpvar_2, _WorldSpaceLightPos0.xyz), 0, 1);
          tmpvar_9 = tmpvar_10;
          float tmpvar_11;
          float4 tmpvar_12;
          tmpvar_12 = tex2D(_OcclusionMap, in_f.xlv_TEXCOORD0.xy);
          tmpvar_11 = tmpvar_12.y;
          rl_1 = dot(in_f.xlv_TEXCOORD4.yzw, _WorldSpaceLightPos0.xyz);
          float4 tmpvar_13;
          tmpvar_13 = unity_SpecCube0_HDR;
          float tmpvar_14;
          float tmpvar_15;
          float smoothness_16;
          smoothness_16 = tmpvar_5;
          tmpvar_15 = (1 - smoothness_16);
          tmpvar_14 = tmpvar_15;
          float4 hdr_17;
          hdr_17 = tmpvar_13;
          float4 tmpvar_18;
          tmpvar_18.xyz = in_f.xlv_TEXCOORD4.yzw;
          tmpvar_18.w = ((tmpvar_14 * (1.7 - (0.7 * tmpvar_14))) * 6);
          float4 tmpvar_19;
          tmpvar_19 = impl_low_textureCubeLodEXT(unity_SpecCube0, in_f.xlv_TEXCOORD4.yzw, tmpvar_18.w);
          float4 tmpvar_20;
          tmpvar_20 = tmpvar_19;
          float tmpvar_21;
          tmpvar_21 = ((rl_1 * rl_1) * (rl_1 * rl_1));
          float specular_22;
          float smoothness_23;
          smoothness_23 = tmpvar_5;
          float2 tmpvar_24;
          tmpvar_24.x = tmpvar_21;
          tmpvar_24.y = (1 - smoothness_23);
          float tmpvar_25;
          tmpvar_25 = (tex2D(unity_NHxRoughness, tmpvar_24).w * 16);
          specular_22 = tmpvar_25;
          float4 tmpvar_26;
          tmpvar_26.w = 1;
          tmpvar_26.xyz = (((((hdr_17.x * ((hdr_17.w * (tmpvar_20.w - 1)) + 1)) * tmpvar_20.xyz) * tmpvar_11) * lerp(tmpvar_7, in_f.xlv_TEXCOORD1.www, in_f.xlv_TEXCOORD5.www)) + (((tmpvar_6 * (0.7790837 - (_Metallic * 0.7790837))) + (specular_22 * tmpvar_7)) * (tmpvar_8 * tmpvar_9)));
          float4 xlat_varoutput_27;
          xlat_varoutput_27.xyz = tmpvar_26.xyz;
          xlat_varoutput_27.w = 1;
          out_f.SV_Target0 = xlat_varoutput_27;
          return out_f;
      }
      
      
      //#endif // DIRECTIONAL
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: FORWARD_DELTA
    {
      Name "FORWARD_DELTA"
      Tags
      { 
        "LIGHTMODE" = "FORWARDADD"
        "PerformanceChecks" = "False"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      LOD 150
      ZWrite Off
      Blend Zero One
      GpuProgramID 453023
      // m_ProgramMask = 6
      //#ifdef POINT
      //  !!!!GLES
      CGPROGRAM
      #pragma multi_compile POINT
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      #define conv_mxt3x3_0(mat4x4) float3(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x)
      #define conv_mxt3x3_1(mat4x4) float3(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y)
      #define conv_mxt3x3_2(mat4x4) float3(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z)
      
      #define CODE_BLOCK_VERTEX
      uniform float4 _MainTex_ST;
      uniform float4 _DetailAlbedoMap_ST;
      uniform float _UVSec;
      uniform float4 _LightColor0;
      uniform sampler2D unity_NHxRoughness;
      uniform float4 _Color;
      uniform sampler2D _MainTex;
      uniform float _Metallic;
      uniform float _Glossiness;
      uniform sampler2D _LightTexture0;
      uniform float4x4 unity_WorldToLight;
      struct IN_Data_Vert
      {
          float4 in_POSITION :POSITION;
          float3 in_NORMAL :NORMAL;
          float4 in_TEXCOORD0 :TEXCOORD0;
          float4 in_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float3 xlv_TEXCOORD5 :TEXCOORD5;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float3 xlv_TEXCOORD5 :TEXCOORD5;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          float3 tmpvar_1;
          tmpvar_1 = in_v.in_NORMAL;
          float2 tmpvar_2;
          tmpvar_2 = in_v.in_TEXCOORD0.xy;
          float2 tmpvar_3;
          tmpvar_3 = in_v.in_TEXCOORD1.xy;
          float3 normalWorld_4;
          float3 eyeVec_5;
          float3 lightDir_6;
          float4 tmpvar_7;
          float4 tmpvar_8;
          tmpvar_8 = mul(unity_ObjectToWorld, in_v.in_POSITION);
          float4 tmpvar_9;
          float4 tmpvar_10;
          tmpvar_10.w = 1;
          tmpvar_10.xyz = in_v.in_POSITION.xyz;
          tmpvar_9 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_10));
          float4 texcoord_11;
          texcoord_11.xy = ((in_v.in_TEXCOORD0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
          float2 tmpvar_12;
          if((_UVSec==0))
          {
              tmpvar_12 = tmpvar_2;
          }
          else
          {
              tmpvar_12 = tmpvar_3;
          }
          texcoord_11.zw = ((tmpvar_12 * _DetailAlbedoMap_ST.xy) + _DetailAlbedoMap_ST.zw);
          float3 tmpvar_13;
          tmpvar_13 = (_WorldSpaceLightPos0.xyz - (tmpvar_8.xyz * _WorldSpaceLightPos0.w));
          lightDir_6 = tmpvar_13;
          float3 tmpvar_14;
          float3 n_15;
          n_15 = lightDir_6;
          float3 tmpvar_16;
          tmpvar_16 = normalize(n_15);
          tmpvar_14 = tmpvar_16;
          lightDir_6 = tmpvar_14;
          float3 tmpvar_17;
          tmpvar_17 = normalize((tmpvar_8.xyz - _WorldSpaceCameraPos));
          eyeVec_5 = tmpvar_17;
          float3 norm_18;
          norm_18 = tmpvar_1;
          float3x3 tmpvar_19;
          tmpvar_19[0] = unity_WorldToObject[0].xyz;
          tmpvar_19[1] = unity_WorldToObject[1].xyz;
          tmpvar_19[2] = unity_WorldToObject[2].xyz;
          float3 tmpvar_20;
          tmpvar_20 = normalize(mul(norm_18, tmpvar_19));
          normalWorld_4 = tmpvar_20;
          tmpvar_7.yzw = (eyeVec_5 - (2 * (dot(normalWorld_4, eyeVec_5) * normalWorld_4)));
          out_v.gl_Position = tmpvar_9;
          out_v.xlv_TEXCOORD0 = texcoord_11;
          out_v.xlv_TEXCOORD1 = tmpvar_8.xyz;
          out_v.xlv_TEXCOORD3 = tmpvar_7;
          out_v.xlv_TEXCOORD4 = tmpvar_14;
          out_v.xlv_TEXCOORD5 = normalWorld_4;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          float atten_1;
          float3 lightCoord_2;
          float3 c_3;
          float4 tmpvar_4;
          tmpvar_4 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy);
          float2 mg_5;
          mg_5.x = _Metallic;
          mg_5.y = _Glossiness;
          float tmpvar_6;
          tmpvar_6 = mg_5.y;
          float3 tmpvar_7;
          tmpvar_7 = (_Color.xyz * tmpvar_4.xyz);
          float tmpvar_8;
          tmpvar_8 = dot(in_f.xlv_TEXCOORD3.yzw, in_f.xlv_TEXCOORD4);
          float tmpvar_9;
          tmpvar_9 = ((tmpvar_8 * tmpvar_8) * (tmpvar_8 * tmpvar_8));
          float specular_10;
          float smoothness_11;
          smoothness_11 = tmpvar_6;
          float2 tmpvar_12;
          tmpvar_12.x = tmpvar_9;
          tmpvar_12.y = (1 - smoothness_11);
          float tmpvar_13;
          tmpvar_13 = (tex2D(unity_NHxRoughness, tmpvar_12).w * 16);
          specular_10 = tmpvar_13;
          c_3 = (((tmpvar_7 * (0.7790837 - (_Metallic * 0.7790837))) + (specular_10 * lerp(float3(0.2209163, 0.2209163, 0.2209163), tmpvar_7, float3(_Metallic, _Metallic, _Metallic)))) * _LightColor0.xyz);
          float4 tmpvar_14;
          tmpvar_14.w = 1;
          tmpvar_14.xyz = in_f.xlv_TEXCOORD1;
          lightCoord_2 = mul(unity_WorldToLight, tmpvar_14).xyz;
          float tmpvar_15;
          float _tmp_dvx_2 = dot(lightCoord_2, lightCoord_2);
          tmpvar_15 = tex2D(_LightTexture0, float2(_tmp_dvx_2, _tmp_dvx_2)).w;
          atten_1 = tmpvar_15;
          c_3 = (c_3 * (atten_1 * clamp(dot(in_f.xlv_TEXCOORD5, in_f.xlv_TEXCOORD4), 0, 1)));
          float4 tmpvar_16;
          tmpvar_16.w = 1;
          tmpvar_16.xyz = c_3;
          float4 xlat_varoutput_17;
          xlat_varoutput_17.xyz = tmpvar_16.xyz;
          xlat_varoutput_17.w = 1;
          out_f.SV_Target0 = xlat_varoutput_17;
          return out_f;
      }
      
      
      //#endif // POINT
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: SHADOWCASTER
    {
      Name "SHADOWCASTER"
      Tags
      { 
        "LIGHTMODE" = "SHADOWCASTER"
        "PerformanceChecks" = "False"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      LOD 150
      GpuProgramID 518401
      // m_ProgramMask = 6
      //#ifdef SHADOWS_DEPTH
      //  !!!!GLES
      CGPROGRAM
      #pragma multi_compile SHADOWS_DEPTH
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      #define conv_mxt3x3_0(mat4x4) float3(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x)
      #define conv_mxt3x3_1(mat4x4) float3(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y)
      #define conv_mxt3x3_2(mat4x4) float3(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z)
      
      #define CODE_BLOCK_VERTEX
      struct IN_Data_Vert
      {
          float4 in_POSITION :POSITION;
          float3 in_NORMAL :NORMAL;
      };
      
      struct OUT_Data_Vert
      {
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 wPos_2;
          float4 tmpvar_3;
          tmpvar_3 = mul(unity_ObjectToWorld, in_v.in_POSITION);
          wPos_2 = tmpvar_3;
          if((unity_LightShadowBias.z!=0))
          {
              float3x3 tmpvar_4;
              tmpvar_4[0] = unity_WorldToObject[0].xyz;
              tmpvar_4[1] = unity_WorldToObject[1].xyz;
              tmpvar_4[2] = unity_WorldToObject[2].xyz;
              float3 tmpvar_5;
              tmpvar_5 = normalize(mul(in_v.in_NORMAL, tmpvar_4));
              float tmpvar_6;
              tmpvar_6 = dot(tmpvar_5, normalize((_WorldSpaceLightPos0.xyz - (tmpvar_3.xyz * _WorldSpaceLightPos0.w))));
              wPos_2.xyz = (tmpvar_3.xyz - (tmpvar_5 * (unity_LightShadowBias.z * sqrt((1 - (tmpvar_6 * tmpvar_6))))));
          }
          tmpvar_1 = mul(unity_MatrixVP, wPos_2);
          float4 clipPos_7;
          clipPos_7.xyw = tmpvar_1.xyw;
          clipPos_7.z = (tmpvar_1.z + clamp((unity_LightShadowBias.x / tmpvar_1.w), 0, 1));
          clipPos_7.z = lerp(clipPos_7.z, max(clipPos_7.z, (-tmpvar_1.w)), unity_LightShadowBias.y);
          out_v.gl_Position = clipPos_7;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          out_f.SV_Target0 = float4(0, 0, 0, 0);
          return out_f;
      }
      
      
      //#endif // SHADOWS_DEPTH
      ENDCG
      
    } // end phase
    Pass // ind: 4, name: META
    {
      Name "META"
      Tags
      { 
        "LIGHTMODE" = "META"
        "PerformanceChecks" = "False"
        "RenderType" = "Opaque"
      }
      LOD 150
      Cull Off
      GpuProgramID 569850
      // m_ProgramMask = 6
      //  !!!!GLES
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      #define conv_mxt3x3_0(mat4x4) float3(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x)
      #define conv_mxt3x3_1(mat4x4) float3(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y)
      #define conv_mxt3x3_2(mat4x4) float3(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z)
      
      #define CODE_BLOCK_VERTEX
      // uniform float4 unity_LightmapST;
      // uniform float4 unity_DynamicLightmapST;
      uniform float4 _MainTex_ST;
      uniform float4 _DetailAlbedoMap_ST;
      uniform float _UVSec;
      uniform bool4 unity_MetaVertexControl;
      uniform float4 _Color;
      uniform sampler2D _MainTex;
      uniform float _Metallic;
      uniform float _Glossiness;
      uniform bool4 unity_MetaFragmentControl;
      uniform float unity_OneOverOutputBoost;
      uniform float unity_MaxOutputValue;
      uniform float unity_UseLinearSpace;
      struct IN_Data_Vert
      {
          float4 in_POSITION :POSITION;
          float4 in_TEXCOORD0 :TEXCOORD0;
          float4 in_TEXCOORD1 :TEXCOORD1;
          float4 in_TEXCOORD2 :TEXCOORD2;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          float2 tmpvar_1;
          tmpvar_1 = in_v.in_TEXCOORD0.xy;
          float2 tmpvar_2;
          tmpvar_2 = in_v.in_TEXCOORD1.xy;
          float4 vertex_3;
          vertex_3 = in_v.in_POSITION;
          if(unity_MetaVertexControl.x)
          {
              vertex_3.xy = ((in_v.in_TEXCOORD1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
              float tmpvar_4;
              if((in_v.in_POSITION.z>0))
              {
                  tmpvar_4 = 0.0001;
              }
              else
              {
                  tmpvar_4 = 0;
              }
              vertex_3.z = tmpvar_4;
          }
          if(unity_MetaVertexControl.y)
          {
              vertex_3.xy = ((in_v.in_TEXCOORD2.xy * unity_DynamicLightmapST.xy) + unity_DynamicLightmapST.zw);
              float tmpvar_5;
              if((vertex_3.z>0))
              {
                  tmpvar_5 = 0.0001;
              }
              else
              {
                  tmpvar_5 = 0;
              }
              vertex_3.z = tmpvar_5;
          }
          float4 tmpvar_6;
          float4 tmpvar_7;
          tmpvar_7.w = 1;
          tmpvar_7.xyz = vertex_3.xyz;
          tmpvar_6 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_7));
          float4 texcoord_8;
          texcoord_8.xy = ((in_v.in_TEXCOORD0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
          float2 tmpvar_9;
          if((_UVSec==0))
          {
              tmpvar_9 = tmpvar_1;
          }
          else
          {
              tmpvar_9 = tmpvar_2;
          }
          texcoord_8.zw = ((tmpvar_9 * _DetailAlbedoMap_ST.xy) + _DetailAlbedoMap_ST.zw);
          out_v.xlv_TEXCOORD0 = texcoord_8;
          out_v.gl_Position = tmpvar_6;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          float4 tmpvar_1;
          float2 mg_2;
          mg_2.x = _Metallic;
          mg_2.y = _Glossiness;
          float4 tmpvar_3;
          tmpvar_3 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy);
          float3 tmpvar_4;
          tmpvar_4 = (_Color.xyz * tmpvar_3.xyz);
          float3 res_5;
          res_5 = ((tmpvar_4 * (0.7790837 - (_Metallic * 0.7790837))) + ((lerp(float3(0.2209163, 0.2209163, 0.2209163), tmpvar_4, float3(_Metallic, _Metallic, _Metallic)) * ((1 - mg_2.y) * (1 - mg_2.y))) * 0.5));
          float4 res_6;
          res_6 = float4(0, 0, 0, 0);
          if(unity_MetaFragmentControl.x)
          {
              float4 tmpvar_7;
              tmpvar_7.w = 1;
              tmpvar_7.xyz = res_5;
              res_6.w = tmpvar_7.w;
              float3 tmpvar_8;
              float _tmp_dvx_3 = clamp(unity_OneOverOutputBoost, 0, 1);
              tmpvar_8 = clamp(pow(res_5, float3(_tmp_dvx_3, _tmp_dvx_3, _tmp_dvx_3)), float3(0, 0, 0), float3(unity_MaxOutputValue, unity_MaxOutputValue, unity_MaxOutputValue));
              res_6.xyz = tmpvar_8;
          }
          if(unity_MetaFragmentControl.y)
          {
              float3 emission_9;
              if(int(unity_UseLinearSpace))
              {
                  emission_9 = float3(0, 0, 0);
              }
              else
              {
                  emission_9 = float3(0, 0, 0);
              }
              float4 tmpvar_10;
              tmpvar_10.w = 1;
              tmpvar_10.xyz = emission_9;
              res_6 = tmpvar_10;
          }
          tmpvar_1 = res_6;
          out_f.SV_Target0 = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "VertexLit"
}
