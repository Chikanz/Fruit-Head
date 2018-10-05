// Toony Colors Pro+Mobile 2
// (c) 2014,2015 Jean Moreno

Shader "Toony Colors Pro 2/User/ToonyGlass"
{
	Properties
	{
		//TOONY COLORS
		_Color ("Color", Color) = (0.5,0.5,0.5,1.0)
		_HColor ("Highlight Color", Color) = (0.6,0.6,0.6,1.0)
		_SColor ("Shadow Color", Color) = (0.3,0.3,0.3,1.0)
		
		//DIFFUSE
		_MainTex ("Main Texture (RGB)", 2D) = "white" {}
		
		//TOONY COLORS RAMP
		_RampThreshold ("#RAMPF# Ramp Threshold", Range(0,1)) = 0.5
		_RampSmooth ("#RAMPF# Ramp Smoothing", Range(0.001,1)) = 0.1
		
		//SPECULAR
		_SpecColor ("#SPEC# Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("#SPEC# Shininess", Range(0.0,2)) = 0.1
		_SpecSmooth ("#SPECT# Smoothness", Range(0,1)) = 0.05
		
		//Blending
		_SrcBlend ("#ALPHA# Blending Source", Float) = 5
		_DstBlend ("#ALPHA# Blending Dest", Float) = 10
		
	}
	
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend [_SrcBlend] [_DstBlend]
		
		CGPROGRAM
		
		#include "../../Shaders 2.0/Include/TCP2_Include.cginc"
		#pragma surface surf ToonyColorsSpec 
		#pragma target 3.0
		#pragma glsl
		
		#pragma multi_compile TCP2_SPEC_TOON
		
		//================================================================
		// VARIABLES
		
		fixed4 _Color;
		sampler2D _MainTex;
		
		fixed _Shininess;
		
		struct Input
		{
			half2 uv_MainTex;
		};
		
		//================================================================
		// SURFACE FUNCTION
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
			
			o.Albedo = mainTex.rgb * _Color.rgb;
			o.Alpha = mainTex.a * _Color.a;
			
			//Specular
			o.Gloss = 1;
			o.Specular = _Shininess;
		}
		
		ENDCG
	}
	
	Fallback "Diffuse"
	CustomEditor "TCP2_MaterialInspector"
}
