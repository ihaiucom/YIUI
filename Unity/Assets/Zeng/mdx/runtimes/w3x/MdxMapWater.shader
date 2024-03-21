Shader "Zeng/MdxMapWater"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        u_heightMap ("u_heightMap", 2D) = "white" {}
        u_waterHeightMap("u_waterHeightMap", 2D) = "white" {}
        u_offsetHeight("u_offsetHeight", Float) = 1.0

        u_tileSize("u_tileSize", Float) = 1.0
        u_tilesets ("u_tilesets", 2DArray) = "" {}
        u_tilesetsIndex("u_tilesetsIndex", Int) = 0

        u_minDepth("u_minDepth", Float) = 0.078125 // 10 / 128
        u_deepLevel("u_deepLevel", Float) = 0.5    // 64 / 128
        u_maxDepth("u_maxDepth", Float) = 0.5625   // 72 / 128

        u_minDeepColor("u_minDeepColor", COLOR) = (0.0, 0.0, 1.0, 0.2)
        u_maxDeepColor("u_maxDeepColor", COLOR) = (0.0, 0.0, 1.0, 1.0)
        u_minShallowColor("u_minShallowColor", COLOR) = (0.0, 1.0, 1.0, 0.2)
        u_maxShallowColor("u_maxShallowColor", COLOR) = (0.0, 1.0, 1.0, 1.0)
            
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
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

                float4 textureIndexs:COLOR;
                
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float2 uv3 : TEXCOORD3;
                float2 uv4 : TEXCOORD4;

                float4 v_color:COLOR1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D u_heightMap;
            sampler2D u_waterHeightMap;
            uniform float u_offsetHeight;
            
            uniform float u_tileSize;
            uniform float u_size[2];
            uniform float u_offset[2];


            uniform float u_minDepth;
            uniform float u_deepLevel;
            uniform float u_maxDepth;

            uniform float4 u_minDeepColor;
            uniform float4 u_maxDeepColor;
            uniform float4 u_minShallowColor;
            uniform float4 u_maxShallowColor;
            
            UNITY_DECLARE_TEX2DARRAY(u_tilesets);
            uniform int u_tilesetsIndex;
            
            
            StructuredBuffer<float> waterFlagsBuffer;


            //const float minDepth = 10.0 / 128.0;
            //const float deepLevel = 64.0 / 128.0;
            //const float maxDepth = 72.0 / 128.0;

            float2 getCell(float variation)
            {
                if (variation < 16.0) {
                    return float2(fmod(variation, 4.0), floor(variation / 4.0));
                }
                else {
                    variation -= 16.0;
                    return float2(4.0 + fmod(variation, 4.0), floor(variation / 4.0));
                }
            }

            float2 getUV(float2 position, float cellSzieX, float variation)
            {
                float2 cell = getCell(variation);
                float2 cellSize = float2(cellSzieX, 0.25);
                float2 uv = float2(position.x, 1.0 - position.y);
                float2 pixelSize = float2(1.0 / 512.0, 1.0 / 256.0);
                float2 r =  clamp(
                    (cell + uv) * cellSize,
                    cell * cellSize + pixelSize,
                    (cell + 1.0) * cellSize - pixelSize
                );
                r.y = 1.0 - r.y;
                return r;
            }
            
            v2f vert (appdata v, uint instanceID : SV_InstanceID)
            {
                v2f o;

                float2 a_position = v.vertex.xz;
                float vertIndex = v.vertex.y;


                
                float columns = u_size[0];
                float rows = u_size[1];
                float2 size = float2(u_size[0] , u_size[1]);

                float2 c = float2(fmod(instanceID, columns), floor(instanceID / (columns)));
                int i = int(c.y * (columns - 1) + c.x);

                float a_isWater = waterFlagsBuffer[i];

                float2 u_pixel = 1.0 / size;
                float2 halfPixel = u_pixel * 0.5;




                float2 corner = float2(fmod(instanceID, columns), floor(instanceID / columns));
                float2 base = corner + a_position;


                float2 height_uv = base / size + halfPixel; // ¹Ø¼üµã + halfPixel
                float height = tex2Dlod(u_heightMap, float4(height_uv, 0.0, 0.0)).x;

                float waterHeight = tex2Dlod(u_waterHeightMap, float4(height_uv, 0.0, 0.0)).x + u_offsetHeight;

                float value = clamp(waterHeight - height, 0.0, 1.0);


                if (value <= u_deepLevel) {
                    value = max(0.0, value - u_minDepth) / (u_deepLevel - u_minDepth);
                    o.v_color = lerp(u_minShallowColor, u_maxShallowColor, value);
                }
                else {
                    value = clamp(value - u_deepLevel, 0.0, u_maxDepth - u_deepLevel) / (u_maxDepth - u_deepLevel);
                    o.v_color = lerp(u_minDeepColor, u_maxDeepColor, value);
                }

                //float hh = (waterHeight - 1.0) / 8.0;
                //hh = value <= u_deepLevel ? 1.0 : 0.0;

                ////o.v_color.rgb = float3(hh, hh, hh);
                //o.v_color = float4(a_isWater, a_isWater, a_isWater, 1.0);
                //o.v_color = float4(hh, hh, hh, 1.0);

                float4 position = float4(base.x * u_tileSize + u_offset[0], base.y * u_tileSize + u_offset[1], waterHeight * u_tileSize, 1.0);
                position.xyz = float3(-position.y, position.z, position.x);


                o.vertex = UnityObjectToClipPos(position);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;

            }


            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = UNITY_SAMPLE_TEX2DARRAY(u_tilesets, float3(i.uv, u_tilesetsIndex));
                col *= i.v_color;
                //col.rgba = i.v_color.rgba;


                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
