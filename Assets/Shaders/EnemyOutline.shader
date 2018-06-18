Shader "Custom/EnemyOutline"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_Outline("Outline thickness", Range(0,0.1)) = 0
		_OutlineColor("Outline color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Transparent-250"}
		LOD 100

		Pass{
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
			};

			float _Outline;
			float4 _OutlineColor;

			v2f vert(appdata_base v) {
				v2f o;
				o.pos.xyz = v.vertex + v.normal.xyz * _Outline;
				o.pos = UnityObjectToClipPos(o.pos);
				return o;

				/*v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 normal = mul((float3x3) UNITY_MATRIX_MV, v.normal);
				normal.x *= UNITY_MATRIX_P[0][0];
				normal.y *= UNITY_MATRIX_P[1][1];
				o.pos.xy += normal.xy * _Outline;
				return o;*/
			}

			half4 frag(v2f i) : COLOR{
				return _OutlineColor;
			}
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 _Color;

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = _Color * tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}

	Fallback "VertexLit"
}	
