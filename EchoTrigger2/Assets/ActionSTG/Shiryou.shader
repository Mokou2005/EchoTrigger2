Shader "Unlit/SimpleWallHackScan"
{
    Properties
    {
        _Color("Scan Color", Color) = (1, 0, 0, 1)
        _Scan("Scan Active", Float) = 0
        _ScanWidth("Ring Width", Float) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            ZTest Always // 壁越し描画

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float _Scan;
            float3 _ScanPosition; // スキャン中心（ワールド）
            float _ScanRadius;    // 波紋の半径
            float _ScanWidth;     // リングの太さ

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                if (_Scan < 0.5) return 0;

                float dist = distance(i.worldPos, _ScanPosition);
                float ring = 1.0 - abs(dist - _ScanRadius) / _ScanWidth;
                ring = saturate(ring);

                return fixed4(_Color.rgb, ring * _Color.a);
            }
            ENDCG
        }
    }
}
