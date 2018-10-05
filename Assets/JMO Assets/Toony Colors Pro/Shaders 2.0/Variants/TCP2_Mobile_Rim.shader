P2_RAMPTEXT
		#pragma shader_feature TCP2_BUMP
		#pragma shader_feature TCP2_RIMDIR
		#pragma shader_feature TCP2_MC TCP2_MCMASK
		
		//================================================================
		// VARIABLES
		
		fixed4 _Color;
		sampler2D _MainTex;
	#if TCP2_MC || TCP2_MCMASK
		sampler2D _MatCap;
	#endif
		
	#if TCP2_BUMP
		sampler2D _BumpMap;
	#endif
		fixed4 _RimColor;
		fixed _RimMin;
		fixed _RimMax;
		#if TCP2_RIMDIR
		float4 _RimDir;
		#endif
		
		struct Input
		{
			half2 uv_MainTex : TEXCOORD0;
	#if TCP2_BUMP
			half2 uv_BumpMap : TEXCOORD1;
	#endif
			fixed rim;
	#if TCP2_MC || TCP2_MCMASK
			half2 matcap;
	#endif
		};
		
		//================================================================
		// VERTEX FUNCTION
		
		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
		#if TCP2_RIMDIR
			_RimDir.x += UNITY_MATRIX_MV[0][3] * (1/UNITY_MATRIX_MV[2][3]) * (1-UNITY_MATRIX_P[3][3]);
			_RimDir.y += UNITY_MATRIX_MV[1][3] * (1/UNITY_MATRIX_MV[2][3]) * (1-UNITY_MATRIX_P[3][3]);
			float3 viewDir = normalize(UNITY_MATRIX_V[0].xyz * _RimDir.x + UNITY_MATRIX_V[1].xyz * _RimDir.y + UNITY_MATRIX_V[2].xyz * _RimDir.z);
		#else
			float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
		#endif
			half rim = 1.0f - saturate( dot(viewDir, v.normal) );
			o.rim = smoothstep(_RimMin, _RimMax, rim) * _RimColor.a;
			
	#if TCP2_MC || TCP2_MCMASK
			//MATCAP
			float3 worldNorm = normalize(_World2Object[0].xyz * v.normal.x + _World2Object[1].xyz * v.normal.y + _World2Object[2].xyz * v.normal.z);
			worldNorm = mul((float3x3)UNITY_MATRIX_V, worldNorm);
			o.matcap.xy = worldNorm.xy * 0.5 + 0.5;
	#endif
		}
		
		//================================================================
		// SURFACE FUNCTION
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 main = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = main.rgb * _Color.rgb;
			o.Alpha = main.a * _Color.a;
			
	#if TCP2_BUMP
			//Normal map
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	#endif
			o.Albedo = lerp(o.Albedo.rgb, _RimColor.rgb, IN.rim);
	#if TCP2_MC || TCP2_MCMASK
			fixed3 matcap = tex2D(_MatCap, IN.matcap).rgb;
		#if TCP2_MCMASK
			matcap *= main.a * _HColor.a;
		#endif
			o.Emission += matcap;
	#endif
		}
		
		ENDCG
		
		//Outlines
		UsePass "Hidden/Toony Colors Pro 2/Outline Only (Shader Model 2)/OUTLINE"
	}
	
	Fallback "Diffuse"
	CustomEditor "TCP2_MaterialInspector"
}
// Toony Colors Pro+Mobile 2
// (c) 2014-2016 Jean Moreno


Shader "Hidden/Toony Colors Pro 2/Variants/Mobile Matcap RimOutline OutlineBlending"
{
	Properties
	{
		//TOONY COLORS
		_Color ("Color", Color) = (1.0,1.0,1.0,1.0)
		_HColor ("Highlight Color", Color) = (0.6,0.6,0.6,1.0)
		_SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1.0)
		
		//DIFFUSE
		_MainTex ("Main Text