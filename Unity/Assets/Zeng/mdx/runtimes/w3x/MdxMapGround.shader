Shader "Zeng/MdxMapGround"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        u_heightMap ("u_heightMap", 2D) = "white" {}
        u_tileSize("u_tileSize", Float) = 1.0
        u_tilesets ("u_tilesets", 2DArray) = "" {}
        u_tilesetsIndex("u_tilesetsIndex", Float) = 1.0
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
            
            uniform float u_tileSize;
            uniform float u_size[2];
            uniform float u_offset[2];
            uniform float u_tilesetsIndex;
            uniform float u_baseTileset;
            uniform float u_extended[14];

            uniform sampler2D  u_tileset_0;
            uniform sampler2D  u_tileset_1;
            uniform sampler2D  u_tileset_2;
            uniform sampler2D  u_tileset_3;
            uniform sampler2D  u_tileset_4;
            uniform sampler2D  u_tileset_5;
            uniform sampler2D  u_tileset_6;
            uniform sampler2D  u_tileset_7;
            uniform sampler2D  u_tileset_8;
            uniform sampler2D  u_tileset_9;
            uniform sampler2D  u_tileset_10;
            uniform sampler2D  u_tileset_11;
            uniform sampler2D  u_tileset_12;
            uniform sampler2D  u_tileset_13;
            uniform sampler2D  u_tileset_14;
            
            
            UNITY_DECLARE_TEX2DARRAY(u_tilesets);
            
            
            StructuredBuffer<float> cornerHeightsBuffer;
            StructuredBuffer<float> cornerTexturesBuffer;
            StructuredBuffer<float> cornerVariationsBuffer;

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

                float2 u_pixel = 1.0 / size;
                float2 halfPixel = u_pixel * 0.5;

                float2 c = float2(fmod(instanceID, columns), floor(instanceID / (columns)));
                int i =int( c.y * (columns-1) +  c.x);

                float4 textureIndexs = float4(
                        cornerTexturesBuffer[i * 4 + 0],
                        cornerTexturesBuffer[i * 4 + 1],
                        cornerTexturesBuffer[i * 4 + 2],
                        cornerTexturesBuffer[i * 4 + 3]
                    );
                textureIndexs = textureIndexs - u_baseTileset;

                if (textureIndexs.x > 0.0 || textureIndexs.y > 0.0 || textureIndexs.z > 0.0 || textureIndexs.w > 0.0) 
                {
                    o.textureIndexs = textureIndexs;

                    float4 a_variations = float4(
                        cornerVariationsBuffer[i * 4 + 0],
                        cornerVariationsBuffer[i * 4 + 1],
                        cornerVariationsBuffer[i * 4 + 2],
                        cornerVariationsBuffer[i * 4 + 3]
                        );

                    o.uv1 = getUV(a_position, u_extended[int(textureIndexs.x - 1)], a_variations.x);
                    o.uv2 = getUV(a_position, u_extended[int(textureIndexs.y - 1)], a_variations.y);
                    o.uv3 = getUV(a_position, u_extended[int(textureIndexs.z - 1)], a_variations.z);
                    o.uv4 = getUV(a_position, u_extended[int(textureIndexs.w - 1)], a_variations.w);


                    float2 corner = float2(fmod(instanceID, columns), floor(instanceID / columns));
                    float2 base = corner + a_position;


                    float2 height_uv = base / size + halfPixel; // ¹Ø¼üµã + halfPixel
                    float height = tex2Dlod(u_heightMap, float4(height_uv, 0.0, 0.0)).x;
                    //float hh = (height - 1.0) / 8.0;
                    //o.v_color.rgb = float3(hh, hh, hh);


                    float4 position = float4(base.x * u_tileSize + u_offset[0], base.y * u_tileSize + u_offset[1], height * u_tileSize, 1.0);
                    position.xyz = float3(-position.y, position.z, position.x);

                    o.vertex = UnityObjectToClipPos(position);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o, o.vertex);
                    return o;
                }
                else {
                    o.vertex = float4(0.0, 0.0, 0.0, 0.0);
                    return o;
                }

            }

            fixed4 sampleTile2(float textureIndex, float2 uv)
            {
                int i = int(textureIndex - 0.6);
                fixed4 col = UNITY_SAMPLE_TEX2DARRAY(u_tilesets, float3(uv, i));
                return col;
            }

            fixed4 sampleTile(float textureIndex, float2 uv)
            {
                int i = int(textureIndex - 0.6);

                if (i == 0) {
                    return tex2D(u_tileset_0, uv);
                }
                else if (i == 1) {
                    return tex2D(u_tileset_1, uv);
                }
                else if (i == 2) {
                    return tex2D(u_tileset_2, uv);
                }
                else if (i == 3) {
                    return tex2D(u_tileset_3, uv);
                }
                else if (i == 4) {
                    return tex2D(u_tileset_4, uv);
                }
                else if (i == 5) {
                    return tex2D(u_tileset_5, uv);
                }
                else if (i == 6) {
                    return tex2D(u_tileset_6, uv);
                }
                else if (i == 7) {
                    return tex2D(u_tileset_7, uv);
                }
                else if (i == 8) {
                    return tex2D(u_tileset_8, uv);
                }
                else if (i == 9) {
                    return tex2D(u_tileset_9, uv);
                }
                else if (i == 10) {
                    return tex2D(u_tileset_10, uv);
                }
                else if (i == 11) {
                    return tex2D(u_tileset_11, uv);
                }
                else if (i == 12) {
                    return tex2D(u_tileset_12, uv);
                }
                else if (i == 13) {
                    return tex2D(u_tileset_13, uv);
                }
                else if (i == 14) {
                    return tex2D(u_tileset_14, uv);
                }
                return fixed4(1.0, 1.0, 1.0, 1.0);
            }

            fixed4 blend(fixed4 color, float textureIndex, float2 uv)
            {
                fixed4 texel = sampleTile(textureIndex, uv);
                return lerp(color, texel, texel.a);
            }


            fixed4 frag(v2f i) : SV_Target
            {
                //fixed4 col = tex2D(_MainTex, i.uv);

                fixed4 col = sampleTile(i.textureIndexs.x, i.uv1);
                
                //col.rgba = 0.0;
                if(i.textureIndexs.y > 0){
                    col = blend(col, i.textureIndexs.y,  i.uv2);
                }
                
                if(i.textureIndexs.z > 0.5){
                    col = blend(col, i.textureIndexs.z,  i.uv3);
                }
                
                if(i.textureIndexs.w > 0.5){
                    col = blend(col, i.textureIndexs.w,  i.uv4);
                }


                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
