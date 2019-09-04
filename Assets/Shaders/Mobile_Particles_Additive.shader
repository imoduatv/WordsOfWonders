Shader "Mobile/Particles/Additive"
{
  Properties
  {
    _MainTex ("Particle Texture", 2D) = "white" {}
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "PreviewType" = "Plane"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
      Fog
      { 
        Mode  Off
      } 
      Blend SrcAlpha One
      GpuProgramID 10119
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
      uniform sampler2D _MainTex;
      struct IN_Data_Vert
      {
          float4 in_POSITION :POSITION;
          float4 in_COLOR :COLOR;
          float4 in_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 tmpvar_2;
          tmpvar_2 = clamp(in_v.in_COLOR, 0, 1);
          tmpvar_1 = tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.in_POSITION.xyz;
          out_v.xlv_COLOR0 = tmpvar_1;
          out_v.xlv_TEXCOORD0 = ((in_v.in_TEXCOORD0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
          out_v.gl_Position = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          float4 tmpvar_1;
          tmpvar_1 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * in_f.xlv_COLOR0);
          out_f.SV_Target0 = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack ""
}
