Shader "Custom/DistortionPP"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        _CameraWorldPos ("Camera World Position", Vector) = (0,0,0,0)
        _Offset_r ("Red", 2D) = "white" {}
        _Offset_g ("Green", 2D) = "white" {}
        _Offset_b ("Blue", 2D) = "white" {}
        _DistortionPower ("Distortion Power", Float) = 1
        _LengthMult ("Length Mult", Float) = 1
        _Offset ("Offset", Vector) = (0,0,0,0)
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
                float2 dir : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            sampler2D _Offset_r;
            sampler2D _Offset_g;
            sampler2D _Offset_b;
            
            half _DistortionPower;
            half _LengthMult;
            float3 _CameraWorldPos;
            float4 _Offset;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                float4 cameraLocalPos = mul(unity_WorldToObject, float4(_CameraWorldPos, 1.0));
                float3 worldVertex = mul(unity_ObjectToWorld, v.vertex) + _Offset.xyz;
                float2 dir = worldVertex.xy-_CameraWorldPos.xy;
                o.dir = dir*_LengthMult;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 dist_r = tex2D(_Offset_r, i.uv).xy;
                float2 dist_g = tex2D(_Offset_g, i.uv).xy;
                float2 dist_b = tex2D(_Offset_b, i.uv).xy;
                
                half len = saturate(length(i.uv-float2(0.5, 0.5))*_LengthMult - 0.3);
                
                float2 rUV = i.uv + dist_r*_DistortionPower*sin(0.1*_Time)*0.5 * len;
                float2 gUV = i.uv + dist_g*_DistortionPower*sin(0.2*_Time+0.1)*0.5 * len;
                float2 bUV = i.uv + dist_b*_DistortionPower*sin(0.15*_Time+0.15)*0.5 * len;
                
                // maintex = colordata
                float r = tex2D(_MainTex, rUV).r;
                float g = tex2D(_MainTex, gUV).g;
                float b = tex2D(_MainTex, bUV).b;

                return float4(r, g, b, 1.0);
                //return float4(len, 0.0, 0.0, 1.0);
            }
            ENDCG
        }
    }
}
