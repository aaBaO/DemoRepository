Shader "ShaderPlayground/BlurTexture" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_BlurRadius("Raius", Range(0, 10)) = 0
	}
	SubShader{
		//UGUI的RenderQueue在Transparent
		Tags{ "Queue" = "Transparent"}

		GrabPass{}

		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			float2 _MainTex_TexelSize;
			sampler2D _GrabTexture;
			float2 _GrabTexture_TexelSize;
			float _BlurRadius;

			struct appdata {
				float4 pos:POSITION;
				float2 uv:TEXCOORD0;
				float4 color:COLOR;
			};

			struct v2f {
				float4 pos:SV_POSITION;
				float2 uv:TEXCOORD0;
				float2 uv1:TEXCOORD1;
				float2 uv2:TEXCOORD2;
				float2 uv3:TEXCOORD3;
				float2 uv4:TEXCOORD4;
				float4 color:COLOR;
			};

			v2f vert(appdata i) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, i.pos);
				o.uv = i.uv.xy;
#if UNITY_UV_STARTS_AT_TOP
				if (_GrabTexture_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
#endif
				o.uv1 = o.uv.xy + _BlurRadius * _GrabTexture_TexelSize * float2(1, 1);
				o.uv2 = o.uv.xy + _BlurRadius * _GrabTexture_TexelSize * float2(-1, 1);
				o.uv3 = o.uv.xy + _BlurRadius * _GrabTexture_TexelSize * float2(-1, -1);
				o.uv4 = o.uv.xy + _BlurRadius * _GrabTexture_TexelSize * float2(1, -1);
				o.color = i.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target{
				fixed4 color = fixed4(0, 0, 0, 0);
				color += tex2D(_GrabTexture, i.uv);
				color += tex2D(_GrabTexture, i.uv1);
				color += tex2D(_GrabTexture, i.uv2);
				color += tex2D(_GrabTexture, i.uv3);
				color += tex2D(_GrabTexture, i.uv4);
				return color * 0.2 ;
			}
			ENDCG
		}
	}
}