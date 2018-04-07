Shader "Custom/TextureChanger" {
	Properties{
		_Radius("Radius", Float) = 5.0
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_AlternateTex("Alternate Texture", 2D) = "white" {}
		[Normal]
		_NormalMap("Normal Map", 2D) = "bump" {}
		_NormalPower("Normal Power", Float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
			float2 uv_AlternateTex;
			float2 uv_NormalMap;
			float3 worldPos;
		};

		int _ActivePositions = 0;
		float3 _Positions[128];

		sampler2D _MainTex;
		sampler2D _AlternateTex;
		sampler2D _NormalMap;
		float _NormalPower;
		float4 _RefPos;
		float _Radius;
		float _BlendRadius;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c;
			bool found = false;
			for (int i = 0; i < _ActivePositions; ++i)
			{
				if (distance(IN.worldPos.xyz, _Positions[i]) < _Radius)
				{
					found = true;
					break;
				}
			}
			if (found)
				c = tex2D(_AlternateTex, IN.uv_AlternateTex) * _Color;
			else
				c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Normal = lerp(UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap)), fixed3(0,0,1), -_NormalPower + 1.0f);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
