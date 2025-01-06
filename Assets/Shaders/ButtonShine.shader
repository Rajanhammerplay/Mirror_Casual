Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Shinetex ("Shine Texture", 2D) = "white" {}
        _ShineSpeed ("Shine Speed",float) = 0
        [HDR]_ShineColor("Shine Color",COLOR) = (1,1,1,1)
        _Shineaffectingarea("shine treshhold",float) = 0
        [HDR]_CornerShineColor("Shine Color",COLOR) = (1,1,1,1)
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

            sampler2D _Shinetex;
            float4 _Shinetex_ST;

            float _ShineSpeed;
            float4 _ShineColor;
            float4 _CornerShineColor;
            float _Shineaffectingarea;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv1 = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv, _Shinetex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 button = tex2D(_MainTex, i.uv1);
                float2 center = (0.5,0.5);
                float distancefromcenter = length(i.uv1-center);
                fixed4 color = lerp(_ShineColor,_CornerShineColor,distancefromcenter * _Shineaffectingarea);
                if(distancefromcenter > _Shineaffectingarea)
                {
                    return button;
                }
                fixed4 shine = tex2D(_Shinetex, i.uv2 + float2(0, _Time.x * _ShineSpeed));
                shine.a *= button.a;
                fixed4 shinewithcolor = shine * color;
                fixed4 mixed = button + shinewithcolor * shine.a;
                return mixed;
            }
            ENDCG
        }
    }
}
