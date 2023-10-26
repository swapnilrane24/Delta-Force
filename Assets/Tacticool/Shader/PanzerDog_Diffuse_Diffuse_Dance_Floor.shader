Shader "PanzerDog/Diffuse/Diffuse_Dance_Floor" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_AddTex ("Add Tex(R)", 2D) = "black" {}
		_AddTScroll ("Add Tex scroll (xy)", Vector) = (0,0,0,0)
		_AddPower ("Add power", Float) = 1
		_DetailsTex ("Details (R)", 2D) = "white" {}
		_DetailsTex2 ("Details2 (R)", 2D) = "black" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Legacy Shaders/Diffuse"
}