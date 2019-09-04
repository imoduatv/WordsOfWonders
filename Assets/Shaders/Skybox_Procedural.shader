Shader "Skybox/Procedural"
{
  Properties
  {
    [KeywordEnum(None, Simple, High Quality)] _SunDisk ("Sun", float) = 2
    _SunSize ("Sun Size", Range(0, 1)) = 0.04
    _SunSizeConvergence ("Sun Size Convergence", Range(1, 10)) = 5
    _AtmosphereThickness ("Atmosphere Thickness", Range(0, 5)) = 1
    _SkyTint ("Sky Tint", Color) = (0.5,0.5,0.5,1)
    _GroundColor ("Ground", Color) = (0.369,0.349,0.341,1)
    _Exposure ("Exposure", Range(0, 8)) = 1.3
  }
  SubShader
  {
    Tags
    { 
      "PreviewType" = "Skybox"
      "QUEUE" = "Background"
      "RenderType" = "Background"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "PreviewType" = "Skybox"
        "QUEUE" = "Background"
        "RenderType" = "Background"
      }
      ZWrite Off
      Cull Off
      GpuProgramID 20516
      // m_ProgramMask = 6
      //#ifdef _SUNDISK_NONE
      //  !!!!GLES
      CGPROGRAM
      #pragma multi_compile _SUNDISK_NONE
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
      uniform float _Exposure;
      uniform float3 _GroundColor;
      uniform float3 _SkyTint;
      uniform float _AtmosphereThickness;
      struct IN_Data_Vert
      {
          float4 in_POSITION :POSITION;
      };
      
      struct OUT_Data_Vert
      {
          float xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          float3 cOut_1;
          float3 cIn_2;
          float far_3;
          float kKr4PI_4;
          float kKrESun_5;
          float3 kSkyTintInGammaSpace_6;
          float tmpvar_7;
          float3 tmpvar_8;
          float3 tmpvar_9;
          float4 tmpvar_10;
          float4 tmpvar_11;
          tmpvar_11.w = 1;
          tmpvar_11.xyz = in_v.in_POSITION.xyz;
          tmpvar_10 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_11));
          kSkyTintInGammaSpace_6 = _SkyTint;
          float3 tmpvar_12;
          tmpvar_12 = (1 / pow(lerp(float3(0.5, 0.42, 0.325), float3(0.8, 0.72, 0.625), (float3(1, 1, 1) - kSkyTintInGammaSpace_6)), float3(4, 4, 4)));
          float tmpvar_13;
          float tmpvar_14;
          tmpvar_14 = pow(_AtmosphereThickness, 2.5);
          tmpvar_13 = (0.05 * tmpvar_14);
          kKrESun_5 = tmpvar_13;
          float tmpvar_15;
          tmpvar_15 = (0.03141593 * tmpvar_14);
          kKr4PI_4 = tmpvar_15;
          float3x3 tmpvar_16;
          tmpvar_16[0] = unity_ObjectToWorld[0].xyz;
          tmpvar_16[1] = unity_ObjectToWorld[1].xyz;
          tmpvar_16[2] = unity_ObjectToWorld[2].xyz;
          float3 tmpvar_17;
          tmpvar_17 = normalize(mul(tmpvar_16, in_v.in_POSITION.xyz));
          far_3 = 0;
          if((tmpvar_17.y>=0))
          {
              float3 frontColor_18;
              float3 samplePoint_19;
              far_3 = (sqrt(((1.050625 + (tmpvar_17.y * tmpvar_17.y)) - 1)) - tmpvar_17.y);
              float tmpvar_20;
              tmpvar_20 = (1 - (dot(tmpvar_17, float3(0, 1.0001, 0)) / 1.0001));
              float tmpvar_21;
              tmpvar_21 = (exp((-0.00287 + (tmpvar_20 * (0.459 + (tmpvar_20 * (3.83 + (tmpvar_20 * (-6.8 + (tmpvar_20 * 5.25))))))))) * 0.2460318);
              float tmpvar_22;
              tmpvar_22 = (far_3 / 2);
              float tmpvar_23;
              tmpvar_23 = (tmpvar_22 * 40.00004);
              float3 tmpvar_24;
              tmpvar_24 = (tmpvar_17 * tmpvar_22);
              float3 tmpvar_25;
              tmpvar_25 = (float3(0, 1.0001, 0) + (tmpvar_24 * 0.5));
              float tmpvar_26;
              tmpvar_26 = sqrt(dot(tmpvar_25, tmpvar_25));
              float tmpvar_27;
              tmpvar_27 = exp((160.0002 * (1 - tmpvar_26)));
              float tmpvar_28;
              tmpvar_28 = (1 - (dot(_WorldSpaceLightPos0.xyz, tmpvar_25) / tmpvar_26));
              float tmpvar_29;
              tmpvar_29 = (1 - (dot(tmpvar_17, tmpvar_25) / tmpvar_26));
          }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          float4 tmpvar_1;
          tmpvar_1.w = 1;
          float _tmp_dvx_4 = clamp(in_f.xlv_TEXCOORD0, 0, 1);
          tmpvar_1.xyz = lerp(in_f.xlv_TEXCOORD2, in_f.xlv_TEXCOORD1, float3(_tmp_dvx_4, _tmp_dvx_4, _tmp_dvx_4));
          out_f.SV_Target0 = tmpvar_1;
          return out_f;
      }
      
      
      //#endif // _SUNDISK_NONE
      ENDCG
      
    } // end phase
  }
  FallBack ""
}
