Shader "Unlit/TileShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Grasstex ("Grass Texture", 2D) = "white" {}
        _Noisetex("Noise Texture", 2D) = "white" {}
        _BlendStrength("Blend Strength",Range(0,2)) = 0
        _Color("Color",COLOR) = (1,1,1,1)
        _Color_1("Color_1",COLOR) = (1,1,1,1)
        _Rotationangle("rot angle",float) = 0
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
                float4 uv1_uv2 : TEXCOORD0;
                float2 uv3: TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _Grasstex;
            float4 _Grasstex_ST;

            sampler2D _Noisetex;
            float4 _Noisetex_ST;

            float4 _Color;
            float4 _Color_1;

            float _BlendStrength;
            float _Rotationangle;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv1_uv2.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv1_uv2.zw = TRANSFORM_TEX(v.uv, _Grasstex);
                o.uv3 = TRANSFORM_TEX(v.uv,_Noisetex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 main = tex2D(_MainTex, i.uv1_uv2.xy);
                fixed4 Grass = tex2D(_Grasstex, i.uv1_uv2.zw);
                fixed4 noise = tex2D(_Noisetex, i.uv3);

                fixed4 mixed = lerp(main * _Color,Grass,noise.r * _BlendStrength);
                return mixed ;
            }
            ENDCG
        }
    }
}
