// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/HelloShader"
{
	Properties{
		_Tint("Tint", Color) = (1, 1, 1, 1)
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader{
		Pass{
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"

			float4 _Tint;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct VertexData {
				float4 position: POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Interpolators {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				//float3 localPosition : TEXCOORD0;
			};

			Interpolators MyVertexProgram(VertexData v) {
				Interpolators i;
				//i.localPosition = v.position.xyz;
				i.position = UnityObjectToClipPos(v.position);
				i.uv = v.uv;
				return i;
			}

			float4 MyFragmentProgram(Interpolators i) : SV_TARGET {
				//float sinColor = (sin(i.localPosition.x/5.0 * 3.0) + sin(i.localPosition.z/5.0 * 3.0));
				//return float4(sinColor, sinColor, sinColor, 1);
				return float4(sin(i.uv.x * 3.0), sin(i.uv.y * 3.0), 1, 1);
			}

			ENDCG
		} 
	}
}
