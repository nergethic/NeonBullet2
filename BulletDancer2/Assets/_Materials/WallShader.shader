// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/NewUnlitShader"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        _CameraWorldPos ("Camera World Position", Vector) = (0,0,0,0)
        _Hide ("Hide", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent"
             "IgnoreProjector"="True"}
        Cull Off
        Zwrite On
        ZTest Always
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 objectVert : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _CameraWorldPos;
            float _Hide;

            v2f vert (appdata v) {
                v2f o;
                o.objectVert = float4(v.normal.x, v.normal.y, v.normal.z, 0.0); // v.vertex.x, (v.vertex.z+5.0)/5.0
                float4 cameraLocalPos = mul(unity_WorldToObject, float4(_CameraWorldPos, 1.0));
                float3 worldVertex = mul(unity_ObjectToWorld, v.vertex);
                
                //float2 dir = normalize(worldVertex.xz-_CameraWorldPos.xz);
                float2 dir = normalize(worldVertex.xy-_CameraWorldPos.xy);
                //float mult = lerp(0.03, 1000.0, worldVertex.y+0.00005);
                float mult = lerp(0.03, 1000.0, -worldVertex.z+0.00004);
                worldVertex.x += mult*dir.x;
                worldVertex.y += mult*dir.y;
                
                // float3 newPos = mul(unity_WorldToObject, float4(worldVertex.x, 0.01, worldVertex.z, 1.0)).xyz;
                float4 newPos = mul(unity_WorldToObject, float4(worldVertex.x, worldVertex.y, -0.01, 1.0)); // -0.01
                
                float4 clip1 = UnityObjectToClipPos(newPos);
                float4 clip2 = UnityObjectToClipPos(float4(v.vertex.xyz, 1.0));
                o.vertex = lerp(clip1, clip2, _Hide);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                half col = -worldVertex.z;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                //fixed4 col = tex2D(_MainTex, i.uv);
                //return col;
                // return fixed4(i.objectVert.r, i.objectVert.g, i.objectVert.b, 0);
                return fixed4(0.00, 0.00, 0.00, 0); // i.objectVert.y
            }
            ENDCG
        }
    }
}
