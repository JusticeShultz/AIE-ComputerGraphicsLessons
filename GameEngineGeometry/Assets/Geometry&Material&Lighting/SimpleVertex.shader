Shader "Custom/SimpleVertex" {
	Properties {
		_Amplitude ("Amplitude", Range(0,1)) = 0.0
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows addshadow vertex:vert
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float vert_Amplitude;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		static half _Frequency = 3000;
		static half _Amplitude = 4;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_base v)
		{
			v.vertex.xyz += v.normal * sin(v.vertex.x * _Frequency + _Time.y) * _Amplitude;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
