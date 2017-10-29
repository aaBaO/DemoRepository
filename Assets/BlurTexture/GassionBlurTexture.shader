Shader "ShaderPlayground/GassionBlur" {
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
			ENDCG
		}
	}

	CGINCLUDE
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
			float2 uv5:TEXCOORD5;
			float2 uv6:TEXCOORD6;
			float2 uv7:TEXCOORD7;
			float2 uv8:TEXCOORD8;
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
			o.uv5 = o.uv.xy + _BlurRadius * _GrabTexture_TexelSize * float2(0, 1);
			o.uv6 = o.uv.xy + _BlurRadius * _GrabTexture_TexelSize * float2(-1, 0);
			o.uv7 = o.uv.xy + _BlurRadius * _GrabTexture_TexelSize * float2(0, -1);
			o.uv8 = o.uv.xy + _BlurRadius * _GrabTexture_TexelSize * float2(1, 0);
			o.color = i.color;
			return o;
		}

		fixed4 frag(v2f i) : SV_Target{
			fixed4 color = fixed4(0, 0, 0, 0);
			color += 0.14 	* tex2D(_GrabTexture, i.uv);
			color += 0.125	* tex2D(_GrabTexture, i.uv5);
			color += 0.125	* tex2D(_GrabTexture, i.uv6);
			color += 0.125	* tex2D(_GrabTexture, i.uv7);
			color += 0.125	* tex2D(_GrabTexture, i.uv8);
			color += 0.09 	* tex2D(_GrabTexture, i.uv1);
			color += 0.09 	* tex2D(_GrabTexture, i.uv2);
			color += 0.09 	* tex2D(_GrabTexture, i.uv3);
			color += 0.09 	* tex2D(_GrabTexture, i.uv4);
			return color;
		}
	ENDCG
}