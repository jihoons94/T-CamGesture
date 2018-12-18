Shader "TCam/Texture"
{
	Properties 
	{
		_Tex ("RGB or RGBA texture", 2D) = "black" {}

//	    _YTex ("Y channel texture", 2D) = "black" {}
//	    _UVTex ("UV channel texture", 2D) = "black" {}

	    _YTex ("Y channel texture", 2D) = "black" {}
	    _UTex ("U channel texture", 2D) = "black" {}
	    _VTex ("V channel texture", 2D) = "black" {}

	    _Format ("Format", Int) = 1
	}

	SubShader 
	{
	    ZWrite Off
	    ZTest Off
	    Tags { "Queue" = "Background" }
	    Pass 
	    {
	        CGPROGRAM

	        #pragma vertex vert
	        #pragma fragment frag

	        sampler2D _Tex;

//	        sampler2D _YTex;
//	        sampler2D _UVTex;

	        sampler2D _YTex;
	        sampler2D _UTex;
   	        sampler2D _VTex;

//        	NV21 = 1,
//			NV12 = 2,
//			RGB888 = 3,
//			GRAYSCALE = 4,
//			RGBA = 5
   	        int _Format;

	        struct vertexInput
	        {
	            float4 vertex : POSITION;
	            float2 uv : TEXCOORD0;
	        };

	        struct vertexOutput
	        {
	            float4 vertex : SV_POSITION;
	            float2 uv : TEXCOORD0;
	        };
	        
	        vertexOutput vert (vertexInput input)
	        {
	            vertexOutput output;
	            output.vertex = UnityObjectToClipPos(input.vertex);
	            output.uv = input.uv;
	            return output;
	        }

			float4 frag (vertexOutput i) : COLOR
	        {
	        	// float: 32bit, half: 16bit, fixed: 11bit

				float r, g, b;

				// NV21, NV12
				if (_Format == 1 || _Format == 2) {
					float y = tex2D(_YTex, i.uv).a;
		            float u = tex2D(_UTex, i.uv).a - 0.5;
		            float v = tex2D(_VTex, i.uv).a - 0.5;

		            r = y + 1.13983*v;
		            g = y - 0.39465*u - 0.58060*v;
		            b = y + 2.03211*u;
		        // RGB888
				} else if (_Format == 3) {
					return float4(tex2D(_Tex, i.uv).rgb, 1.0);
				// GrayScale
				} else if (_Format == 4) {
					float y = tex2D(_YTex, i.uv).a;
					return float4(y, y, y, 1.0);
				// RGBA
				} else if (_Format == 5) {
					return tex2D(_Tex, i.uv);
				}

	            return float4(r, g, b, 1.0);
	        }

	        ENDCG
	    }
	}
}