Shader "Unlit/LaserShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("mask Texture", 2D) = "white" {}
        _ScrollSpeed ("ScrollSpeed", vector) = (0,0,0,0)
        _Rotationangle("rotagle",vector) = (0,0,0,0)
        _TexScale("texscale",float) = 0
        [HDR]_Color("Color",COLOR) = (0,0,0,0)
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv_1 : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float4 Vertexcolor: COLOR0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _MaskTex;
            float4 _MaskTex_ST;

            float2 _ScrollSpeed;
            float2 _Rotationangle;
            float4 _Color;
            float _TexScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv_1 = TRANSFORM_TEX(v.uv, _MaskTex);
                o.Vertexcolor = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float cosAngle = cos(_Rotationangle);
                float sinAngle = sin(_Rotationangle);
                float2x2 rotationMatrix = float2x2(
                    cosAngle, -sinAngle,
                    sinAngle, cosAngle
                );
                
                i.uv = mul(rotationMatrix,i.uv);
                i.uv.xy += (_Time.xx * _ScrollSpeed.xy);
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 mask = tex2D(_MainTex, i.uv_1);
                col *= i.Vertexcolor;
                return col;
            }
            ENDCG
        }
    }
}
