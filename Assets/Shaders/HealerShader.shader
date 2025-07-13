Shader "Unlit/HealerShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MinAlpha("MinAlpha", Range(0.0, 1.0)) = 0.2
        _MaxAlpha("MaxAlpha", Range(0.0, 1.0)) = 0.8
        _PulseSpeed("Pulse Speed", Range(0.1, 5.0)) = 1.0
        [HDR]_Color("Highligh color",COLOR) = (0,0,0,0)
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        BlendOp Add
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color;
            float _MaxAlpha;
            float _MinAlpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col * _Color;
            }
            ENDCG
        }
    }
}
