Shader "Zeng/Mdx/sd_addivite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _TeamColor("TeamColor", Color) = (1,0,0,1)
        _filterMode("FilterMode", Float) = 3


        _vertexColor("vertexColor", Color) = (1,1,1,1)
        _geosetColor("geosetColor", Color) = (1,0,0,1)
        _layerAlpha("layerAlpha", Range(0 , 1)) = 0.54
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        LOD 100
        Blend SrcAlpha One , SrcAlpha One
        ZWrite Off

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
                fixed4 color : COLOR;
            };


            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform float4 _TeamColor;
            uniform float4 _Color;
            uniform float _filterMode;

            uniform float4 _vertexColor;
            uniform float4 _geosetColor;
            uniform float _layerAlpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

              /*  float4 ori = mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
                float4 vt = v.vertex;
                vt.y = vt.z;
                vt.z = 0;
                vt.xyz += ori.xyz;
                o.vertex = mul(UNITY_MATRIX_P, vt);*/

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = _vertexColor * _geosetColor * fixed4(1.0, 1.0, 1.0, _layerAlpha);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {


                half4 albedo = _Color;

                // sample the texture
                half4 texColor = tex2D(_MainTex, i.uv);
                half3 color = texColor.a * texColor.rgb + (1.0 - texColor.a) * _TeamColor.rgb;


                albedo.rgb *= color;
                albedo.a *= texColor.a;
                albedo *= i.color;

                // 1bit Alpha
                if (_filterMode == 1.0 && albedo.a < 0.75) {
                    discard;
                }

                // "Close to 0 alpha"
                if (_filterMode >= 5.0 && albedo.a < 0.02) {
                    discard;
                }
                //albedo.rgb = texColor.a;

                fixed4 finalColor = albedo;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                return finalColor;
            }
            ENDCG
        }
    }
}
