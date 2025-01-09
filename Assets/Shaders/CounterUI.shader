Shader "Unlit/CounterUI"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mask ("Mask", 2D) = "white" {}
        _MaskStrength ("Mask Strength", Range(0, 10)) = 1
        _Smoothness("Smoothness",float) = 0
        _Edge("Edge",float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Blend SrcAlpha OneMinusSrcAlpha
        BlendOp Add
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
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _Mask;
            float4 _Mask_ST;

            float _Smoothness;
            float _Edge;
            float _MaskStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv1 = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv, _Mask);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv1);
                fixed4 mask = tex2D(_Mask,i.uv2);

                float distance = length(i.uv2-float2(0,0));
                float edgeValue = smoothstep(0.5 -_Edge - _Smoothness,0.5 + _Edge + _Smoothness,distance);

                float maskvalue = mask.r  * _MaskStrength * edgeValue;

                return fixed4(col.rgb ,edgeValue);
            }
            ENDCG
        }
    }
}
