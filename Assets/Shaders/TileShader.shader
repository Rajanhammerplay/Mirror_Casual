Shader "Unlit/TileShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Grasstex ("Grass Texture", 2D) = "white" {}
        [Toggle]NOISETEX("Noise Texture Blend", float) = 1
        _Noisetex("Noise Texture", 2D) = "white" {}
        _BlendStrength("Blend Strength",Range(0,2)) = 0
        _BlendStrength_1("Edge Factor",Range(0,2)) = 0
        _Color("Color",COLOR) = (1,1,1,1)
        _Color_1("Color_1",COLOR) = (1,1,1,1)
        _Rotationangle("rot angle",float) = 0

        _EdgeImapctMin("Edge Imapct min",Range(0,10)) = 0
        _EdgeImapctMax("Edge Imapct max",Range(0,10)) = 0 

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
            #pragma multi_compile _ NOISETEX_ON

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal: NORMAL;
            };

            struct v2f
            {
                float4 uv1_uv2 : TEXCOORD0;
                float2 uv3: TEXCOORD1;
                float4 vertex : SV_POSITION;
                float3 viewdir : TEXCOORD2;
                float3 normal : TEXCOORD3;
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
            float _BlendStrength_1;
            float _Rotationangle;

            float _EdgeImapctMin;
            float _EdgeImapctMax;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv1_uv2.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv1_uv2.zw = TRANSFORM_TEX(v.uv, _Grasstex);
                o.uv3 = TRANSFORM_TEX(v.uv,_Noisetex);
                o.viewdir = normalize(WorldSpaceViewDir(v.vertex));
                o.normal = normalize(UnityObjectToWorldNormal(v.normal));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 main = tex2D(_MainTex, i.uv1_uv2.xy);
                fixed4 Grass = tex2D(_Grasstex, i.uv1_uv2.zw);
                fixed4 noise = tex2D(_Noisetex, i.uv3);
              
                fixed4 mixed = lerp(main * _Color,Grass,noise.r * _BlendStrength);

                #ifdef NOISETEX_ON
                    i.uv1_uv2.xy -= float2(0.5,0.5);
                    float leng = max(abs(i.uv1_uv2.x), abs(i.uv1_uv2.y));
                    float edge = sin(_Time.x * 2.0 * 3.14159) * (_EdgeImapctMax - _EdgeImapctMin) * 0.5 + (_EdgeImapctMin + _EdgeImapctMax) * 0.5;
                    float edgefact = smoothstep(_BlendStrength_1+_EdgeImapctMax,_BlendStrength_1-_EdgeImapctMax,leng);
                    mixed = lerp(_Color_1,main,edgefact);
                #endif

                return float4(mixed);
            }
            ENDCG
        }
    }
}
