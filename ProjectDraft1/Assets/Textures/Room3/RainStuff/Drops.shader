// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Doctrina/Drops" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Albedo ("Albedo A - roughness (RGBA)", 2D) = "white" {}
		_Drops ("Drops (RGB)", 2D) = "white" {}
		_Drops2 ("Drops2 (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_Amount ("Inversed amount", Range(0,1) ) = 0

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _Albedo;
		sampler2D _Drops;
		sampler2D _Drops2;
		sampler2D _BumpMap;

		struct Input {
		    float2 uv_Albedo;
			float2 uv_Drops;
			float2 uv_Drops2;
			float2 uv_BumpMap;
		};

		half _Glossiness;
		half _Metallic;
		half _Amount;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		inline fixed3 combineNormalMaps (fixed3 base, fixed3 detail) {
     		base += fixed3(0, 0, 1);
     		detail *= fixed3(-1, -1, 1);
     		return base * dot(base, detail) / base.z - detail;
   		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
					
		    float3 c = tex2D(_Drops, IN.uv_Drops);
		    float3 c2 = tex2D(_Drops2, IN.uv_Drops2);
		   
            half r = saturate( 1 - c.r - frac(_Time.z + c.g ) );
            half r2 = saturate( 1 - c2.r - frac(_Time.z + c2.g ) );
                       
            half d1 = saturate( sin( r * 15 ) * c.r );
            half d2 = saturate( sin( r2 * 15 ) * c2.r ); 

            float d = d1 * saturate(c.b - _Amount) + d2 * saturate(c2.b - _Amount);
           
            float3 nrm = float3( ddx(d), ddy(d), 1 ); 

            float3 nmp = UnpackNormal( tex2D( _BumpMap, IN.uv_BumpMap-_Time.x*3));

            o.Normal = combineNormalMaps(nmp, nrm);

            float4 alb = tex2D(_Albedo, IN.uv_Albedo );

            o.Albedo = alb * _Color;			
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness * alb.a;

		}
		ENDCG
	}
	FallBack "Diffuse"
}
