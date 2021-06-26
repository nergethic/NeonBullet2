Shader "Custom/PixelizePP"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        _Height ("Height", Float) = 0.1
        _Color ("Scanline Color", Color) = (0.3, 0.3, 0.3, 0.5)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Height;
            float4 _Color;
            
            float2 pixelArt(float2 uv, const float pixelSample) {
                half pixel = 1 / pixelSample;
                half2 pixelUV = half2((int)(uv.x / pixel) * pixel, (int)(uv.y / pixel) * pixel);
                return pixelUV;
            }

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                half3 mainTex = tex2D(_MainTex, pixelArt(i.uv, 300));
                float scanline = sin(i.uv.y * _Height) * 0.5 + 0.5;
                mainTex = lerp(mainTex, mainTex*_Color.rgb, _Color.a * scanline);
                return half4(mainTex, 1);
            }
            ENDCG
        }
    }
}
