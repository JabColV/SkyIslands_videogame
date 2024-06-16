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





// //Tipo de shader -> Unlit, nombre del shader -> Water
// Shader "Unlit/Water"
// {
//     //Propiedades del shader -> Textura
//     Properties
//     {
//         //These a re like public variables
//         _MainTex ("Albedo Texture", 2D) = "white" {}
//         _TintColor ("Tint Color", Color) = (0.2,0.4,0.7,0.8) 
//         _Transparency("Transparency", Range(0.0,1.0)) = 0.8
//         _CutoutThresh("Cutout Threshold", Range(0.0,1.0)) = 0.2
//         _Distance("Distance", Float) = 1
//         _Amplitude("Amplitude", Float) = 1
//         _Speed("Speed", Float) = 1
//         _Amount("Amount", Range(0.0,1.0)) = 1
//     }
//     //Subshader block -> Actual code, we can have multiple subshaders
//     SubShader
//     {
//         Tags { "Queue"="Transparent" "RenderType"="Transparent" }
//         LOD 100 //Level of detail

//         ZWrite Off //Does not write to the depth buffer
//         Blend SrcAlpha OneMinusSrcAlpha //Blends the colors of the object

//         Pass
//         {
//             CGPROGRAM
//             #pragma vertex vert //Function declaration
//             #pragma fragment frag //Function declaration

//             #include "UnityCG.cginc" //Added to use helper functions from Unity

//             //Structs
//             //Use to pass information about the vertices, 3D models
//             struct appdata
//             {
//                 float4 vertex : POSITION; //variable to store the position of the vertex
//                 float2 uv : TEXCOORD0;
//             };

//             struct v2f
//             {
//                 float2 uv : TEXCOORD0;
//                 float4 vertex : SV_POSITION; //position in screen space
//                 float4 worldPos : TEXCOORD1; //world position for more complex effects
//             };

//             sampler2D _MainTex;
//             float4 _MainTex_ST;
//             float4 _TintColor;
//             float _Transparency;
//             float _CutoutThresh;
//             float _Distance;
//             float _Amplitude;
//             float _Speed;
//             float _Amount;

//             //Vertex function, modifies the shape of the model
//             v2f vert (appdata v)
//             {
//                 v2f o; //Output struct
//                 float wave = sin(_Time.y * _Speed + v.vertex.x * _Amplitude) * _Distance * _Amount; //Wave calculation
//                 v.vertex.y += wave; //Modifies the y position of the vertex
//                 o.vertex = UnityObjectToClipPos(v.vertex); //Transforms the vertex position from object space to clip space 
//                 o.uv = TRANSFORM_TEX(v.uv, _MainTex); //Transforms the texture coordinates from 0-1 to the texture size
//                 o.worldPos = mul(unity_ObjectToWorld, v.vertex); //Get the world position
//                 return o; //Returns the output struct
//             }

//             //Fragment function, receives what was done in the vertex function and paints the pixels
//             fixed4 frag (v2f i) : SV_Target
//             {
//                 // Sample the texture
//                 fixed4 col = tex2D(_MainTex, i.uv) * _TintColor; //Apply tint color
//                 col.a *= _Transparency; //Apply transparency
//                 clip(col.a - _CutoutThresh); //Cuts out the pixels that are below the threshold


//                 return col;
//             }
//             ENDCG
//         }
//     }
//     FallBack "Diffuse"
// }


                // Apply simple lighting
                // float3 lightDir = normalize(float3(0.3, 0.5, 0.7)); //Directional light direction
                // float diff = saturate(dot(normalize(i.worldPos.xyz), lightDir)); //Diffuse lighting calculation
                // col.rgb *= diff; //Apply lighting to the color



// //Tiipo de shader -> Unlit, nombre del shader -> Water
// Shader "Unlit/Water"
// {
//     //Propiedades del shader -> Textura
//     //Here we tell Unity how we are going to render things
//     Properties
//     {
        // //These a re like public variables
        // _MainTex ("Albedo Texture", 2D) = "white" {}
        // _TintColor ("Tint Color", Color) = (1,1,1,1) 
        // _Transparency("Transparency", Range(0.0,1.0)) = 0.8
        // _CutoutThresh("Cutout Threshold", Range(0.0,1.0)) = 0.2
        // _Distance("Distance", Float) = 1
        // _Amplitude("Amplitude", Float) = 1
        // _Speed("Speed", Float) = 1
        // _Amount("Amount", Range(0.0,1.0)) = 1
//     }
//     //Subshader block -> Actual code, we can have multiple subshaders
//     SubShader
//     {
//         Tags { "Queue"="Transparent" "RenderType"="Transparent" }
//         LOD 100 //Level of detail

//         ZWrite Off //Does not write to the depth buffer
//         Blend SrcAlpha OneMinusSrcAlpha //Blends the colors of the object

//         Pass
//         {
//             CGPROGRAM
//             #pragma vertex vert //Function declaration
//             #pragma fragment frag //Function declaration

//             #include "UnityCG.cginc" //Added to use helper functions from Unity

//             //Structs
//             //Use to pass information about the verices, 3D models
//             struct appdata
//             {
//                 float4 vertex : POSITION; //variable to store the position of the vertex
//                 float2 uv : TEXCOORD0;
//             };

//             struct v2f
//             {
//                 float2 uv : TEXCOORD0;
//                 float4 vertex : SV_POSITION; //position in screen space
//             };

//             sampler2D _MainTex;
//             float4 _MainTex_ST;
//             float4 _TintColor;
//             float _Transparency;
//             float _CutoutThresh;
//             float _Distance;
//             float _Amplitude;
//             float _Speed;
//             float _Amount;

//             //Vertex function, modifies the shape of the model
//             v2f vert (appdata v)
//             {
//                 v2f o; //Output struct
//                 v.vertex.x += sin(_Time.y * _Speed + v.vertex.y * _Amplitude) * _Distance * _Amount; //Modifies the x position of the vertex
//                 o.vertex = UnityObjectToClipPos(v.vertex); //Transforms the vertex position from object space to clip space 
//                 o.uv = TRANSFORM_TEX(v.uv, _MainTex); //Transforms the texture coordinates from 0-1 to the texture size
//                 return o; //Returns the output struct
//             }

//             //Fragment function, recieves what was done in the vertex function and paints the pixels
//             fixed4 frag (v2f i) : SV_Target
//             {
//                 // sample the texture
//                 fixed4 col = tex2D(_MainTex, i.uv) + _TintColor; //
//                 col.a = _Transparency; //Transparency
//                 clip(col.r - _CutoutThresh); //Cuts out the pixels that are below the threshold
//                 return col;
//             }
//             ENDCG
//         }
//     }
// }
