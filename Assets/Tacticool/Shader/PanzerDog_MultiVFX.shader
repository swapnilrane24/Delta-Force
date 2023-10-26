Shader "PanzerDog/MultiVFX" {
	Properties {
		[HDR] _Color ("Color", Vector) = (1,1,1,1)
		[HDR] _Color2 ("Color2", Vector) = (1,1,1,1)
		_MainTex ("Base", 2D) = "transparent" {}
		_SecondaryTex ("Secondary", 2D) = "transparent" {}
		_Direction ("Direction (XY Main, ZW Secondary)", Vector) = (0,0,0,0)
		_ColorMultiplier ("Color Multiplier", Float) = 1
		[Toggle(_ROTATION_ENABLED)] _RotationEnabled ("Rotation", Float) = 0
		_RotationSpeed ("Rotation Speed", Float) = 0
		[Toggle(_FRESNEL_ENABLED)] _FresnelEnabled ("Fresnel", Float) = 0
		_FresnelPower ("Fresnel Power", Float) = 5
		_FresnelOffset ("Fresnel Offset", Float) = 0.05
		[HDR] _FresnelColor ("Fresnel Color", Vector) = (1,1,1,1)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}