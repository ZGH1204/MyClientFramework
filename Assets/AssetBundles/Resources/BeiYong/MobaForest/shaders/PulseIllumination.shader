Shader "Self-Illumin/PulseIllumination" {
 
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = ""
		_IllumTex ("Illumination (A)", 2D) = "black" { }
		_LightPower ("Light Power", float) = 5
		_PulseSpeed ("Pulse Speed", Float) = .5
		_EffectAnimX ("Blend Anim X", Float) = 1
		_EffectAnimY ("Blend Anim Y", Float) = 1
		_EffectTex ("Blend Texture", 2D) = "black" { }
		 
		_BlendPower ("Blend Power", Float) = .5

	}
 
	SubShader {
		//Tags {Queue=Transparent}
		ZWrite On
		Lighting On

		//Tags { "RenderType"="Transparent" }
		CGPROGRAM
		

		   #pragma surface surf Lambert
		   #include "UnityCG.cginc"

			sampler2D _MainTex;

			sampler2D _EffectTex;


			sampler2D _IllumTex;
			float4	_Color;
			float _LightPower;
			float _BlendPower;
			float _PulseSpeed;
			float _EffectAnimX;
			float _EffectAnimY;

			float _Shift;
			float _Delta;
			float _Tempo;

			struct Input {
				float2 uv_MainTex;
				
				float2 uv_EffectTex;
				float2 uv_IllumTex;
				float4 _Time;
			};

			void surf (Input IN, inout SurfaceOutput o) {

				float pulseWave = sin(_Time*_PulseSpeed).a / 2 + .5;

				float2 fxMove = IN.uv_EffectTex.xy;

				float2 fxMoveAnim;
				fxMoveAnim.x = _EffectAnimX * _Time;
				fxMoveAnim.y = _EffectAnimY * _Time;

				half4 fx = tex2D (_EffectTex, fxMove+fxMoveAnim);
				half4 main = tex2D (_MainTex, IN.uv_MainTex);
				half4 ilum = tex2D (_IllumTex, IN.uv_IllumTex);

				o.Albedo =  (fx.rgb * ilum * abs(_LightPower * _BlendPower)) + (pulseWave * _LightPower * ilum) + main * (_Color);
				o.Alpha = main.a;

			}
		ENDCG
	}
	FallBack "Diffuse"
}