Shader "Grayscale"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    _StencilComp ("Stencil Comparison", float) = 8
    _Stencil ("Stencil ID", float) = 0
    _StencilOp ("Stencil Operation", float) = 0
    _StencilWriteMask ("Stencil Write Mask", float) = 255
    _StencilReadMask ("Stencil Read Mask", float) = 255
    _ColorMask ("Color Mask", float) = 15
    _GrayscaleAmount ("Grayscale Amount", Range(0, 1)) = 1
    [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", float) = 0
  }
  SubShader
  {
    Tags
    { 
      "CanUseSpriteAtlas" = "true"
      "IGNOREPROJECTOR" = "true"
      "PreviewType" = "Plane"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
      Stencil
      { 
        Ref 0
        ReadMask 0
        WriteMask 0
        Comp Disabled
        Pass Keep
        Fail Keep
        ZFail Keep
        CompFront Disabled
        PassFront Keep
        FailFront Keep
        ZFailFront Keep
        CompBack Disabled
        PassBack Keep
        FailBack Keep
        ZFailBack Keep
      } 
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask 0
      GpuProgramID 58899
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
      uniform float4 _Color;
      uniform float4 _ClipRect;
      uniform float _GrayscaleAmount;
      uniform sampler2D _MainTex;
      struct IN_Data_Vert
      {
          float4 in_POSITION :POSITION;
          float4 in_COLOR :COLOR;
          float4 in_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR :COLOR;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 xlv_COLOR :COLOR;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1 = in_v.in_POSITION;
          float2 tmpvar_2;
          tmpvar_2 = in_v.in_TEXCOORD0.xy;
          float4 tmpvar_3;
          float2 tmpvar_4;
          float4 tmpvar_5;
          tmpvar_5.w = 1;
          tmpvar_5.xyz = tmpvar_1.xyz;
          tmpvar_4 = tmpvar_2;
          tmpvar_3 = (in_v.in_COLOR * _Color);
          out_v.gl_Position = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_5));
          out_v.xlv_COLOR = tmpvar_3;
          out_v.xlv_TEXCOORD0 = tmpvar_4;
          out_v.xlv_TEXCOORD1 = tmpvar_1;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          float4 tmpvar_1;
          float4 color_2;
          float4 tmpvar_3;
          tmpvar_3 = ((tex2D(_MainTex, in_f.xlv_TEXCOORD0) + _TextureSampleAdd) * in_f.xlv_COLOR);
          color_2 = tmpvar_3;
          float tmpvar_4;
          float2 tmpvar_5;
          if(float((_ClipRect.z>=in_f.xlv_TEXCOORD1.x)))
          {
              tmpvar_5.x = 1;
          }
          else tmpvar_5.x = 0;
          if(float((_ClipRect.w>=in_f.xlv_TEXCOORD1.y)))
          {
              tmpvar_5.y = 1;
          }
          else tmpvar_5.y = 0;
          float2 tmpvar_6;
          float _tmp_dvx_5 = greaterThanEqual(in_f.xlv_TEXCOORD1.xy, _ClipRect.xy);
          tmpvar_6 = (float2(_tmp_dvx_5, _tmp_dvx_5) * tmpvar_5);
          tmpvar_4 = (tmpvar_6.x * tmpvar_6.y);
          color_2.w = (color_2.w * tmpvar_4);
          float tmpvar_7;
          tmpvar_7 = dot(color_2.xyz, float3(0.3, 0.59, 0.11));
          float3 tmpvar_8;
          tmpvar_8 = lerp(color_2.xyz, float3(tmpvar_7, tmpvar_7, tmpvar_7), float3(_GrayscaleAmount, _GrayscaleAmount, _GrayscaleAmount));
          color_2.xyz = tmpvar_8;
          tmpvar_1 = color_2;
          out_f.SV_Target0 = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack ""
}
