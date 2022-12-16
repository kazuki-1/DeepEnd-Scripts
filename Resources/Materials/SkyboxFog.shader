Shader "Skybox/SkyboxFog"
{
    Properties
    {
         [NoScaleOffset] _Tex("Cubemap (HDR)", Cube) = "grey" {}
        _lerp("Lerp", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Cull off
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
                float4 vertex : SV_POSITION;
                UNITY_FOG_COORDS(1)
                float3 texcoord : TEXCOORD0;
            };

            samplerCUBE _Tex;
            float _lerp;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.vertex;
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = texCUBE(_Tex, i.texcoord);
                float4 col2 = col;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col2);
                return lerp(col, col2, _lerp);
            }
            ENDCG
        }
    }
}
