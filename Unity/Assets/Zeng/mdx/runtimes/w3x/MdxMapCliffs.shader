Shader "Zeng/MdxMapCliffs"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        u_textures("u_textures", 2DArray) = "" {}
        u_tileSize("u_tileSize", Float) = 1.0
        u_heightMap("u_heightMap", 2D) = "white" {}
        u_centerOffsetPixel("u_centerOffsetPixel", Vector) = (0,0,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float3 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;

                float4 textureIndexs:COLOR;
                

                float4 v_color:COLOR1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            uniform float u_tileSize;
            
            UNITY_DECLARE_TEX2DARRAY(u_tilesets);

            
            StructuredBuffer<float> locationBuffer;
            StructuredBuffer<int> textureBuff;


            UNITY_DECLARE_TEX2DARRAY(u_textures);

            sampler2D u_heightMap;
            uniform float4 u_centerOffsetPixel;

            v2f vert (appdata v, uint instanceID : SV_InstanceID)
            {
                v2f o;

                float sizeScale = u_tileSize / 128.0;
                float2 u_centerOffset = u_centerOffsetPixel.xy;
                float2 u_pixel = u_centerOffsetPixel.zw;

                float2 halfPixel = u_pixel * 0.5;

                float3 a_instancePosition = float3(
                    locationBuffer[instanceID * 3 + 0],
                    locationBuffer[instanceID * 3 + 1],
                    locationBuffer[instanceID * 3 + 2]
                    );

                float3 a_position = float3(v.vertex.z, -v.vertex.x, v.vertex.y);


                float2 corner = floor((a_instancePosition.xy - float2(1.0, 0.0) - u_centerOffset.xy) / 128.0);

                float bottomLeft = tex2Dlod(u_heightMap, float4(corner * u_pixel + halfPixel, 0.0, 0.0)).x;
                float bottomRight = tex2Dlod(u_heightMap, float4((corner + float2(1.0, 0.0)) * u_pixel + halfPixel, 0.0, 0.0)).x;
                float topLeft = tex2Dlod(u_heightMap, float4((corner + float2(0.0, 1.0)) * u_pixel + halfPixel, 0.0, 0.0)).x;
                float topRight = tex2Dlod(u_heightMap, float4((corner + float2(1.0, 1.0)) * u_pixel + halfPixel, 0.0, 0.0)).x;


                float bottom = lerp(bottomRight, bottomLeft, -a_position.x / 128.0);
                float top = lerp(topRight, topLeft, -a_position.x / 128.0);
                float height = lerp(bottom, top, a_position.y / 128.0);

                float3 v_position = a_position + float3(a_instancePosition.xy, a_instancePosition.z + height * 128.0);
                v_position *= sizeScale;
                v_position = float3(-v_position.y, v_position.z, v_position.x);

                //float h = v_position.y;
                //o.v_color.rgba = (h - 1.0) / 8.0;
                //o.v_color.rgba = height;


                o.vertex = UnityObjectToClipPos(float4(v_position, 1.0));
                o.uv.xy = TRANSFORM_TEX(v.uv.xy, _MainTex);
                o.uv.z = textureBuff[instanceID];
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;

            }



            fixed4 frag(v2f i) : SV_Target
            {

                //fixed4 col = tex2D(_MainTex, i.uv);

                fixed4 col = UNITY_SAMPLE_TEX2DARRAY(u_textures, float3(i.uv.xy, i.uv.z));

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
