Shader "PanzerDog/Diffuse/Diffuse_Parachute" {
	Properties {
		_Color ("Main Color", Vector) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SinAmpl ("Sin Ampl", Vector) = (0,0,0,0)
		_SinSpeed ("Sin speed", Float) = 1
		_SinFreq ("Sin frequency", Float) = 0.5
		_SinOffset ("Sin offset multiply", Float) = 1
		_OffsetXYZ ("Offset XYZ", Vector) = (0,0,0,0)
		_Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
		_CColor ("Cutoff outline Color", Vector) = (1,1,1,1)
		_Coutline ("Cutoff outline", Float) = 0
		_CoutlinePower ("Cutoff outline power", Float) = 1
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
	Fallback "Legacy Shaders/VertexLit"
}