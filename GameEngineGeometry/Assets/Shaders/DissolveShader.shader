Shader "Custom Shaders/Dissolving" 
{
	Properties
	{
	  _MainTex("Texture (RGB)", 2D) = "white" {}
	  _SliceGuide("Slice Guide (RGB)", 2D) = "white" {}
	  _EmissiveEdge("Emissive Edge (RGB)", 2D) = "white" {}
	  _EdgeAmount("Edge Emission Amount", Range(0.0, 50.0)) = 1.0
	  _SliceAmount("Slice Amount", Range(0.0, 1.0)) = 0.5
	  _Amplitude("Wave Size", Range(0,1)) = 0.4
	  _Frequency("Wave Freqency", Range(1, 500)) = 2
	  _AnimationSpeed("Animation Speed", Range(0,5)) = 1
	}
	
	SubShader
	{
		Tags 
		{ 
		  "RenderType" = "Opaque" 
		}

		Cull Off
		CGPROGRAM

		#pragma surface surf Lambert addshadow vertex:vert

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_SliceGuide;
			float2 uv_EmissiveEdge;
			float _EdgeAmount;
			float _SliceAmount;
		};

		sampler2D _MainTex;
		sampler2D _SliceGuide;
		sampler2D _EmissiveEdge;
		float _EdgeAmount;
		float _SliceAmount;
		float _Amplitude;
		float _Frequency;
		float _AnimationSpeed;

		void vert(inout appdata_full data) 
		{
			float4 modifiedPos = data.vertex;
			modifiedPos.y += sin(data.vertex.x * _Frequency + _Time.y * _AnimationSpeed) * _Amplitude;

			float3 posPlusTangent = data.vertex + data.tangent * 0.01;
			posPlusTangent.y += sin(posPlusTangent.x * _Frequency + _Time.y * _AnimationSpeed) * _Amplitude;

			float3 bitangent = cross(data.normal, data.tangent);
			float3 posPlusBitangent = data.vertex + bitangent * 0.01;
			posPlusBitangent.y += sin(posPlusBitangent.x * _Frequency + _Time.y * _AnimationSpeed) * _Amplitude;

			float3 modifiedTangent = posPlusTangent - modifiedPos;
			float3 modifiedBitangent = posPlusBitangent - modifiedPos;

			float3 modifiedNormal = cross(modifiedTangent, modifiedBitangent);
			data.normal = normalize(modifiedNormal);
			data.vertex = modifiedPos;
		}

		void surf(Input IN, inout SurfaceOutput o) 
		{
			clip(tex2D(_SliceGuide, IN.uv_SliceGuide).rgb - _SliceAmount);
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
			//clip(tex2D(_EmissiveEdge, IN.uv_EmissiveEdge).rgb - (_SliceAmount + 0.1));
			o.Emission = tex2D(_EmissiveEdge, IN.uv_EmissiveEdge).rgb * _EdgeAmount * _SliceAmount;
		}
		
		ENDCG
	  }
	  Fallback "Diffuse"
}