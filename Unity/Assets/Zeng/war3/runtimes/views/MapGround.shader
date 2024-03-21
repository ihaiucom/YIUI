Shader "Zeng/war3/MapGround"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        u_war3unitRate("u_war3unitRate", Float) = 128 // war3 和u3d的单位比例关系
        u_size_offset("u_size_offset", Vector) = (10, 10, 0, 0)
        u_heightMap ("u_heightMap", 2D) = "white" {} // 高度贴图
        //u_tilesets ("u_tilesets", 2DArray) = "" {} // 地面贴图数组
        
        // 地面贴图列表
        u_tileset_0 ("u_tileset_0", 2D) = "white" {}
        u_tileset_1 ("u_tileset_1", 2D) = "white" {}
        u_tileset_2 ("u_tileset_2", 2D) = "white" {}
        u_tileset_3 ("u_tileset_3", 2D) = "white" {}
        u_tileset_4 ("u_tileset_4", 2D) = "white" {}
        u_tileset_5 ("u_tileset_5", 2D) = "white" {}
        u_tileset_6 ("u_tileset_6", 2D) = "white" {}
        u_tileset_7 ("u_tileset_7", 2D) = "white" {}
        u_tileset_8 ("u_tileset_8", 2D) = "white" {}
        u_tileset_9 ("u_tileset_9", 2D) = "white" {}
        u_tileset_10 ("u_tileset_10", 2D) = "white" {}
        u_tileset_11 ("u_tileset_11", 2D) = "white" {}
        u_tileset_12 ("u_tileset_12", 2D) = "white" {}
        u_tileset_13 ("u_tileset_13", 2D) = "white" {}
        u_tileset_14 ("u_tileset_14", 2D) = "white" {}
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;

                float4 v_textureIndexs:COLOR;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float2 uv3 : TEXCOORD3;
                float2 uv4 : TEXCOORD4;

                float4 v_color: COLOR1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            uniform float u_war3unitRate;
            uniform float4 u_size_offset;

            //StructuredBuffer<float> cornerHeightsBuffer; // 高度数据Buffer
            sampler2D u_heightMap; // 高度贴图

            StructuredBuffer<float> cornerTexturesBuffer; // 贴图索引 Buffer
            StructuredBuffer<float> cornerVariationsBuffer; // // 贴图Cell UV 索引
            uniform float u_extended[14]; // 贴图cell 是否超过16 , 宽度大于高度一倍


            // 地面贴图列表
            //UNITY_DECLARE_TEX2DARRAY(u_tilesets);
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

                float2 u_size = u_size_offset.xy;
                float2 u_offset = u_size_offset.zw;
                
                float columns = u_size.x - 1;
                float rows = u_size.y - 1;

                
                float2 u_pixel = 1.0 / u_size;
                float2 halfPixel = u_pixel * 0.5;
                
                float2 corner = float2(fmod(instanceID, columns), floor(instanceID / columns)); // 当前实例对象 在第corner.x 列，corner.y 行
                int i =int( corner.y * (columns + 1) +  corner.x); // 我们需要重新计算一下 索引

                

                float4 v_textureIndexs = float4(
                        cornerTexturesBuffer[instanceID * 4 + 0],
                        cornerTexturesBuffer[instanceID * 4 + 1],
                        cornerTexturesBuffer[instanceID * 4 + 2],
                        cornerTexturesBuffer[instanceID * 4 + 3]
                    );

                float hasTexture = float(bool(v_textureIndexs.x + v_textureIndexs.y + v_textureIndexs.z + v_textureIndexs.w));
                    
                o.v_textureIndexs = v_textureIndexs;
                    
                float4 a_variations = float4(
                    cornerVariationsBuffer[instanceID * 4 + 0],
                    cornerVariationsBuffer[instanceID * 4 + 1],
                    cornerVariationsBuffer[instanceID * 4 + 2],
                    cornerVariationsBuffer[instanceID * 4 + 3]
                    );

                    

                o.uv1 = getUV(a_position, u_extended[int(v_textureIndexs.x - 1)], a_variations.x);
                o.uv2 = getUV(a_position, u_extended[int(v_textureIndexs.y - 1)], a_variations.y);
                o.uv3 = getUV(a_position, u_extended[int(v_textureIndexs.z - 1)], a_variations.z);
                o.uv4 = getUV(a_position, u_extended[int(v_textureIndexs.w - 1)], a_variations.w);
                
                // 平面位置
                float2 base = corner + a_position;

                // 高度
                //float height = cornerHeightsBuffer[i];
                float2 height_uv = base / u_size + halfPixel; // 关键点 + halfPixel
                float height = tex2Dlod(u_heightMap, float4(height_uv, 0.0, 0.0)).x;

                o.v_color.rgba = height;  // 为了方便查看，我们用颜色的debug

                float3 position = float3(base + u_offset / u_war3unitRate, height);
                position = float3(-position.y, position.z, position.x); // 坐标系转换

                
                o.vertex = UnityObjectToClipPos(float4(position, 1.0)) * hasTexture;
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            
            //fixed4 sampleTile2(float textureIndex, float2 uv)
            //{
            //    int i = int(textureIndex - 0.6);
            //    fixed4 col = UNITY_SAMPLE_TEX2DARRAY(u_tilesets, float3(uv, i));
            //    return col;
            //}

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
                //fixed4 col = tex2D(_MainTex, i.uv1);
                //col.rgba = i.v_color; // 为了方便查看，我们用颜色的debug
                
                fixed4 col = sampleTile(i.v_textureIndexs.x, i.uv1);
                
                //col.rgba = 0.0;
                if(i.v_textureIndexs.y > 0){
                    col = blend(col, i.v_textureIndexs.y,  i.uv2);
                }
                
                if(i.v_textureIndexs.z > 0.5){
                    col = blend(col, i.v_textureIndexs.z,  i.uv3);
                }
                
                if(i.v_textureIndexs.w > 0.5){
                    col = blend(col, i.v_textureIndexs.w,  i.uv4);
                }

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
