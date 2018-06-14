Shader "Custom/TextureChanger" {
	Properties{
		_Color ("Color", Color) = (1,1,1,1)
		[Header(Default Textures)]
		_MainTex ("Main Texture", 2D) = "white" {}
		[Normal]
		_NormalMap("Normal Map", 2D) = "bump" {}
		_NormalPower("Normal Power", Float) = 1.0
		_RoughnessMap("Roughness Map", 2D) = "white" {}
		[Header(Alternate Texture)]
		_AlternateTex("Alternate Texture", 2D) = "white" {}
		[Normal]
		_AlternateNormalMap("Alternate Normal Map", 2D) = "bump" {}
		_AlternateNormalPower("Alternate Normal Power", Float) = 1.0
		_AlternateRoughnessMap("Alternate Roughness Map", 2D) = "white" {}

	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		// The next 3 variables are assign from the TextureChangerSource C# script
		int _ActiveElements;
		float4 _Elements[128];
		float _ElementsBlendStartRadius[128];

		fixed4 _Color;
		float _BlendRadius;

		sampler2D _MainTex;
		sampler2D _NormalMap;
		float _NormalPower;
		sampler2D _RoughnessMap;

		sampler2D _AlternateTex;
		sampler2D _AlternateNormalMap;
		float _AlternateNormalPower;
		sampler2D _AlternateRoughnessMap;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
			float2 uv_RoughnessMap;
			float2 uv_AlternateTex;
			float2 uv_AlternateNormalMap;
			float2 uv_AlternateRoughnessMap;

			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float finalLerpFactor = 1;
			bool inEffectRadius = false;

			for (int i = 0; i < _ActiveElements; ++i)
			{
				float distanceToEnemy = distance(IN.worldPos.xyz, _Elements[i].xyz);
				if (distanceToEnemy < _Elements[i].w)
				{
					inEffectRadius = true;
					float inPct = distanceToEnemy / _Elements[i].w;
					float blendRadius = _ElementsBlendStartRadius[i];
					float lerpFactor = (inPct - blendRadius) / (1 - blendRadius);
					if (lerpFactor < finalLerpFactor)
						finalLerpFactor = lerpFactor;
					if (finalLerpFactor <= 0)
					{
						finalLerpFactor = 0;
						break;
					}
				}
			}

			fixed4 color;
			fixed4 normal;
			fixed4 roughness;
			if (inEffectRadius)
			{
				if (finalLerpFactor <= 0)
				{
					color = tex2D(_AlternateTex, IN.uv_AlternateTex) * _Color;
					normal = tex2D(_AlternateNormalMap, IN.uv_AlternateNormalMap);
					roughness = tex2D(_AlternateRoughnessMap, IN.uv_AlternateRoughnessMap);
				}
				else
				{
					color = lerp(tex2D(_AlternateTex, IN.uv_AlternateTex), tex2D(_MainTex, IN.uv_MainTex), finalLerpFactor) * _Color;
					normal = lerp(tex2D(_AlternateNormalMap, IN.uv_AlternateNormalMap), tex2D(_NormalMap, IN.uv_NormalMap), finalLerpFactor);
					roughness = lerp(tex2D(_AlternateRoughnessMap, IN.uv_AlternateRoughnessMap), tex2D(_RoughnessMap, IN.uv_RoughnessMap), finalLerpFactor);
				}
			}
			else
			{
				color = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				normal = tex2D(_NormalMap, IN.uv_NormalMap);
				roughness = tex2D(_RoughnessMap, IN.uv_RoughnessMap);
			}

			o.Albedo = color.rgb;
			o.Alpha = color.a;
			o.Normal = lerp(UnpackNormal(normal), fixed3(0,0,1), -_NormalPower + 1.0f);
			o.Smoothness = float4(1,1,1,1) - roughness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
