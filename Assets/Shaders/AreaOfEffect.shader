Shader "Custom/Area Of Effect" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_AlternateStartRadius("Alternate Start", Range(0,1)) = 0.0
		_ConquerFactor("Conquer factor", Range(0,1)) = 0.0
		[Toggle(CONQUERED)]
		_Conquered("Conquered", Float) = 0
		_InitialTex("Initial Texture", 2D) = "white" {}
		_AlternateTex("Alternate Texture", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _InitialTex;
		sampler2D _AlternateTex;

		struct Input {
			float2 uv_InitialTex;
			float2 uv_AlternateTex;
		};

		half _AlternateStartRadius;
		half _ConquerFactor;
		half _Conquered;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c;
			if (_Conquered == 0)
			{
				float2 centeredUVs = IN.uv_InitialTex - float2(0.5, 0.5);
				float radiusModifier = sin(atan2(centeredUVs.y, centeredUVs.x) * 8 + 0.5 *  _Time.w) * 0.04;
				float startRadius = _AlternateStartRadius / 2;
				startRadius += radiusModifier * startRadius;
				float radius = _ConquerFactor / 2;
				radius += radiusModifier * radius;
				float texRadius = sqrt(centeredUVs.x * centeredUVs.x + centeredUVs.y * centeredUVs.y);
				if (texRadius < startRadius)
				{
					c = tex2D(_AlternateTex, IN.uv_AlternateTex);
				}
				else if (texRadius < radius)
				{
					float progress = (texRadius - startRadius) / (radius - startRadius);
					c = lerp(tex2D(_AlternateTex, IN.uv_AlternateTex), tex2D(_InitialTex, IN.uv_InitialTex), progress);
				}
				else
				{
					c = tex2D(_InitialTex, IN.uv_InitialTex);
					//c = lerp(tex2D(_MainTex, IN.uv_MainTex), tex2D(_MainTex2, IN.uv_MainTex2), _ConquerFactor) * _Color;
				}
			}
			else
			{
				c = tex2D(_AlternateTex, IN.uv_AlternateTex);
			}
			
			if (c.a == 0) discard;

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

		}
		ENDCG
	}
	FallBack "Diffuse"
}
