Shader "Custom/RoundedSprite2D"
{
    Properties
    {
        [PerRendererData]_MainTex ("Sprite", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _Radius ("Corner Radius (px)", Float) = 16
        _Softness ("Edge Softness (px)", Float) = 1

        _Border ("Border (px)", Float) = 0
        _BorderColor ("Border Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            #ifdef UNITY_UI_CLIP_RECT
            #define USE_UI 1
            #endif

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize; // x=1/w, y=1/h

            fixed4 _Color;

            float _Radius;     // px
            float _Softness;   // px
            float _Border;     // px
            fixed4 _BorderColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                fixed4 color  : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                fixed4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            // Signed distance to rounded rectangle centered at 0 with half-size b and radius r
            float sdRoundRect(float2 p, float2 b, float r)
            {
                float2 q = abs(p) - (b - r);
                return length(max(q, 0.0)) - r;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);

                // UV -> local space centered at 0,0 (range ~ -0.5..0.5)
                float2 p = i.uv - 0.5;

                // Yarı boyut (yarım genişlik/yükseklik) = 0.5 (UV alanı)
                float2 halfSize = float2(0.5, 0.5);

                // Piksel -> UV dönüşümleri
                float2 px2uv = float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y);
                float radUV = _Radius * min(px2uv.x, px2uv.y);
                float softUV = max(_Softness, 0.0) * min(px2uv.x, px2uv.y);
                float borderUV = max(_Border, 0.0) * min(px2uv.x, px2uv.y);

                // Dış şekil (tam kutu)
                float distOuter = sdRoundRect(p, halfSize, radUV);

                // Kenarlık varsa iç şekli hesapla
                float distInner = sdRoundRect(p, halfSize - borderUV, max(radUV - borderUV, 0.0));

                // Kenar yumuşatma: 1 - smoothstep(0, soft, distanceOutside)
                float alphaOuter = 1.0 - smoothstep(0.0, softUV, distOuter);
                float alphaInner = 1.0 - smoothstep(0.0, softUV, distInner);

                // Kenarlık opaklığı = dış - iç
                float borderMask = saturate(alphaOuter - alphaInner);

                // İç dolgu (iç şeklin içi)
                float fillMask = saturate(alphaInner);

                // Nihai renk: önce dolgu (texture * fill), üzerine kenarlık (borderColor * borderMask)
                fixed4 col = 0;
                col.rgb = tex.rgb * i.color.rgb * fillMask + _BorderColor.rgb * borderMask * _BorderColor.a;
                col.a   = tex.a * i.color.a * fillMask + borderMask * _BorderColor.a;

                // Dışarıyı sertçe kes (isteğe bağlı, AA zaten smoothstep ile)
                clip(col.a - 1e-3);

                return col;
            }
            ENDCG
        }
    }

    FallBack "Sprites/Default"
}
