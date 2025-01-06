Shader "Unlit/LoadingScreenShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Progress("Progress",Range(-8,8)) = 0
        _ProgressSmooth("Progress smooth",float) = 0
        _Color("Color",COLOR) = (1,1,1,1)
        _SmoothColor("smooth Color",COLOR) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transaprent" }
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
                float4 uv1_uv2 : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;

            float _Progress;
            float _ProgressSmooth;
            float4 _Color;
            float4 _SmoothColor;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv1_uv2.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv1_uv2.zw = TRANSFORM_TEX(v.uv, _NoiseTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float distance = length(i.uv1_uv2.xy - (0.5,0.5));
                

                fixed4 maintex = tex2D(_MainTex, i.uv1_uv2.xy);
                fixed4 noisetex = tex2D(_NoiseTex, i.uv1_uv2.zw);
                //float smoothtransition = step(noisetex.r , _Progress);
                float smoothtransition = smoothstep(noisetex.r + _ProgressSmooth,noisetex.r - _ProgressSmooth,_Progress);
                float4 color = lerp(_Color,_SmoothColor,smoothtransition);
                fixed4 finalColor = maintex * color;
                finalColor.a = smoothtransition;
                
                return finalColor;
            }
            ENDCG
        }
    }
}
