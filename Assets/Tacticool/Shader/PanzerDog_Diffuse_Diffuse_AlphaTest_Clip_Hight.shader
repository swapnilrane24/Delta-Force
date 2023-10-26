Shader "PanzerDog/Diffuse/Diffuse_AlphaTest_Clip_Hight" {
	Properties {
		_Color ("Main Color", Vector) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
		[NoScaleOffset] _ClipTex ("Clip mask (R)", 2D) = "white" {}
		_ClipRepeat ("Clip mask repeat (XY)", Vector) = (1,1,0,0)
		_ClipPower ("Clip mask power", Float) = 1
		_Hight ("Hight Clip distance", Float) = 5
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
	Fallback "Legacy Shaders/Transparent/Cutout/VertexLit"
}