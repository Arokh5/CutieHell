Shader "Custom/RoundInfo"
{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainText("Main texture", 2D) = "white" {}
 		_DissolveTex("Dissolution texture", 2D) = "gray" {}
		_BorderColor("Border Color", color) = (1,1,1,1)
		_BorderThickness("Border thickness", Range(0.,0.1)) = 0.
		_Frequency("Frequency", float) = 1.
		_Fill("Fill", Range(0., 1.)) = 0.
	}

	SubShader{

		Tags{ "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainText;

			v2f vert(appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			sampler2D _DissolveTex;
			float _Fill;
			fixed4 _Color;

			fixed4 frag(v2f i) : SV_Target{
				float4 c = tex2D(_MainText, i.uv);
				if (c.a < 0.01f)
					discard;
				c *= _Color;
				float val = tex2D(_DissolveTex, i.uv).a;

				if (val > _Fill)
					discard;

				return c;
			}
			ENDCG
		}

		Pass {

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainText;

			v2f vert(appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			sampler2D _DissolveTex;
			float _Fill;
			float _BorderThickness;
			fixed4 _BorderColor;
			float _Frequency;

			fixed4 frag(v2f i) : SV_Target{
				if (_Fill == 0.0f)
					discard;

				float4 c = tex2D(_MainText, i.uv);
				if (c.a < 0.01f)
					discard;
				c.rgb = _BorderColor.rgb;
				float val = tex2D(_DissolveTex, i.uv).a;

				float topLimit = _Fill + _BorderThickness;
				float bottomLimit = _Fill - _BorderThickness;
				if (bottomLimit < 0)
					bottomLimit = 0;
				if (val > topLimit || val < bottomLimit || val <= 0.01f)
					discard;

				float rimAlpha;

				if (val > _Fill)
					rimAlpha = (topLimit - val) / _BorderThickness + 0.05f * (sin(_Time.y * _Frequency) + 0.5f);
				else if (val < _Fill)
					rimAlpha = (val - bottomLimit) / _BorderThickness + 0.05f * (sin(_Time.y * _Frequency) + 0.5f);

				c.a = rimAlpha;

				return c;
			}
			ENDCG
		}
	}
}
