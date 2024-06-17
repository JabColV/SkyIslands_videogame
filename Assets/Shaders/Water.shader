Shader "Unlit/Water"
{
    Properties
    {
        _MainTex ("Albedo Texture", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (0.2, 0.4, 0.7, 0.8)
        _Transparency("Transparency", Range(0.0, 1.0)) = 0.8
        _CutoutThresh("Cutout Threshold", Range(0.0, 1.0)) = 0.2
        _WaveScale ("Wave Scale", Float) = 0.1
        _WaveSpeed ("Wave Speed", Float) = 1.0
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        // Render both sides of the polygons
        Cull Off

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _TintColor;
            float _Transparency;
            float _CutoutThresh;
            float _WaveScale;
            float _WaveSpeed;

            v2f vert(appdata v)
            {
                v2f o;
                float2 wave = v.uv;
                wave.y += sin(_Time.y * _WaveSpeed + wave.x * 2.0) * _WaveScale;
                wave.x += cos(_Time.y * _WaveSpeed + wave.y * 2.0) * _WaveScale;
                o.uv = wave;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _TintColor;
                col.a *= _Transparency;
                clip(col.a - _CutoutThresh);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}





